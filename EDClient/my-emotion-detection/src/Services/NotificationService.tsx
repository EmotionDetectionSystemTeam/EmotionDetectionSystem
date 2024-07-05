import { storage } from "./SessionService";

const f = (onMessage) => {
  const x = storage.getItem("address");

  if (x != null) {
    const newWs = new WebSocket(x);
    addEventListener(newWs, "message", function (event: any) {
      var data = JSON.parse(event.data);
      //alertFunc("Message from server:\n" + data.message);
      onMessage(data)

    });

    addEventListener(newWs, "open", function (event: any) {
    });

    addEventListener(newWs, "close", function (event: any) { });
    return newWs;
  }

  return null;
};
interface Conn {
  ws: WebSocket | null;
}

// Initialize the connection object with the correct type
const conn: Conn = { ws: null };

export function alertFunc(m: string) {
  alert(m);
}
export function initWebSocket(address: string, onMessage) {
  storage.setItem("address", address);
  conn.ws = f(onMessage);
}
export function addEventListener(
  ws: WebSocket,
  listenTo: string,
  funcToExec: any
) {
  ws.addEventListener(listenTo, function (event) {
    funcToExec(event);
  });
}
