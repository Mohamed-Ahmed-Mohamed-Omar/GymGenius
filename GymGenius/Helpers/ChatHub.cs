using Microsoft.AspNetCore.SignalR;

namespace GymGenius.Helpers
{
    public class ChatHub : Hub
    {
        public Task SendMessage1(string user, string message)               // Two parameters accepted
        {
            return Clients.All.SendAsync("ReceiveMessage", user, message);    // Note this 'ReceiveOne' 
        }
    }
}
