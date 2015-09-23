// -----------------------------------------------------------------------
// <copyright file="StudioStateModel.cs">
// Copyright (c) 2015 Andrew Zavgorodniy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyStudio.Models
{
    using System;

    using Catel.Data;
    using Catel.IoC;

    using System.ComponentModel;

    using MyStudio.Services;

    public class StudioStateModel : ModelBase
    {
        private JsProject currentProject;

        public StudioStateModel()
        {
            this.CurrentProject = new JsProject();
        }

        public event EventHandler<PropertyChangedEventArgs> ProjectPropertyChanged;

        /// <summary>
        /// Gets a value indicating whether debug session is active
        /// </summary>
        public bool IsSessionActive
        {
            get
            {
                return this.Interpreter != null;
            }
        }

        /// <summary>
        /// Gets or sets current project
        /// </summary>
        public JsProject CurrentProject
        {
            get
            {
                return this.currentProject;
            }
            set
            {
                if (this.currentProject == value)
                {
                    return;
                }

                if (this.currentProject != null)
                {
                    this.currentProject.PropertyChanged -= this.OnProjectPropertyChanged;
                }

                this.currentProject = value;
                this.currentProject.PropertyChanged += this.OnProjectPropertyChanged;
            }
        }

        /// <summary>
        /// Gets current interpreter
        /// </summary>
        public IInterpreter Interpreter { get; private set; }

        /// <summary>
        /// Starts debug session
        /// </summary>
        public void StartSession()
        {
            if (this.Interpreter != null)
            {
                throw new Exception("Can not start to debug sessions simultaneously");
            }

            var serviceLocator = ServiceLocator.Default;
            this.Interpreter = serviceLocator.ResolveType<IInterpreter>();

            this.Interpreter.ExecuteScript(this.CurrentProject.MainScript);
        }

        private void OnProjectPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var handler = this.ProjectPropertyChanged;
            if (handler != null)
            {
                handler(sender, e);
            }
        }
    }
}
