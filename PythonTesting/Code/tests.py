import pytest
from pytest import approx

def test_method1():
    x=2
    y=3
    assert x+1 == approx(y, 1),"test failed"

def test_method2():
    x=3
    y=6
    assert x+1 == y,"test failed" 

def test_method3():
    x=2
    y=3
    assert x+0 == y,"test failed"