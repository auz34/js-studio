namespace MyStudio.Models
{
    using Catel.Data;

    public class JsProject : ModelBase
    {
        public JsProject()
        {
            this.HasChanges = false;
        }

        public string MainScriptFilePath { get; set; }

        public bool HasChanges { get; private set; }

    }
}
