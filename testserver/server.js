var express = require('express');
var app = express();

var crypto = require('crypto');

fs = require('fs');

app.listen(3000);
console.log('Listening on port 3000');

app.get('/hello.txt', function(req, res) {
    var body = 'Hello World';
    res.setHeader('Content-Type', 'text/plain');
    res.setHeader('Content-Length', body.length);
    res.end(body);
});

// object with Cache-control set to no-cache
app.get('/nocache.jpg', function(req, res) {
    console.log('\n=== Request for item nocache.jpg with Cache-control: no-cache')
    var img = fs.readFileSync('./images/nocache.jpg');

    console.log(req['headers']);

    res.setHeader('Cache-control', 'no-cache');
    res.setHeader('Content-Type', 'image/jpg');
    res.setHeader('Content-Length', img.length);
    res.setHeader('ETag', 'dontevercachethis 1');
    res.end(img);

    console.log('===');
});

// object with Cache-control set to max-age=0
app.get('/stale.jpg', function(req, res) {
    console.log('\n=== Request for item stale.jpg with Cache-control: max-age=0')
    var img = fs.readFileSync('./images/nocache.jpg');

    console.log(req['headers']);

    res.setHeader('Cache-control', 'max-age=0');
    res.setHeader('Content-Type', 'image/jpg');
    res.setHeader('Content-Length', img.length);
    res.setHeader('ETag', 'dontevercachethis 2');
    res.end(img);

    console.log('===');
});

// object with Cache-control set to max-age=(x minutes)
app.get('/cachecontrol-:int(\\d+)min.jpg', function(req, res) {
    var minutes = req.params.int;
    var seconds = minutes * 60;

    console.log('\n=== Request for item cachecontrol-' + minutes + 'min.jpg with Cache-control: max-age=' + seconds)
    var img = fs.readFileSync('./images/ulv.jpg');

    res.setHeader('Cache-control', 'max-age=' + seconds);
    res.setHeader('Content-Type', 'image/jpg');
    res.setHeader('Content-Length', img.length);
    res.end(img);

    console.log(req['headers']);
    console.log('===');
});

// object with Cache-control set to max-age=(x seconds)
app.get('/cachecontrol-:int(\\d+)sec.jpg', function(req, res) {
    var seconds = req.params.int;

    console.log('\n=== Request for item cachecontrol-' + seconds + 'sec.jpg with Cache-control: max-age=' + seconds)
    var img = fs.readFileSync('./images/ulv.jpg');

    res.setHeader('Cache-control', 'max-age=' + seconds);
    res.setHeader('Content-Type', 'image/jpg');
    res.setHeader('Content-Length', img.length);
    res.end(img);

    console.log(req['headers']);
    console.log('===');
});

// object with Expires set to x min in the future
app.get('/expires-:int(\\d+)min.jpg', function(req, res) {
    console.log('here');

    var minutes = req.params.int;

    var d = new Date(new Date().getTime() + minutes*60*1000);

    console.log('\n=== Request for item expires-' + minutes + 'min.jpg with Expires: ' + d.toUTCString())
    var img = fs.readFileSync('./images/elg.jpg');

    console.log(req['headers']);

    res.setHeader('Expires', d.toUTCString());
    res.setHeader('Content-Type', 'image/jpg');
    res.setHeader('Content-Length', img.length);
    res.end(img);

    console.log('===');
});

// object with Expires set to x seconds in the future
app.get('/expires-:int(\\d+)sec.jpg', function(req, res) {
    var seconds = req.params.int;

    var d = new Date(new Date().getTime() + seconds*1000);

    console.log('\n=== Request for item expires-' + seconds + 'sec.jpg with Expires: ' + d.toUTCString())
    var img = fs.readFileSync('./images/elg.jpg');

    console.log(req['headers']);

    res.setHeader('Expires', d.toUTCString());
    res.setHeader('Content-Type', 'image/jpg');
    res.setHeader('Content-Length', img.length);
    res.end(img);

    console.log('===');
});

// numbered object
app.get('/item-:int(\\d+).jpg', function(req, res) {
    console.log("foobar");
    var item = req.params.int;

    console.log('\n=== Request for item item-' + item + '.jpg')
    var img = fs.readFileSync('./images/ulv.jpg');

    res.setHeader('ETag', 'numbered-object-' + item);
    res.end(img);

    console.log(req['headers']);
    console.log('===');
});

// remaining images to get with ETAG
app.get('/:img.jpg', function(req, res) {
    console.log("\n=== Request for item " + req.params.img + "that has ETAG")

    var shasum = crypto.createHash('sha1');
    shasum.update(req.params.img);
    var etag = '"' + shasum.digest('hex') + '"';

    try {
        var img = fs.readFileSync('./images/' + req.params.img + '.jpg');
    }
    catch (e) {
        res.send(404);
        console.log('===');
        return;
    }

    console.log(req['headers']);

    if (req.headers['if-none-match'] == etag) {
        res.send(304);
    }
    else {
        res.setHeader('Content-Type', 'image/jpg');
        res.setHeader('Content-Length', img.length);
        res.setHeader('ETag', etag);
        res.end(img);
    }

    console.log('===');
});
