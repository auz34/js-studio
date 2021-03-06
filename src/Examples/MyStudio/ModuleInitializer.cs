﻿// -----------------------------------------------------------------------
// <copyright file="ModuleInitializer.cs">
// Copyright (c) 2015 Andrew Zavgorodniy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Catel.IoC;
using Orchestra.Services;
using MyStudio.Services;

/// <summary>
/// Used by the ModuleInit. All code inside the Initialize method is ran as soon as the assembly is loaded.
/// </summary>
public static class ModuleInitializer
{
    /// <summary>
    /// Initializes the module.
    /// </summary>
    public static void Initialize()
    {
        var serviceLocator = ServiceLocator.Default;

        serviceLocator.RegisterType<IRibbonService, RibbonService>();
        serviceLocator.RegisterType<IApplicationInitializationService, ApplicationInitializationService>();

        serviceLocator.RegisterType<ICommandsService, CommandsService>();

        serviceLocator.RegisterType<IInterpreter, DummyInterpreter>();
    }
}