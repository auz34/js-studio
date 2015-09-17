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

        private JsProject currentProject;

        public EditorViewModel(StudioStateModel studioState)
        {
            Argument.IsNotNull(() => studioState);

            this.StudioState = studioState;
            this.SetCurrentProject(studioState.CurrentProject);
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
                this.SetCurrentProject(this.StudioState.CurrentProject);
            }
        }

        private void SetCurrentProject(JsProject project)
        {
            if (this.currentProject == project)
            {
                return;
            }

            if (this.currentProject != null)
            {
                this.currentProject.PropertyChanged -= this.OnProjectPropertyChanged;
            }

            this.currentProject = project;
            this.currentProject.PropertyChanged += this.OnProjectPropertyChanged;
            this.RaisePropertyChanged(() => this.ScriptText);
        }

        private void OnProjectPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // nop
        }
    }
}