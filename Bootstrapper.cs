using DungeonCrawlerGame.Pages;
using Stylet;
using StyletIoC;

namespace DungeonCrawlerGame
{
    public class Bootstrapper : Bootstrapper<ShellViewModel>
    {
        protected override void ConfigureIoC(IStyletIoCBuilder builder)
        {
            // Configure the IoC container in here
        }

        protected override void Configure()
        {
            // Perform any other configuration before the application starts
        }

#if DEBUG
        protected override void OnStart()
        {
            Stylet.Logging.LogManager.Enabled = true;
        }
#endif
    }
}
