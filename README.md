Currently this repo contains to solutions Windows Cache and Web Cache. They can be merged in a future.

WINDOWS CACHE
=============
Set of cache components:

* PCL library `Rakuten.Framework.Cache`
* Windows desktop `Rakuten.Framework.Cache.Desktop`
* Windows store 8.0 `Rakuten.Framework.Cache.WindowsStore`
* Windows store 8.1 ..TBD
* Windows phone ..TBD

Nuget packages
===========

Cache components available as nuget packages from TeamCity server. Please find [packages source URL here](https://rakuten.atlassian.net/wiki/display/SMART/Access+TeamCity+nuget+server)

Build server
===========
Build server is on [p21448 intra machine](http://p21448/)
For more details on TeamCity CI server please refer to [TeamCity documentation](https://rakuten.atlassian.net/wiki/display/SMART/Build+Servers)

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






