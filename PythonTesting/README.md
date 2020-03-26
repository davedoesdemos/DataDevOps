# Data Platform DevOps - Testing

**Produced by Dave Lusty and Cindy Weng**

## Introduction

This demo shows how to use DevOps pipelines to run automated testing in Azure DevOps with Python testing carried out using PyTest. The video is [not available yet](https://youtu.be/R7tJZelEt-Q )

There are multiple tasks associated with this demo:

* Create the test project in Python
* Write individual tests around your testing scenarios
* Set up the tests in Azure DevOps
* Import the test results to the pipeline

## Pytest File

```Python
import pytest

def test_method1():
    x=2
    y=3
    assert x+1 == y,"test failed"

def test_method2():
    x=3
    y=6
    assert x+1 == y,"test failed" 

def test_method3():
    x=2
    y=3
    assert x+0 == y,"test failed"
```

## batch file
```
pip install pytest
pytest PythonTesting\Code\tests.py --junitxml=PythonTesting\Code\results.xml
```

continue on error