using DungeonCrawlerGame.Pages;
using Stylet;
using StyletIoC;
using System.Windows;

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
            builder.Bind<Services.LevelService>().ToSelf().InSingletonScope();
        }

        /// <summary>
        /// Perform any other configuration before the application starts.
        /// </summary>
        protected override void Configure()
        {
            Container.Get<Services.SettingsService>().LoadSettings();
        }

        /// <summary>
        /// Called on application exit.
        /// </summary>
        protected override void OnExit(ExitEventArgs e)
        {
            Container.Get<Services.SettingsService>().SaveSettings();
        }

        /// <summary>
        /// On application start.
        /// </summary>
        protected override void OnStart()
        {
#if DEBUG
            Stylet.Logging.LogManager.Enabled = true;
#endif
        }
    }
}
