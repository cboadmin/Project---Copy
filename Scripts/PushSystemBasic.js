var PushSystemBasic = {
    Notification: function (options) {
        var Onload = true;
        var pbConfigdefaults = {
			'WsURL': 'ws://localhost:33056/api/WS?key=1234'
        };
        var t;
        var config = {}
        t = this;
        config = $.extend({}, pbConfigdefaults, options);
        var initialize = false;
        var AutoRecieve = true;

        function getRootUrl() {
            var defaultPorts = { "http:": 80, "https:": 443 };

            return window.location.protocol + "//" + window.location.hostname
                       + (((window.location.port)
                        && (window.location.port != defaultPorts[window.location.protocol]))
                        ? (":" + window.location.port) : "");
        }
        function sendMessage(msg) {
            // Wait until the state of the socket is not ready and send the message when it is...
            waitForSocketConnection(sws, function () {
                //console.log("message sent!!!");
                sws.send(msg);
            });
        }

        function waitForSocketConnection(socket, callback) {
            setTimeout(
                function () {
                    if (socket.readyState === 1) {
                        //console.log("Connection is made")
                        if (callback != null) {
                            callback();
                        }
                        return;

                    } else {
                        //console.log("wait for connection...")
                        waitForSocketConnection(socket, callback);
                    }

                }, 5); // wait 5 milisecond for the connection...
        }
        function Connect() {
            try {
                //sws = new ReconnectingWebSocket("ws://hscitp.hscwarranty.com/api/CallApi");
				sws = new ReconnectingWebSocket(config.WsURL);
				var u = sws.url;
				//sws = new WebSocket(config.WsURL);

                //sws = new ReconnectingWebSocket("ws://QA.pmc.hscwarranty/api/CallApi");
                sws.onopen = function () {

                    //sendMessage(config.User);
                    if (initialize == false) {
                        initialize = true;
                        $('#msgsws').trigger('SocketReady_' + config.User);

                    }
                };
                sws.onmessage = function (evt) {;

                    $('#msgsws').trigger('DataArival', evt.data);

                };
                sws.onerror = function (evt) {
                    var s = '';
                };
                sws.onclose = function () {

                };

            }
            catch (err) {

            }

        };

        Connect();


		this.UpdateAuthKey = function (newKeyurl) {
			sws.url = newKeyurl;
		}
		this.SendMessage = function (squery) {
            sendMessage(squery);

        }
    }
}