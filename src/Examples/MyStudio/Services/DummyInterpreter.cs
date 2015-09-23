// -----------------------------------------------------------------------
// <copyright file="DummyInterpreter.cs">
// Copyright (c) 2015 Andrew Zavgorodniy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

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
