Windows Cache component provides lightweight solution for cache, using fast protobuf serialization. Allows to store any .NET objects in cache.

WebCache allows to cache web requested data, uses SQLite.

Currently this repo contains two solutions Windows Cache and Web Cache. They can be merged in a future.

WINDOWS CACHE
=============
Set of cache components:

* PCL library `Framework.Cache`
* Windows desktop `Framework.Cache.Desktop`
* Windows store 8.0 `Framework.Cache.WindowsStore`
* Windows store 8.1 ..TBD
* Windows phone ..TBD

Nuget packages
===========

Cache components can be available from nuget server. Please find nuspec files in folders for each library.

Build server
===========
Script to run unit tests for windows store applications is included, please see `Tools` folder

WEB CACHE MANAGER
=============

Initial version.

Prerequisites:

* [SQLite for WinRT ( >= 3.8.2)](http://visualstudiogallery.msdn.microsoft.com/23f6c55a-4909-4b1f-80b1-25792b11639e)


Test server
===========

A simple test server that generates test cases with various useful combinations
of http headers is found in testserver. 

To install
----------

        cd WebCache\testserver
        npm install

To run
------

        node server.js

--or--

        nodemon server.js
