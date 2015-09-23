// -----------------------------------------------------------------------
// <copyright file="RibbonService.cs">
// Copyright (c) 2015 Andrew Zavgorodniy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyStudio.ViewModels
{
    using System.ComponentModel;

    using Catel;
    using Catel.Memento;
    using Catel.MVVM;

    using MyStudio.Models;

    public class EditorViewModel : ViewModelBase
    {
        private bool isMainScript;

        public readonly IMementoService mementoService;

        public EditorViewModel(StudioStateModel studioState,
            IMementoService mementoService)
        {
            Argument.IsNotNull(() => studioState);
            Argument.IsNotNull(() => mementoService);

            this.StudioState = studioState;
            this.StudioState.ProjectPropertyChanged += StudioState_ProjectPropertyChanged;
            this.mementoService = mementoService;
            this.isMainScript = true;
        }

        void StudioState_ProjectPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "MainScript")
            {
                this.RaisePropertyChanged(() => this.ScriptText);
            }
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
                    this.mementoService.Add(
                        new PropertyChangeUndo(
                            this.StudioState.CurrentProject,
                            "MainScript",
                            this.StudioState.CurrentProject.MainScript,
                            value));

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