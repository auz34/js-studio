namespace MyStudio.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using Catel.IoC;
    using Catel.Logging;
    using Catel.MVVM;

    using MyStudio.Models;

    using Orchestra.Services;
    using InputGesture = Catel.Windows.Input.InputGesture;

    public class ApplicationInitializationService : ApplicationInitializationServiceBase
    {
        #region Constants
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        #endregion

        public override async Task InitializeBeforeCreatingShellAsync()
        {
            // Non-async first
            await this.InitializeCommands();

            await RunAndWaitAsync(new Func<Task>[]
            { 
                this.InitializePerformance,
                this.InitializeMainModel
            });
        }

        private async Task InitializeCommands()
        {
            var commandManager = ServiceLocator.Default.ResolveType<ICommandManager>();

            commandManager.CreateCommand("Script.Open", new InputGesture(Key.O, ModifierKeys.Control), throwExceptionWhenCommandIsAlreadyCreated: false);
            commandManager.CreateCommand("Script.Save", new InputGesture(Key.S, ModifierKeys.Control), throwExceptionWhenCommandIsAlreadyCreated: false);
            commandManager.CreateCommand("Script.SaveAs", null, throwExceptionWhenCommandIsAlreadyCreated: false);

            commandManager.CreateCommand("App.Exit", throwExceptionWhenCommandIsAlreadyCreated: false);

            commandManager.CreateCommand("Help.About", throwExceptionWhenCommandIsAlreadyCreated: false);
        }

        public override async Task InitializeAfterCreatingShellAsync()
        {
            Log.Info("Delay to show the splash screen");
            Thread.Sleep(2500);
        }

        private async Task InitializePerformance()
        {
            Log.Info("Improving performance");

            Catel.Data.ModelBase.DefaultSuspendValidationValue = true;
            Catel.Windows.Controls.UserControl.DefaultCreateWarningAndErrorValidatorForViewModelValue = false;
            Catel.Windows.Controls.UserControl.DefaultSkipSearchingForInfoBarMessageControlValue = true;
        }

        private async Task InitializeMainModel()
        {
            var studioModel = new StudioModel();
            ServiceLocator.Default.RegisterType(m => studioModel);
        }
    }
}
