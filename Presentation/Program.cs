using Microsoft.Extensions.DependencyInjection;
using Presentation.Interfaces;

namespace Presentation
{
    class Program
    {
        static void Main(string[] args)
        {

            var services = new ServiceCollection();
            Infrastructure.ServiceExtensions.ConfigureServices(services);
            Application.ServiceExtensions.ConfigureServices(services);
            ServiceExtensions.ConfigureServices(services);
            var serviceProvider = services.BuildServiceProvider();


            var mainMenuManager = serviceProvider.GetService<IMainMenuManager>();
            mainMenuManager?.DisplayMainMenu();
        }
    }
}
