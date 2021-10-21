# Data Platform DevOps - Testing

**Produced by Dave Lusty**

## Introduction

This demo shows some example tests you can use against your data environment. The video is [not available yet](https://youtu.be/R7tJZelEt-Q )

There are multiple tasks associated with this demo:

* Create the test project in Visual Studio (shown in testing intro demo)
* Write individual tests around your testing scenarios
* Set up the tests in Azure DevOps (shown in testing intro demo)

## Create Projects in Visual Studio

### Parameters

By using a run settings file, we can add parameters to the test suite. While it may seem like a great idea to use these to control what tests are run, in reality for data testing purposes we're mostly just making the code secure and enabling Azure Key Vault to hold all of the secrets we'll need such as connection strings and keys. In the examples here I have also used a parameter for a query, but this was just for convenience while testing and also makes the code reusable between projects without making too many changes.

Runsettings, from a parameter perspecive are just key-value pairs as you can see in the example code below. Note that this is XML and so requires proper structure to open/close the tags.

```xml
<?xml version="1.0" encoding="utf-8"?>
  <RunSettings>
  
  <TestRunParameters>
    <Parameter name="searchString" value="set this in Azure DevOps Pipeline" />
    <Parameter name="storageConnectionString" value="set this in Azure DevOps Pipeline" />
    <Parameter name="containerName" value="set this in Azure DevOps Pipeline" />
    <Parameter name="SQLConnectionString" value="" />
    <Parameter name="SqlQuery" value="" />
    <Parameter name="SqlQuery" value="" />
  </TestRunParameters>
  </RunSettings>
```

To utilise these we need to configure the TestContext within the class. This reads the file and sets up the values to allow us to read them.

```csharp
namespace DataOpsTesting
{
    [TestClass]
    public class DataTests
    {
        //get the parameters from the runsettings file
        public TestContext TestContext { get; set; }
```

Within the test method we can then read the properties into variables as required. In this example we read in some strings for the Blob connection.

```csharp
        [TestMethod]
        public void BlobTestCount()
        {
            //This test counts the number of objects in Blob

            string containerName = this.TestContext.Properties["containerName"].ToString();
            string StorageConnectionString = this.TestContext.Properties["storageConnectionString"].ToString();
```

## Writing Tests

### SQL Schema Tests

```csharp
        public void SQLTestColumnType()
        {
            //This test checks the data type of the specified column
            string SQLConnectionString = this.TestContext.Properties["SQLConnectionString"].ToString();
            //string SQLConnectionString = this.TestContext.Properties["SQLConnectionString"].ToString();
            string SqlQuery = "SELECT DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '<tablename>' AND COLUMN_NAME = '<columnname>'";

            //Set up the connection to SQL
            SqlConnection SqlDb;
            SqlDb = new SqlConnection(SQLConnectionString);
            SqlDb.Open();

            //Run the query
            SqlCommand command;
            SqlDataReader dataReader;
            command = new SqlCommand(SqlQuery, SqlDb);
            dataReader = command.ExecuteReader();

            //Set up a string value to hold the result
            string dataValue = "";
            if (dataReader.Read())
            {
                dataValue = dataReader[0].ToString();
            }
            Regex myRegExToMatch = new Regex("^varchar$"); //this is the column type expected
            StringAssert.Matches(dataValue, myRegExToMatch, "Incorrect Column Type");

            //Close the connection to SQL
            SqlDb.Close();
        }
```

### SQL String Match

```csharp
        [TestMethod]
        public void SQLTestString()
        {
            //This test connects to SQL or SQL DW and runs a query to test if the response matches a string

            //Get the variables and connection details
            string SQLConnectionString = this.TestContext.Properties["SQLConnectionString"].ToString();
            string SqlQuery = this.TestContext.Properties["SqlQuery"].ToString();

            //Set up the connection to SQL
            SqlConnection SqlDb;
            SqlDb = new SqlConnection(SQLConnectionString);
            SqlDb.Open();

            //Run the query
            SqlCommand command;
            SqlDataReader dataReader;
            command = new SqlCommand(SqlQuery, SqlDb);
            dataReader = command.ExecuteReader();

            //Set up a string value to hold the result
            string dataValue = "";
            if (dataReader.Read())
            {
                dataValue = dataReader[0].ToString();
            }
            Regex myRegExToMatch = new Regex("^Jim$");
            StringAssert.Matches(dataValue, myRegExToMatch, "Characters don't match");

            //Close the connection to SQL
            SqlDb.Close();
        }
```

### SQL Row Count

```csharp
        [TestMethod]
        public void SQLTestCount()
        {
            //This test connects to SQL or SQL DW and runs a query to test if the correct number of rows are returned

            //Get the variables and connection details
            string SQLConnectionString = this.TestContext.Properties["SQLConnectionString"].ToString();
            string SqlQuery = this.TestContext.Properties["SqlQuery2"].ToString();

            //Set up the connection to SQL
            SqlConnection SqlDb;
            SqlDb = new SqlConnection(SQLConnectionString);
            SqlDb.Open();

            //Run the query
            SqlCommand command;
            SqlDataReader dataReader;
            command = new SqlCommand(SqlQuery, SqlDb);
            dataReader = command.ExecuteReader();

            //Set up a counter to count the rows
            int countItems = 0;



            if (dataReader.Read())
            {
                countItems = System.Convert.ToInt32(dataReader[0].ToString());
            }
            Assert.AreEqual(2, countItems, 0, "Count doesn't match");


            //Close the connection to SQL
            SqlDb.Close();
        }
```

### Blob Count

```csharp
        [TestMethod]
        public void BlobTestCount()
        {
            //This test counts the number of objects in Blob

            string containerName = this.TestContext.Properties["containerName"].ToString();
            string StorageConnectionString = this.TestContext.Properties["storageConnectionString"].ToString();
            int blobCount = 0;

            // Retrieve storage account information from connection string
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(StorageConnectionString);
            //storageAccountConn = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            // Create a blob client for interacting with the blob service.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            // Get a container for listing blobs within the storage account.
            CloudBlobContainer container = blobClient.GetContainerReference(containerName);
            //count total blobs
            foreach (IListBlobItem blob in container.ListBlobs())
            {

                blobCount++;
            }
            Assert.AreEqual(1, blobCount, 0, "count doesn't match");
        }
```

## Testing in Azure DevOps
