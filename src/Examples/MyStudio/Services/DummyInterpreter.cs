namespace MyStudio.Services
{
    using System.Diagnostics;

    public class DummyInterpreter: IInterpreter
    {
        public void ExecuteScript(string script)
        {
            Debug.Write("It works");
        }
    }
}
