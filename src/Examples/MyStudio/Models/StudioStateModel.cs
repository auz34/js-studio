namespace MyStudio.Models
{
    using System;

    using Catel.Data;
    using Catel.IoC;

    using MyStudio.Services;

    public class StudioStateModel : ModelBase
    {
        public StudioStateModel()
        {
            this.CurrentProject = new JsProject();
        }

        public bool IsSessionActive
        {
            get
            {
                return this.Interpreter != null;
            }
        }

        public JsProject CurrentProject { get; set; }

        public IInterpreter Interpreter { get; private set; }

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
    }
}
