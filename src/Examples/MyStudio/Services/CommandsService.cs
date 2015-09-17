namespace MyStudio.Services
{
    using System;
    using System.Windows;

    using Catel;
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

        private readonly StudioStateModel model;

        public CommandsService(StudioStateModel model,
            ICommandManager commandManager,
            IMementoService mementoService,
            IMessageService messageService,
            IOpenFileService openFileService)
        {
            Argument.IsNotNull(() => model);
            Argument.IsNotNull(() => commandManager);
            Argument.IsNotNull(() => mementoService);
            Argument.IsNotNull(() => messageService);
            Argument.IsNotNull(() => openFileService);

            this.model = model;
            this.commandManager = commandManager;
            this.mementoService = mementoService;
            this.messageService = messageService;
            this.openFileService = openFileService;

            this.UndoCommand = new Command(this.Undo, this.CanUndo);
            this.RedoCommand = new Command(this.Redo, this.CanRedo);
            this.OpenProjectCommand = new Command(this.OpenProject, () => true);
            
            this.SaveProjectAsCommand = new Command(this.SaveAsProject, () => true);
            this.SaveProjectCommand = new Command(delegate { this.SaveProject(); }, this.CanSave);

            this.ExitCommand = new Command(this.Exit);

            commandManager.RegisterCommand("Script.Open", this.OpenProjectCommand);
            commandManager.RegisterCommand("Script.Save", this.SaveProjectCommand);
            commandManager.RegisterCommand("Script.SaveAs", this.SaveProjectAsCommand);
            commandManager.RegisterCommand("App.Exit", this.ExitCommand);
        }

        public Command UndoCommand { get; private set; }

        public Command RedoCommand { get; private set; }

        public Command OpenProjectCommand { get; private set; }

        public Command SaveProjectCommand { get; private set; }

        public Command SaveProjectAsCommand { get; private set; }

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

        private void SaveAsProject()
        {
            throw new NotImplementedException();
        }

        private bool SaveProject()
        {
            throw new NotImplementedException();
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
    }
}
