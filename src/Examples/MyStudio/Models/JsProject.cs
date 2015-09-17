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
            this.HasChanges = false;
            this.MainScript = "// Start type here";
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

        public string MainScriptFilePath { get; private set; }

        public string MainScript { get; set; }

        public bool HasChanges { get; private set; }

    }
}
