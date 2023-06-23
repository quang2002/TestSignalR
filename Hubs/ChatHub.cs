namespace TestSignalR.Hubs;

using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.Enums;
using TableDependency.SqlClient.Base.EventArgs;
using TestSignalR.Models;

public class ChatHub : Hub
{
    private SqlTableDependency<Chat> ChatDependency  { get; }
    public  IServiceProvider         ServiceProvider { get; }

    public ChatHub(IServiceProvider serviceProvider)
    {
        this.ServiceProvider = serviceProvider;

        using var scope     = this.ServiceProvider.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<ChatDbContext>();

        this.ChatDependency = new SqlTableDependency<Chat>(
            connectionString: dbContext.Database.GetConnectionString(),
            tableName: "Chats");

        this.ChatDependency.OnChanged += this.ChatsOnChanged;

        this.ChatDependency.Start();
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        this.ChatDependency.Stop();
        this.ChatDependency.Dispose();
    }

    private void ChatsOnChanged(object sender, RecordChangedEventArgs<Chat> e)
    {
        if (e.ChangeType != ChangeType.None)
        {
            this.Clients.All.SendAsync("receive", e.Entity.Id, e.Entity.Sender, e.Entity.Message);
        }
    }

    [HubMethodName("send")]
    public async Task SendMessage(string sender, string message)
    {
        using var       scope     = this.ServiceProvider.CreateScope();
        await using var dbContext = scope.ServiceProvider.GetRequiredService<ChatDbContext>();

        var chat = dbContext.Chats.Add(new Chat { Sender = sender, Message = message });
        await dbContext.SaveChangesAsync();

        await this.Clients.All.SendAsync("receive", chat.Entity.Id, sender, message);
    }
}