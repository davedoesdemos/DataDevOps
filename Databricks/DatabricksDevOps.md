# Data Platform DevOps - Azure Databricks
**Produced by Dave Lusty**

## Introduction
This demo shows how to use DevOps pipelines and branching for code promotion within Azure DevOps. The video is available [coming soon](youtu.be/CW5GXIEhePE )

<a href="https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fdavedoesdemos%2FDataDevOps%2Fmaster%2FDatabricks%2Fdeploy%2Fazuredeploy.json" target="_blank">
    <img src="http://azuredeploy.net/deploybutton.png"/>
    </a>
This button doesn't yet work!

## Setup

### Azure Key Vault

Log into your Azure Portal and click add resource. Choose Azure Key Vault then click Create.

![KeyVault](images/1.keyvault.png)

Set up a resource group for the demo and give your keyvault a unique name

![2.Create.png](images/2.Create.png)

Leave the remaining options as default and click Create.

### Azure Databricks

In the portal, add an Azure Databricks resource.

![3.Databricks.png](images/3.Databricks.png)

Give your workspace a name and select the demo resource group. Leave all other settings as default and click Create.

![4.CreateDatabricks.png](images/4.CreateDatabricks.png)

## Key Vault

![New Token](images/5.NewToken.png)

Give the token a name and select a suitable lifespan. Note that it should become a normal part of your administration to rotate these tokens. It is entirely possible to automate that process through the Token API in Databricks and the command line access of Key Vault but I won't go into that here.

![Generate Token](images/6.GenerateToken.png)

Copy the generated token now as you won't be able to access it again.

![Generated Token](images/7.GeneratedToken.png)