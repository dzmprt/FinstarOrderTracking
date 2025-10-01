using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;

namespace FOT.OrderChangedWorker;

public class WebSocketConnectionManager
{
    private readonly ConcurrentDictionary<string, WebSocket> _sockets = new();

    public void AddSocket(string id, WebSocket socket) => _sockets.TryAdd(id, socket);
    
    public void RemoveSocket(string id) => _sockets.TryRemove(id, out var value);

    public async Task BroadcastAsync(string message)
    {
        var buffer = Encoding.UTF8.GetBytes(message);
        foreach (var socket in _sockets.Values)
        {
            if (socket.State == WebSocketState.Open)
                await socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}