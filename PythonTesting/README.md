# Data Platform DevOps - Testing

**Produced by Dave Lusty and Cindy Weng**

## Introduction

This demo shows how to use DevOps pipelines to run automated testing in Azure DevOps with Python testing carried out using PyTest. The video is [not available yet](https://youtu.be/R7tJZelEt-Q )

There are multiple tasks associated with this demo:

* Create the test project in Python
* Write individual tests around your testing scenarios
* Set up the tests in Azure DevOps
* Import the test results to the pipeline

## Create Python Test File

The below code gives a very basic introduction to PyTest. For full documentation see [this page](https://docs.pytest.org/en/latest/index.html) which will include information on the various test methods available. For each Python module you create, you should also create unit tests to ensure it produces the expected results. It is generally wise to write your tests before writing your code. As an example, if you created a function to add two numbers you may want a test that calls the function with values of 2 and 3 and expects a response of 5. When working with a data lake you'll need a host of tests including testing outliers to ensure data formats are working as expected. If you're testing dates, use tests in the morning and afternoon, before and after noon and midnight, and whole hours as well as precise times. Map out your expected results first, write the tests and then create test data to match and run against.

The code below has been designed to produce one pass and two failures
```Python
import pytest
from pytest import approx

def test_method1():
    x=3
    y=2
    assert x == approx(y, 1)

def test_method2():
    x=3
    y=6
    assert x == approx(y, 1)

def test_method3():
    x=3
    y=2
    assert x == y
```

## batch file
```
pip install pytest
pytest PythonTesting\Code\tests.py --junitxml=PythonTesting\Code\results.xml
```

continue on error