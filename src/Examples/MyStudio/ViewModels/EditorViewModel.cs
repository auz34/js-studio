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

            this.ScriptText = 
                string.IsNullOrEmpty(this.StudioState.CurrentProject.MainScriptFilePath) ? 
                    "// Start editing file here" : 
                    System.IO.File.ReadAllText(this.StudioState.CurrentProject.MainScriptFilePath); 
        }

        [Model]
        public StudioStateModel StudioState { get; private set; }

        public string ScriptText { get; set; }
    }
}
