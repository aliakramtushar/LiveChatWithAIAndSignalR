using Microsoft.AspNetCore.SignalR;
using SignalRChatSystem.Services;

namespace SignalRChatSystem.Hubs
{
    public class ChatHub : Hub
    {
        private readonly OpenRouterService _aiService = new OpenRouterService();

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        public async Task SendMessageWithAi(string user, string message)
        {
            // Broadcast user message
            await Clients.All.SendAsync("ReceiveMessage", user, message);

            // Call AI
            var aiReply = await _aiService.GetAIResponse(message);

            // Broadcast AI reply
            await Clients.All.SendAsync("ReceiveMessage", "AI", aiReply);
        }
    }
}
