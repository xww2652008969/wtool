using System.Net;
using System.Net.WebSockets;
using ECommons.DalamudServices;

public class WebSocketServer
{
    private readonly HttpListener _httpListener;
    private WebSocket _webSocket;

    public WebSocketServer(string uriPrefix)
    {
        _httpListener = new HttpListener();
        _httpListener.Prefixes.Add(uriPrefix);
    }

    public async Task StartAsync()
    {
        _httpListener.Start();
        Svc.Log.Debug("WebSocket Server started...");

        while (true)
        {
            var httpContext = await _httpListener.GetContextAsync();

            if (httpContext.Request.IsWebSocketRequest)
            {
                var webSocketContext = await httpContext.AcceptWebSocketAsync(null);
                _webSocket = webSocketContext.WebSocket;

                Svc.Log.Debug("WebSocket connection established.");
            }
            else
            {
                httpContext.Response.StatusCode = 400;
                httpContext.Response.Close();
            }
        }
    }

    // private async Task ReceiveMessagesAsync()
    // {
    //     var buffer = new byte[1024 * 4];
    //
    //     while (_webSocket.State == WebSocketState.Open)
    //     {
    //         var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
    //         if (result.MessageType == WebSocketMessageType.Close)
    //         {
    //             await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
    //             Svc.Log.Debug("WebSocket connection closed.");
    //         }
    //         else
    //         {
    //             var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
    //             Svc.Log.Debug($"Received: {message}");
    //         }
    //     }
    // }

    public void SendMessageAsync(byte[] message)
    {
        if (_webSocket?.State == WebSocketState.Open)
        {
            _webSocket.SendAsync(new ArraySegment<byte>(message), WebSocketMessageType.Binary, true,
                                 CancellationToken.None);
            Svc.Log.Debug("Message sent to client.");
        }
        else
            Svc.Log.Debug("WebSocket connection is not open.");
    }

    public void CloseAsync()
    {
        if (_webSocket?.State == WebSocketState.Open)
        {
            _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
            Svc.Log.Debug("WebSocket connection closed by server.");
        }
    }
}
