namespace TestSignalR.TableDependencies;

using Microsoft.EntityFrameworkCore;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.EventArgs;

public abstract class TableDependencyService<TEntity, TDbContext> : IDisposable
    where TEntity : class, new()
    where TDbContext : DbContext
{
    protected SqlTableDependency<TEntity>? Dependency { get; }

    public TableDependencyService(TDbContext dbContext)
    {
        this.Dependency = new SqlTableDependency<TEntity>(dbContext.Database.GetConnectionString());

        this.Dependency.OnChanged += this.OnChanged;

        this.Dependency.Start();
    }

    protected abstract void OnChanged(object sender, RecordChangedEventArgs<TEntity> e);

    public void Dispose()
    {
        this.Dependency?.Stop();
        this.Dependency?.Dispose();
    }
}