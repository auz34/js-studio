// -----------------------------------------------------------------------
// <copyright file="JsProject.cs">
// Copyright (c) 2015 Andrew Zavgorodniy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyStudio.Models
{
    using System;

    using Catel.Data;
    using System.IO;

    using Catel;

    public class JsProject : ModelBase
    {
        public JsProject()
        {
            this.MainScript = "// Start type here";
            this.HasChanges = false;
        }

        public JsProject(string filePath)
        {
            Argument.IsNotNullOrEmpty(() => filePath);
            var ext = Path.GetExtension(filePath);
            switch (ext)
            {
                case ".js":
                    this.MainScriptFilePath = filePath;
                    break;
                default:
                    throw new NotSupportedException(string.Format("{0} extension isn't supported", ext));
            }

            if (!File.Exists(this.MainScriptFilePath))
            {
                throw new FileNotFoundException("Main script wasn't found", this.MainScriptFilePath);
            }

            this.MainScript = File.ReadAllText(this.MainScriptFilePath);
        }

        /// <summary>
        /// Gets file path for project main script
        /// </summary>
        public string MainScriptFilePath { get; private set; }

        /// <summary>
        /// Gets or sets current value of the main script
        /// </summary>
        public string MainScript { get; set; }

        /// <summary>
        /// Gets a value indicating whether project is new
        /// </summary>
        public bool IsNew
        {
            get
            {
                return string.IsNullOrEmpty(this.MainScriptFilePath);
            }
        }

        /// <summary>
        /// Gets a value indicating whether project has any changes
        /// </summary>
        public bool HasChanges { get; private set; }

        /// <summary>
        /// Saves project
        /// </summary>
        /// <param name="path">Project location in the file system</param>
        public void SaveProject(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                path = this.MainScriptFilePath;
            }

            File.WriteAllText(path, this.MainScript);
            this.MainScriptFilePath = path;
            this.HasChanges = false;
        }

        protected override void OnPropertyChanged(AdvancedPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.PropertyName == "MainScript")
            {
                this.HasChanges = true;
            }
        }
    }
}
