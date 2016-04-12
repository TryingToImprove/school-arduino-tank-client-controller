var host = "ws://" + document.location.hostname + ":" + document.location.port + "/ws";

function Connection() {
    this.isConnected = false;

    this.socket = new WebSocket(host);
    this.socket.onopen = function (openEvent) {
        this.isConnected = true;
        console.log("CONNECTED")
    }.bind(this);

    this.send = function (message) {
        if (this.isConnected) {
            var socketMessage = JSON.stringify(message);
            this.socket.send(socketMessage);
        }
    }
}

function Controller(connection) {
    var $controlpanel = {
        $forward: document.querySelector("#controlpanel__control_forward"),
        $left: document.querySelector("#controlpanel__control_left"),
        $right: document.querySelector("#controlpanel__control_right"),
        $backward: document.querySelector("#controlpanel__control_backward")
    };

    $controlpanel.$forward.addEventListener("mousedown", function (e) {
        e.preventDefault();

        connection.send(["move-forward engineA 255", "move-forward engineB 255"]);
    }, false);

    $controlpanel.$backward.addEventListener("mousedown", function (e) {
        e.preventDefault();

        connection.send(["move-backward engineA 255", "move-backward engineB 255"]);
    }, false);

    $controlpanel.$right.addEventListener("mousedown", function (e) {
        e.preventDefault();

        connection.send(["move-forward engineA 255", "move-backward engineB 255"]);
    }, false);

    $controlpanel.$left.addEventListener("mousedown", function (e) {
        e.preventDefault();

        connection.send(["move-backward engineA 255", "move-forward engineB 255"]);
    }, false);

    // When any of the controls are not pressed it should trigger a stop
    var stopFunc = function (e) {
        e.preventDefault();

        connection.send(["stop engineA", "stop engineB"]);
    }

    for (var propName in $controlpanel) {
        if ($controlpanel.hasOwnProperty(propName)) {
            $controlpanel[propName].addEventListener("mouseup", stopFunc, false);
        }
    }
}


var connection = new Connection();
var controller = new Controller(connection);