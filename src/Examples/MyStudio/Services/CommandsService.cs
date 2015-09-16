namespace MyStudio.Services
{
    using System;

    using Catel;
    using Catel.Memento;
    using Catel.MVVM;
    using Catel.Services;

    using MyStudio.Models;

    public class CommandsService : ICommandsService
    {
        private readonly IMementoService mementoService;

        private readonly IMessageService messageService;

        private readonly StudioStateModel model;

        public CommandsService(StudioStateModel model, 
            IMementoService mementoService,
            IMessageService messageService)
        {
            Argument.IsNotNull(() => model);
            Argument.IsNotNull(() => mementoService);
            Argument.IsNotNull(() => messageService);

            this.model = model;
            this.mementoService = mementoService;
            this.messageService = messageService;

            this.UndoCommand = new Command(this.Undo, this.CanUndo);
            this.RedoCommand = new Command(this.Redo, this.CanRedo);
            this.OpenProjectCommand = new Command(this.OpenProject, () => true);
            this.SaveProjectCommand = new Command(delegate { this.SaveProject(); }, this.CanSave);
        }

        public Command UndoCommand { get; private set; }

        public Command RedoCommand { get; private set; }

        public Command OpenProjectCommand { get; private set; }

        public Command SaveProjectCommand { get; private set; }

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
                var messageResult = await this.messageService.ShowWarningAsync("Do you want to save changes Current project contains");
                if (messageResult == MessageResult.OK)
                {
                    if (!this.SaveProject())
                    {
                        return;
                    }
                }

                throw new NotImplementedException();
            }
        }

        private bool SaveProject()
        {
            throw new NotImplementedException();
        }

        private bool CanSave()
        {
            return this.mementoService.CanUndo || this.model.CurrentProject.HasChanges;
        }
    }
}
