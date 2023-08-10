using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using TodoListManagement.Api.Common;

namespace TodoListManagement.Api.Extensions;

public static class HostExtensions
{
    public static async Task InitializeApplicationAsync(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var databaseInitializer = scope.ServiceProvider.GetRequiredService<ApplicationInitializer>();

        await databaseInitializer.InitializeAsync();
    }
}
