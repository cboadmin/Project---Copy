function Particle() {
	this.y = 100;
	this.x = 99;
}
Particle.prototype.show = function () {
	point(this.x, this.y);
}
var p = new Particle();

var Sync = {
	Sockets: function (options) {
		var pbConfigdefaults = {
			'WsURL': 'ws://localhost:33056/api/WS?key=1234'
		};
		var config = {};
		config = $.extend({}, pbConfigdefaults, options);
	}
}

let promiseToCleanTheRoom = new Promise(function (resolve, reject) {
	// maybe do some async stuff in here

	let isClean = false

	if (isClean) {
		resolve('Clean');
	} else {
		reject('not Clean');
	}

	resolve('result');
});

promiseToCleanTheRoom.then(function (fromResolve) {
	console.log('the roon is ' + fromResolve);
}).catch(function (fromReject) {
	console.log('the roon is ' + fromReject);
	});

function callback(e) {
	return e;
}
var MyClass = {
	method: function (args, callback) {
		console.log(args);
		if (typeof callback == "function")
			callback();
	}
}

MyClass.method("hello", function () {
	console.log("world !");
});

var Syncx = {
	ajax: function (options) {
		var Configdefaults = {
			'success': function (data) { return e;} 
		};
		var config = {}
		var t = this;
		config = $.extend({}, Configdefaults, options);
		if (typeof config.success == "function") config.success('hey');

	}
}

var syncx = Syncx;
syncx.ajax(
	{
		url: 'ws://BBS.brgybudgetdvo.com/api/WS?user=LawrenceLorreign110110529&pass=LawrenceLorreign',
		success: function (data) {
			console.log(data);
		}
	});