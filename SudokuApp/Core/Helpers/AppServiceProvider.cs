using Microsoft.Extensions.DependencyInjection;
using SudokuApp.Core.Contexes;
using SudokuApp.Core.Services;

namespace SudokuApp.Core.Helpers;

public static class AppServiceProvider
{
    public static ServiceProvider ServiceProvider { get; private set; }

    public static void Initialize()
    {
        var services = new ServiceCollection();

        services.AddDbContext<SudokuContext>();
        services.AddTransient<SolutionService>();

        ServiceProvider = services.BuildServiceProvider();
    }
}
