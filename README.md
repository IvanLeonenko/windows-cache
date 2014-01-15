Currently this repo contains to solutions Windows Cache and Web Cache. They can be merged in a future.

WINDOWS CACHE
=============
Set of cache components:

* PCL library `Rakuten.Framework.Cache`
* Windows desktop `Rakuten.Framework.Cache.Desktop`
* Windows store ..
* Windows phone ..

Build server
===========
Build server is on [p20672 intra machine](http://p20672:88/)


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






