using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace RuyaOptik.API.Hubs
{
    [Authorize(Roles = "Admin")] // JWT ile bağlanmak zorunda
    public class OrdersHub : Hub
    {
        public const string AdminGroup = "admins";

        public override async Task OnConnectedAsync()
        {
            // Admin rolündeyse admin grubuna ekle
            if (Context.User?.IsInRole("Admin") == true)
                await Groups.AddToGroupAsync(Context.ConnectionId, AdminGroup);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, AdminGroup);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
