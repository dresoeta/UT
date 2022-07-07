using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace UT.Hubs
{
    
    public class NotificationHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }
    }
}
