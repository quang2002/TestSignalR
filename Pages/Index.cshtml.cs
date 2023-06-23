namespace TestSignalR.Pages;

using Microsoft.AspNetCore.Mvc.RazorPages;
using TestSignalR.Models;

public class IndexModel : PageModel
{
    public IEnumerable<Chat> Chats { get; set; } = null!;

    public IServiceProvider ServiceProvider { get; }

    public IndexModel(IServiceProvider serviceProvider)
    {
        this.ServiceProvider = serviceProvider;
    }

    public void OnGet()
    {
        using var scope     = this.ServiceProvider.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<ChatDbContext>();
        
        this.Chats = dbContext.Chats.ToList();
    }
}