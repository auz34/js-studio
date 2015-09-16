namespace MyStudio.ViewModels
{
    using Catel;
    using Catel.MVVM;

    using MyStudio.Models;
    using MyStudio.Services;

    public class RibbonViewModel : ViewModelBase
    {
        public RibbonViewModel(StudioStateModel model,
            ICommandsService commandsService)
        {
            Argument.IsNotNull(() => model);
            Argument.IsNotNull(() => commandsService);
            
        }

        
    }
}
