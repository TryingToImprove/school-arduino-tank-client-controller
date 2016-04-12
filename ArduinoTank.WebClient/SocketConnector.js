var host = "ws://localhost:1491/ws";

function SocketConnector(host) {
    var promise = new Promise(function (resolve) {
        var socket = new WebSocket(host);
        socket.onopen = function (openEvent) {
            resolve(socket);
        };
    });
}