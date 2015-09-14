namespace MyStudio.Models
{
    using Catel.Data;

    public class StudioStateModel : ModelBase
    {
        public StudioStateModel()
        {
            this.CurrentProject = new JsProject();
        }

        public JsProject CurrentProject { get; set; }
    }
}
