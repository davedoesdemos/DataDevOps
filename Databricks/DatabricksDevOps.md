# Data Platform DevOps - Azure Databricks
**Produced by Dave Lusty**

## Introduction
This demo shows how to use DevOps pipelines and branching for code promotion within Azure DevOps. The video is [available here](https://youtu.be/R7tJZelEt-Q )

## Setup

### Create Azure Key Vault

Log into your Azure Portal and click add resource. Choose Azure Key Vault then click Create.

![KeyVault](images/1.keyvault.png)

Set up a resource group for the demo and give your keyvault a unique name

![2.Create.png](images/2.Create.png)

Leave the remaining options as default and click Create.

### Create Azure Databricks

In the portal, add an Azure Databricks resource.

![3.Databricks.png](images/3.Databricks.png)

Give your workspace a name and select the demo resource group. Leave all other settings as default and click Create.

![4.CreateDatabricks.png](images/4.CreateDatabricks.png)

### Set up Azure DevOps Project

Log in at [dev.azure.com](https://dev.azure.com) to your DevOps portal. If you're still using the old visualstudio links please update them as they can break some processes.

Click New Project and set up a project for the Databricks demo.

![11.NewDevOpsProject.png](images/11.NewDevOpsProject.png)

Click Repos and then click Initialize near the bottom to create the empty repository where we'll link our notebooks.

![12.initializeRepo.png](images/12.initializeRepo.png)

## Linking Notebooks

Log in to your Azure Databricks workspace and click on your user icon in the top right corner then select User Settings.

![13.userSettings.png](images/13.userSettings.png)

Click the Git integration tab and select Azure DevOps Services to connect to your project repository. If you prefer, Github works equally well for this purpose but you'll need an access token from GitHub to configure this securely.

![14.GitIntegration.png](images/14.GitIntegration.png)

Next, click on Workspace and right click to create a new notebook.

![15.NewNotebook.png](images/15.NewNotebook.png)

For now it doesn't matter what kind of notebook you create because we're not doing any coding just adding in some comments to show the process. Choose Python to ensure the remaining instructions and code will work, the PowerShell release script references Python and you'll need to change this if you don't choose Python. Don't select a cluster as we're not planning to run the script.

![16.NewNotebook2.png](images/16.NewNotebook2.png)

Once created, click on Git: Not Linked under Revision History.

![17.LinkGit.png](images/17.LinkGit.png)

Click Link and then type a name for your feature branch and click create branch. We'll work on this branch and use Master as the collaboration branch. Click Save to begin working.

![18.FeatureBranch.png](images/18.FeatureBranch.png)

Type a comment such as `# Feature Branch One`

![19.typeCode.png](images/19.typeCode.png)

click save on the revision history menu. Choose to also commit to Git and type a comment then click Save.

![20.firstCommit.png](images/20.firstCommit.png)

Open your repository in Azure DevOps to see the Master branch is empty and the feature branch has your comment in. From your Databricks workspace, click the Git: Synced button under Revision History. Click Create PR to create a pull request. This launches Azure DevOps (you can just create the PR in DevOps if you prefer, the link just opens the interface for you).

![21.PullRequest.png](images/21.PullRequest.png)

Select your feature branch and add comments as necessary then click create.

![22.CreatePR.png](images/22.CreatePR.png)

Approve the request and complete the merge to finish. You should now change your branch in Databricks to Master or create a new feature branch. The old feature branch will be removed during the merge.

## Setting up the Authentication Token

### Generate the token and store it in Key Vault

In Azure Databricks workspace, click your user icon and enter user settings. Under Access Tokens click Generate New Token to create the token Azure DevOps will use to securely connect to the Databricks API.

![New Token](images/5.NewToken.png)

Give the token a name and select a suitable lifespan. Note that it should become a normal part of your administration to rotate these tokens. It is entirely possible to automate that process through the Token API in Databricks and the command line access of Key Vault but I won't go into that here.

![Generate Token](images/6.GenerateToken.png)

Copy the generated token now as you won't be able to access it again.

![Generated Token](images/7.GeneratedToken.png)

Open your Key Vault and click Secrets on the menu, then Generate/Import.

![Key Vault Secrets](images/8.KeyVaultSecrets.png)

Enter your secret and give the secret a descriptive name. Add in the expiration timestamp to ensure you have this recorded centrally.

![Create Secret](images/9.CreateSecret.png)

### Load the token as a variable in Azure DevOps

In your DevOps project, click Pipelines and then Library. Next, click + Variable Group.

![New Variable Group](images/10.NewVariableGroup.png)

Give the group a name such as Azure Key Vault. Select to link secrets from an Azure Key Vault and then select your subscription and Key Vault from the drop down lists. You may need to click authorize to configure the connections. Add in the Databricks token variable and click save.

![10.NewVariableGroup2.png](images/10.NewVariableGroup2.png)

## Azure DevOps

This section explains how to create your pipelines in Azure DevOps

### Build

In Azure DevOps, click Pipelines and then Create Pipeline.

![23.createPipeline.png](images/23.createPipeline.png)

Click "Use the classic editor" to use the GUI to create this pipeline. Choose your repository and then the Master branch, since that's our collaboration branch for this build.

![24.createPipeline2.png](images/24.createPipeline2.png)

Select Empty Job

![25.emptyJob.png](images/25.emptyJob.png)

Give your build a name such as Build Databricks Artifact.

![26.newBuild.png](images/26.newBuild.png)

Click the + next to Agent Job 1 and add a Publish build artifacts task.

![27.addTask.png](images/27.addTask.png)

Select the Notebooks directory in your repository as the path to publish, and name the artifact Notebooks.

![28.artifacts.png](images/28.artifacts.png)

Click Triggers on the menu and click Enable Continuous Integration. Select your master branch.

![29.trigger.png](images/29.trigger.png)

Click Save and Queue to complete the build task and create the first build. Add a comment such as "created build job" then click save and run. Your task should now run and build the first artifact with your notebook in it.

![30.saveandqueue.png](images/30.saveandqueue.png)

### Release

Click Pipelines, Releases and create your first release pipeline.

![31.firstRelease.png](images/31.firstRelease.png)

Click start with an empty job and name the first stage Testing. You can create identical jobs for pre-prod and production later.

![32.testing.png](images/32.testing.png)

Click the add artifacts button and then select your build pipeline which will show that it last created an artifact called notebooks.

![33.addArtifact.png](images/33.addArtifact.png)

Click the lightning icon next to the artifact to enable continuous deployment.

![34.trigger.png](images/34.trigger.png)

Click tasks on the menu to set up the job. Add a PowerShell task to the pipeline.

![35.addTask.png](images/35.addTask.png)

Configure the powershell task to use inline code and paste in the below code:

```powershell
# Upload a notebook to Azure Databricks
# Docs at https://docs.microsoft.com/en-us/azure/databricks/dev-tools/api/latest/workspace#--import


$fileName = "$(System.DefaultWorkingDirectory)/<path to file in artifact>/<filename>.py"
$newNotebookName = "ImportedNotebook"
# Get our secret from the variable
$Secret = "Bearer " + "$(Databricks)"

# Set the URI of the workspace and the API endpoint
$Uri = "https://<your region>.azuredatabricks.net/api/2.0/workspace/import"

# Open and import the notebook
$BinaryContents = [System.IO.File]::ReadAllBytes($fileName)
$EncodedContents = [System.Convert]::ToBase64String($BinaryContents)

$Body = @{
    content = "$EncodedContents"
    language = "PYTHON"
    overwrite = $true
    format = "SOURCE"
    path= "/Users/<your user>/" + "$newNotebookName"
}

#Convert body to JSON
$BodyText = $Body | ConvertTo-Json

$headers = @{
    Authorization = $Secret
}

Invoke-RestMethod -Uri $uri -Method Post -Headers $headers -Body $BodyText

```
Change `<path to file in artifact>/<filename>.py` to your path inside the artifact. If you don't know this path, add a "copy files" task to the pipeline and use the browse button to select the file and then copy the path. Remove the copy files task after you've done this and pasted the path into your script.

Change `<your user>` to your user id in databricks. The variable set from Key Vault will automatically be downloaded so you don't need to do anything to use it, just reference it by name. Make sure your URI is correct for your workspace, you can see this on the overview pane in the Azure Portal when looking at the workspace.

![36.powershell.png](images/36.powershell.png)

Naturally, this script could be extended for multiple files using a foreach loop on a folder. Using a different workspace URI or different token would deploy to different workspaces. You'll probably want one for testing, one for QA or Preprod and one for production.

Finally, click Create Release on the menu. In future this won't be necessary since we set up the trigger, but since we won't have another build to start that trigger we need to manually start this one. Just click create and your tasks will run and deploy the notebook to your workspace using the $newNotebookName variable as the name. Since we only have one workspace in the demo, that's where the notebook will go.

## Next Steps

Please bear in mind that deploying notebooks is used here to demonstrate using DevOps with branches. In a production scenario, your code should be placed into libraries and deployed to the workspace with notebooks calling those libraries of tested code. The notebook in that scenario will simply act as a placeholder for parameters for the job.
I showed a single workspace here but you should end up with multiple workspaces, each acting as the trigger for the next in the release pipeline.

![37.multiplestages.png](images/37.multiplestages.png)