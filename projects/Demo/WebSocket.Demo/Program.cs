using Fleck;

new WebSocketServer("ws://0.0.0.0:8181").Start(socket =>
{
    socket.OnOpen = () => Console.WriteLine("Client connected");
    socket.OnClose = () => Console.WriteLine("Client disconnected");
    socket.OnMessage = message =>
    {
        socket.Send($"Received echo message: {message}");
    };
});

WebApplication.CreateBuilder(args).Build().Run();