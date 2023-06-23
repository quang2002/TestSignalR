namespace TestSignalR.TableDependencies;

using Microsoft.AspNetCore.SignalR;
using TableDependency.SqlClient.Base.Enums;
using TableDependency.SqlClient.Base.EventArgs;
using TestSignalR.Hubs;
using TestSignalR.Models;

public class ChatDependencyService : TableDependencyService<Chat, ChatDbContext>
{
    public ChatHub ChatHub { get; }

    public ChatDependencyService(ChatDbContext dbContext, ChatHub chatHub) : base(dbContext)
    {
        this.ChatHub = chatHub;
    }

    protected override void OnChanged(object sender, RecordChangedEventArgs<Chat> e)
    {
        if (e.ChangeType != ChangeType.None)
        {
            this.ChatHub.Clients.All.SendAsync("receive", e.Entity.Id, e.Entity.Sender, e.Entity.Message);
        }
    }
}