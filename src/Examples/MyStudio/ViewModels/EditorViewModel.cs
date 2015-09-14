namespace MyStudio.ViewModels
{
    using Catel;
    using Catel.MVVM;

    using MyStudio.Models;

    public class EditorViewModel : ViewModelBase
    {
        public EditorViewModel(StudioStateModel studioState)
        {
            Argument.IsNotNull(() => studioState);

            this.StudioState = studioState;

            if (string.IsNullOrEmpty(this.StudioState.CurrentProject.MainScriptFilePath))
            {
                this.ScriptText = "// Start editing file here";
            } 
        }

        [Model]
        public StudioStateModel StudioState { get; private set; }

        public string ScriptText { get; set; }
    }
}
