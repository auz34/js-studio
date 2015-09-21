namespace MyStudio.ViewModels
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.Tracing;

    using Catel;
    using Catel.MVVM;

    using MyStudio.Models;

    public class EditorViewModel : ViewModelBase
    {
        private bool isMainScript;

        public EditorViewModel(StudioStateModel studioState)
        {
            Argument.IsNotNull(() => studioState);

            this.StudioState = studioState;
            this.isMainScript = true;
        }

        [Model]
        public StudioStateModel StudioState { get; private set; }

        public string ScriptText
        {
            get
            {
                return this.isMainScript ? this.StudioState.CurrentProject.MainScript : string.Empty;
            }

            set
            {
                if (this.ScriptText == value)
                {
                    return;
                }

                if (this.isMainScript)
                {
                    this.StudioState.CurrentProject.MainScript = value;
                }
            }
        }

        protected override void OnModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnModelPropertyChanged(sender, e);
            if (e.PropertyName == "CurrentProject")
            {
                this.RaisePropertyChanged(() => this.ScriptText);
            }
        }
    }
}