using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    //this class main purpose is to notify user when their payment is succeeded from stripe 
    [Authorize]
    public class NotificationHub : Hub
    {
      private static readonly ConcurrentDictionary<string, string> UserConnections = new();

     //connecting
    public override Task OnConnectedAsync()
    {
        var email = Context.User?.GetUserEmail(); //getting user email

        if (!string.IsNullOrEmpty(email)) //check email is null or empty
        UserConnections[email] = Context.ConnectionId; 

        return base.OnConnectedAsync();
    }

   //disconnecting
    public override Task OnDisconnectedAsync(Exception? exception)
    {
        var email = Context.User?.GetUserEmail();

        if (!string.IsNullOrEmpty(email))
         UserConnections.TryRemove(email, out _); //disconnecting key

        return base.OnDisconnectedAsync(exception);
    }

    //get connection by id 
    public static string? GetConnectionIdByEmail(string email)
    {
        UserConnections.TryGetValue(email, out var connectionId);

        return connectionId;
    }
    }
}