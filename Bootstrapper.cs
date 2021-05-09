using DungeonCrawlerGame.Pages;
using Stylet;
using StyletIoC;

namespace DungeonCrawlerGame
{
    public class Bootstrapper : Bootstrapper<ShellViewModel>
    {
        /// <summary>
        /// Configure services.
        /// </summary>
        protected override void ConfigureIoC(IStyletIoCBuilder builder)
        {
            builder.Bind<Services.SettingsService>().ToSelf().InSingletonScope();
        }

        /// <summary>
        /// Perform any other configuration before the application starts.
        /// </summary>
        protected override void Configure()
        {
        }

#if DEBUG
        /// <summary>
        /// On application start.
        /// </summary>
        protected override void OnStart() => Stylet.Logging.LogManager.Enabled = true;
#endif
    }
}
