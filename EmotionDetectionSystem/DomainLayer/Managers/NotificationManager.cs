namespace EmotionDetectionSystem.DomainLayer.Managers;

using System.Text.Json;
using WebSocketSharp.Server;

 public class NotificationManager
    {
        private static NotificationManager _notificationManager = null;
        private static object _lock = new object();
        private WebSocketServer _notificationServer;

        private NotificationManager() 
        {
        }
        public void setNotificationServer(WebSocketServer notificatinserver)
        {
            this._notificationServer = notificatinserver;
        }


        public static NotificationManager GetInstance()
        {
            if (_notificationManager != null)
                return _notificationManager;
            lock (_lock)
            {
                if (_notificationManager == null)
                    _notificationManager = new NotificationManager();
            }
            return _notificationManager;
        }

        public void SendNotification(string message, string username)
        {
            string _relativeServicePath = "/" + username + "-notifications";

            if (_notificationServer != null && _notificationServer.WebSocketServices[_relativeServicePath] != null)
            {
                lock(_lock)
                {
                    var jsonMessage = new
                    {
                        message = message
                    };

                    var json = JsonSerializer.Serialize(jsonMessage);

                    var webSocketService = _notificationServer.WebSocketServices[_relativeServicePath];

                    if (webSocketService != null && webSocketService.Sessions.Count > 0)
                    {
                        foreach (var session in webSocketService.Sessions.Sessions)
                        {
                            var webSocket = session.Context.WebSocket;
                            if (webSocket != null && webSocket.IsAlive)
                            {
                                webSocket.Send(json);
                            }
                        }
                    }
                }
            }
        }
    }