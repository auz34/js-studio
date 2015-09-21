namespace MyStudio.ViewModels
{
    using Catel;
    using Catel.MVVM;

    using MyStudio.Models;
    using MyStudio.Services;

    public class RibbonViewModel : ViewModelBase
    {
        private StudioStateModel model;

        private ICommandsService commandsService;

        public RibbonViewModel(StudioStateModel model,
            ICommandsService commandsService)
        {
            Argument.IsNotNull(() => model);
            Argument.IsNotNull(() => commandsService);

            this.model = model;
            this.commandsService = commandsService;
        }


        public Command StartCommand
        {
            get
            {
                return this.commandsService != null ? this.commandsService.StartCommand : null;
            }
        }

        public Command UndoCommand
        {
            get
            {
                return this.commandsService != null ? this.commandsService.UndoCommand : null;
            }
        }

        public Command RedoCommand
        {
            get
            {
                return this.commandsService != null ? this.commandsService.UndoCommand : null;
            }
        }
    }
}
