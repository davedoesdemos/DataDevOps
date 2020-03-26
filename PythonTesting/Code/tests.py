import pytest
from pytest import approx

def test_method1():
    x=3
    y=2
    assert x == approx(y, abs=1)

def test_method2():
    x=3
    y=6
    assert x == approx(y, abs=1)

def test_method3():
    x=3
    y=2
    assert x == y