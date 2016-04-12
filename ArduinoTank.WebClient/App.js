var host = "ws://" + document.location.hostname + ":" + document.location.port + "/ws";

function addEventListenerExtended($dom, eventNames, func) {
    eventNames.forEach(function (eventName) {
        $dom.addEventListener(eventName, func, false);
    });
}

function Connection() {
    this.isConnected = false;

    this.socket = new WebSocket(host);
    this.socket.onopen = function (openEvent) {
        this.isConnected = true;
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
        },
        controlpanelButtonEventFunc = function (extendedFunc) {
            return function (e) {
                e.preventDefault();

                this.classList.add("controlpanel__button--active");

                extendedFunc();
            }
        };

    addEventListenerExtended($controlpanel.$forward, ["mousedown", "touchstart", "MSPointerDown"], controlpanelButtonEventFunc(function () {
        connection.send(["move-forward engineA 255", "move-forward engineB 255"]);
    }));

    addEventListenerExtended($controlpanel.$backward, ["mousedown", "touchstart", "MSPointerDown"], controlpanelButtonEventFunc(function () {
        connection.send(["move-backward engineA 255", "move-backward engineB 255"]);
    }));

    addEventListenerExtended($controlpanel.$right, ["mousedown", "touchstart", "MSPointerDown"], controlpanelButtonEventFunc(function () {
        connection.send(["move-forward engineA 255", "move-backward engineB 255"]);
    }));

    addEventListenerExtended($controlpanel.$left, ["mousedown", "touchstart", "MSPointerDown"], controlpanelButtonEventFunc(function () {
        connection.send(["move-backward engineA 255", "move-forward engineB 255"]);
    }));

    // When any of the controls are not pressed it should trigger a stop
    var stopFunc = function (e) {
        e.preventDefault();

        this.classList.remove("controlpanel__button--active");
        connection.send(["stop engineA", "stop engineB"]);
    }

    for (var propName in $controlpanel) {
        if ($controlpanel.hasOwnProperty(propName)) {
            addEventListenerExtended($controlpanel[propName], ["mouseup", "touchend", "MSPointerUp"], stopFunc);
        }
    }
}


var connection = new Connection();
var controller = new Controller(connection);