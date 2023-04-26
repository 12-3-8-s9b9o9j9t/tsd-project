using System.Net.WebSockets;
using back.Services;
using Microsoft.AspNetCore.Mvc;

namespace back.Controllers;

public class WebSocketController : ControllerBase
{

    private readonly ISessionService _sessionService;

    public WebSocketController(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }
    
    [Route("/ws")]
    [HttpGet]
    public async Task Get()
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            _sessionService.addWS(webSocket);

            var buffer = new byte[1024 * 4];
            var receiveResult = await webSocket.ReceiveAsync(
                new ArraySegment<byte>(buffer), CancellationToken.None);

            while (!receiveResult.CloseStatus.HasValue)
            {
                receiveResult = await webSocket.ReceiveAsync(
                    new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            
            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "bieng", CancellationToken.None);
            _sessionService.removeWS(webSocket);
        }
        else
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }
}