using System;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;

/* 
    Testing samples suite
    Produced by Dave Lusty

    This file is designed to be used as an example of code you can use for various tests on your data platform.

    Useful links:
    https://docs.microsoft.com/en-us/visualstudio/test/using-the-assert-classes?view=vs-2019
    Assert functions:
    https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.testtools.unittesting.assert?redirectedfrom=MSDN&view=mstest-net-1.2.0
    String Assert functions:
    https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.testtools.unittesting.stringassert?redirectedfrom=MSDN&view=mstest-net-1.2.0
    Collection Assert functions, may be useful comparing JSON:
    https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.testtools.unittesting.collectionassert?redirectedfrom=MSDN&view=mstest-net-1.2.0
*/

namespace DataOpsTesting
{
    [TestClass]
    public class DataTests
    {
        //get the parameters from the runsettings file
        public TestContext TestContext { get; set; }
        
        [TestMethod]
        public void CharacterMap()
        {
            //This test checks that characters from input data match output data
            //Use CSV input with worldwide characters
            //Probably starts with a connection to SQL DW and a query
            String searchString = this.TestContext.Properties["searchString"].ToString();
            String inputString = "input"; //get this from a query to your data
            Regex myRegExToMatch = new Regex("^" + searchString + "$");
            StringAssert.Matches(inputString, myRegExToMatch, "Characters don't match");
        }

        [TestMethod]
        public void CharacterMap2()
        {
            //This test checks that characters from input data match output data
            //Use CSV input with worldwide characters
            //Probably starts with a connection to SQL DW and a query

            String inputString = "myṢtring"; //replace the S with a different S
            Regex myRegExToMatch = new Regex("^myString$");
            StringAssert.Matches(inputString, myRegExToMatch, "Characters don't match");
        }

        [TestMethod]
        public void BlobTestCount()
        {
            //This test counts the number of objects in Blob
            //CloudStorageAccount storageAccountConn;
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
    }
}
