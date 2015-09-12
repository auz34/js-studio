namespace MyStudio
{
    using System;
    using System.Diagnostics;
    using System.Windows;

    using Catel.IoC;
    using Catel.Logging;

    using Orchestra.Services;
    using Orchestra.Views;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Constants
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        #endregion

        #region Fields
        private readonly DateTime start;
        private readonly Stopwatch stopwatch;
        private DateTime end;
        #endregion

        #region Constructors
        public App()
        {
            this.stopwatch = new Stopwatch();
            this.stopwatch.Start();
            this.start = DateTime.Now;
        }
        #endregion

        #region Methods
        protected override void OnStartup(StartupEventArgs e)
        {
#if DEBUG
            LogManager.AddDebugListener(true);
#endif

            var serviceLocator = ServiceLocator.Default;
            var shellService = serviceLocator.ResolveType<IShellService>();
            shellService.CreateWithSplashAsync<ShellWindow>();

            this.end = DateTime.Now;
            this.stopwatch.Stop();

            Log.Info("Elapsed startup stopwatch time: {0}", this.stopwatch.Elapsed);
            Log.Info("Elapsed startup time: {0}", this.end - this.start);
        }
        #endregion
    }
}
