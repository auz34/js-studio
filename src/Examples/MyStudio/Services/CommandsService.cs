namespace MyStudio.Services
{
    using System;
    using System.Windows;

    using Catel;
    using Catel.Data;
    using Catel.Memento;
    using Catel.MVVM;
    using Catel.Services;

    using MyStudio.Models;

    public class CommandsService : ICommandsService
    {
        private readonly IMementoService mementoService;

        private readonly IMessageService messageService;

        private readonly ICommandManager commandManager;

        private readonly IOpenFileService openFileService;

        private readonly ISaveFileService saveFileService;

        private readonly StudioStateModel model;

        public CommandsService(StudioStateModel model,
            ICommandManager commandManager,
            IMementoService mementoService,
            IMessageService messageService,
            IOpenFileService openFileService,
            ISaveFileService saveFileService)
        {
            Argument.IsNotNull(() => model);
            Argument.IsNotNull(() => commandManager);
            Argument.IsNotNull(() => mementoService);
            Argument.IsNotNull(() => messageService);
            Argument.IsNotNull(() => openFileService);
            Argument.IsNotNull(() => saveFileService);

            this.model = model;
            this.commandManager = commandManager;
            this.mementoService = mementoService;
            this.messageService = messageService;
            this.openFileService = openFileService;
            this.saveFileService = saveFileService;

            this.UndoCommand = new Command(this.Undo, this.CanUndo);
            this.RedoCommand = new Command(this.Redo, this.CanRedo);
            this.OpenProjectCommand = new Command(this.OpenProject, () => true);

            this.SaveProjectAsCommand = new Command(delegate { this.SaveAsProject(); }, () => true);
            this.SaveProjectCommand = new Command(delegate { this.SaveProject(); }, this.CanSave);

            this.StartCommand = new Command(this.Start, this.CanStart);

            this.ExitCommand = new Command(this.Exit);

            commandManager.RegisterCommand("Script.Open", this.OpenProjectCommand);
            commandManager.RegisterCommand("Script.Save", this.SaveProjectCommand);
            commandManager.RegisterCommand("Script.SaveAs", this.SaveProjectAsCommand);
            commandManager.RegisterCommand("App.Exit", this.ExitCommand);

            this.model.ProjectPropertyChanged += this.OnProjectPropertyChanged;
        }

        public Command UndoCommand { get; private set; }

        public Command RedoCommand { get; private set; }

        public Command OpenProjectCommand { get; private set; }

        public Command SaveProjectCommand { get; private set; }

        public Command SaveProjectAsCommand { get; private set; }

        public Command StartCommand { get; private set; }

        /// <summary>
        /// Gets the Exit command.
        /// </summary>
        public Command ExitCommand { get; private set; }

        /// <summary>
        /// The undo.
        /// </summary>
        private void Undo()
        {
            this.mementoService.Undo();

            this.UndoCommand.RaiseCanExecuteChanged();
            this.RedoCommand.RaiseCanExecuteChanged();
        }

        private bool CanUndo()
        {
            return this.mementoService.CanUndo;
        }

        /// <summary>
        /// The redo.
        /// </summary>
        private void Redo()
        {
            this.mementoService.Redo();

            this.UndoCommand.RaiseCanExecuteChanged();
            this.RedoCommand.RaiseCanExecuteChanged();
        }

        private bool CanRedo()
        {
            return this.mementoService.CanUndo;
        }

        private async void OpenProject()
        {
            if (this.CanSave())
            {
                switch (await this.messageService.ShowAsync("Do you want to save changes?", "Save changes?", MessageButton.YesNoCancel))
                {
                    case MessageResult.Cancel:
                        return;

                    case MessageResult.Yes:
                        if (!this.SaveProject())
                        {
                            return;
                        }
                        break;

                    case MessageResult.No:
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            
            // TODO add more files if we decide to work with "projects"
            this.openFileService.Filter = "Single script files|*.js";
            if (this.openFileService.DetermineFile())
            {
                this.model.CurrentProject = new JsProject(this.openFileService.FileName);
            }
        }

        private bool SaveAsProject()
        {
            this.saveFileService.Filter = "Single script files|*.js";
            if (this.saveFileService.DetermineFile())
            {
                this.model.CurrentProject.SaveProject(this.saveFileService.FileName);
                return true;
            }

            return false;
        }

        private bool SaveProject()
        {
            if (this.model.CurrentProject.IsNew)
            {
                return this.SaveAsProject();
            }

            this.model.CurrentProject.SaveProject(null);
            return true;
        }

        private bool CanSave()
        {
            return this.mementoService.CanUndo || this.model.CurrentProject.HasChanges;
        }

        /// <summary>
        /// Method to invoke when the Exit command is executed. 
        /// </summary>
        private async void Exit()
        {
            if (this.CanSave())
            {
                switch (await this.messageService.ShowAsync("Do you want to save changes?", "Save changes?", MessageButton.YesNoCancel))
                {
                    case MessageResult.Cancel:
                        return;

                    case MessageResult.Yes:
                        if (!this.SaveProject())
                        {
                            return;
                        }
                        break;

                    case MessageResult.No:
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            Application.Current.Shutdown();
        }

        /// <summary>
        /// Starts debugging session. 
        /// </summary>
        private void Start()
        {
            this.model.StartSession();
        }

        private bool CanStart()
        {
            return !this.model.IsSessionActive;
        }

        private void OnProjectPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.SaveProjectCommand.RaiseCanExecuteChanged();
            this.UndoCommand.RaiseCanExecuteChanged();
            this.RedoCommand.RaiseCanExecuteChanged();
        }
    }
}
