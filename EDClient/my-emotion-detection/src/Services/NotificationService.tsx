import { storage } from "./SessionService";

const f = () => {
  const x = storage.getItem("address");

  if (x != null) {
    const newWs = new WebSocket(x);
    addEventListener(newWs, "message", function (event: any) {
      var data = JSON.parse(event.data);
      alertFunc("Message from server:\n" + data.message);
    });

    addEventListener(newWs, "open", function (event: any) {});

    addEventListener(newWs, "close", function (event: any) {});
    return newWs;
  }

  return null;
};
const conn: { ws: WebSocket | null } = { ws: f() };
export function alertFunc(m: string) {
  alert(m);
}
export function initWebSocket(address: string) {
  storage.setItem("address", address);
  conn.ws = f();
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
