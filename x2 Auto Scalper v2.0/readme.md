#Auto Scalper

##Introduction

This cTrader Bot has been 4 years in the making, iteration after iteration. But I am finally happy with the outcome.

Some Rules To Follow
In the OnTick() method you should only call l_ (Logic) methods, which are build using b_ (Boolean) methods and these boolean methods are built using w_ (Worker) methods, and these worker methods are built using d_ (Data) methods.
And by following these rules it makes for a easier system to return to months later and adjust things
It also means we can start building a Library for future work
It also means we can buildBots quicker and keep them clean

## Coding Notes
The design pattern to be followed:

* Everything starts with a Array, List, Collection, IndicatorList, or DataSeries. 
*Call these  **Data** and prefix all method names with a d_

* Then we have methods that do work on the Data and can either return void or other data
*Call these **Worker** and prefix all method names with a w_

* Then we have methods that return only Boolean, these methods answer one specific question and are only built with **Worker** functions. 
If you need a some data build a worker function to access it. Also we are tring to build a library so the naming needs to be universal and clear, also follow Namming Rules
*Call these **Boolean** and prefix all function name with a b_

* Then we have methods that use the Boolean methods to build and handle Logic
If you need a some data build a worker function to access it. Also we are tring to build a library so the naming needs to be universal and clear, also follow Namming Rules
*Call these **Logic** and prefix all function name with a l_