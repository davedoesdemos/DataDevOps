# Data Platform DevOps - Blue Green Deployment and test environments
**Produced by Dave Lusty**

## Introduction
This demo shows how to use blue green deployment in a data lake environment. The video is [not ready yet](https://youtu.be/CW5GXIEhePE)

When creating a DevOps strategy for a data lake the question often comes up of how to create test environments as part of a deployment. These ephemeral environments are created as a release pipeline and will often include testing both automated and manual. The environment will be destroyed as soon as it's either accepted or rejected. An alternative to this, is that the triggers for the environment are migrated and the new components remain as the new version of the dataset while the old version is destroyed.
![bluegreenoverview](images\bluegreen.png)