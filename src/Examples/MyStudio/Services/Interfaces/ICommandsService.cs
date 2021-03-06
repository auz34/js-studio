﻿// -----------------------------------------------------------------------
// <copyright file="ICommandsService.cs">
// Copyright (c) 2015 Andrew Zavgorodniy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyStudio.Services
{
    using Catel.MVVM;

    public interface ICommandsService
    {
        /// <summary>
        /// Gets command of canceling last user activity.
        /// </summary>
        Command UndoCommand { get; }

        /// <summary>
        /// Gets command of redo last canceled user activity.
        /// </summary>
        Command RedoCommand { get; }

        /// <summary>
        /// Gets command of opening new project.
        /// </summary>
        Command OpenProjectCommand { get; }

        /// <summary>
        /// Gets command of opening new project.
        /// </summary>
        Command SaveProjectCommand { get; }

        /// <summary>
        /// Gets command of opening recently used item.
        /// </summary>
        Command<string> OpenRecentlyUsedItemCommand { get; }

        /// <summary>
        /// Gets command of starting new execution.
        /// </summary>
        Command StartCommand { get; }

        /// <summary>
        /// Gets the Pin command.
        /// </summary>
        Command<string> PinItemCommand { get; }

        /// <summary>
        /// Gets the Unpin command.
        /// </summary>
        Command<string> UnpinItemCommand { get; }

        /// <summary>
        /// Gets the OpenInExplorer command.
        /// </summary>
        Command<string> OpenInExplorerCommand { get; }
    }
}