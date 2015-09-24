// -----------------------------------------------------------------------
// <copyright file="CommandsService.cs">
// Copyright (c) 2015 Andrew Zavgorodniy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyStudio.Services
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;

    using Catel;
    using Catel.Memento;
    using Catel.MVVM;
    using Catel.Services;

    using MyStudio.Models;

    using Orchestra.Models;
    using Orchestra.Services;

    public class CommandsService : ICommandsService
    {
        private readonly IMementoService mementoService;

        private readonly IMessageService messageService;

        private readonly ICommandManager commandManager;

        private readonly IOpenFileService openFileService;

        private readonly ISaveFileService saveFileService;

        private readonly IRecentlyUsedItemsService recentlyUsedItemsService;

        private readonly StudioStateModel model;

        public CommandsService(StudioStateModel model,
            ICommandManager commandManager,
            IMementoService mementoService,
            IMessageService messageService,
            IOpenFileService openFileService,
            IRecentlyUsedItemsService recentlyUsedItemsService,
            ISaveFileService saveFileService)
        {
            Argument.IsNotNull(() => model);
            Argument.IsNotNull(() => commandManager);
            Argument.IsNotNull(() => mementoService);
            Argument.IsNotNull(() => messageService);
            Argument.IsNotNull(() => openFileService);
            Argument.IsNotNull(() => recentlyUsedItemsService);
            Argument.IsNotNull(() => saveFileService);

            this.model = model;
            this.commandManager = commandManager;
            this.mementoService = mementoService;
            this.messageService = messageService;
            this.openFileService = openFileService;
            this.recentlyUsedItemsService = recentlyUsedItemsService;
            this.saveFileService = saveFileService;

            this.UndoCommand = new Command(this.Undo, this.CanUndo);
            this.RedoCommand = new Command(this.Redo, this.CanRedo);
            this.OpenProjectCommand = new Command(this.OpenProject, () => true);

            this.SaveProjectAsCommand = new Command(delegate { this.SaveAsProject(); }, () => true);
            this.SaveProjectCommand = new Command(delegate { this.SaveProject(); }, this.CanSave);

            this.OpenRecentlyUsedItemCommand = new Command<string>(this.OnOpenRecentlyUsedItemExecute);

            this.StartCommand = new Command(this.Start, this.CanStart);

            this.ExitCommand = new Command(this.Exit);

            commandManager.RegisterCommand("Script.Open", this.OpenProjectCommand);
            commandManager.RegisterCommand("Script.Save", this.SaveProjectCommand);
            commandManager.RegisterCommand("Script.SaveAs", this.SaveProjectAsCommand);
            commandManager.RegisterCommand("App.Exit", this.ExitCommand);

            this.model.ProjectPropertyChanged += this.OnProjectPropertyChanged;
        }

        /// <summary>
        /// Gets Undo command
        /// </summary>
        public Command UndoCommand { get; private set; }

        /// <summary>
        /// Gets Redo command
        /// </summary>
        public Command RedoCommand { get; private set; }

        /// <summary>
        /// Gets OpenProject command
        /// </summary>
        public Command OpenProjectCommand { get; private set; }

        /// <summary>
        /// Gets SaveProject command
        /// </summary>
        public Command SaveProjectCommand { get; private set; }

        /// <summary>
        /// Gets SaveProjectAs command
        /// </summary>
        public Command SaveProjectAsCommand { get; private set; }

        /// <summary>
        /// Gets command of opening recently used item.
        /// </summary>
        public Command<string> OpenRecentlyUsedItemCommand { get; private set; }

        /// <summary>
        /// Gets Start command
        /// </summary>
        public Command StartCommand { get; private set; }

        /// <summary>
        /// Gets the Exit command.
        /// </summary>
        public Command ExitCommand { get; private set; }
        
        private void Undo()
        {
            this.mementoService.Undo();

            this.UndoCommand.RaiseCanExecuteChanged();
            this.RedoCommand.RaiseCanExecuteChanged();
        }

        private bool CanUndo()
        {
            return this.mementoService.CanUndo;
        }

        /// <summary>
        /// The redo.
        /// </summary>
        private void Redo()
        {
            this.mementoService.Redo();

            this.UndoCommand.RaiseCanExecuteChanged();
            this.RedoCommand.RaiseCanExecuteChanged();
        }

        private bool CanRedo()
        {
            return this.mementoService.CanUndo;
        }

        /// <summary>
        /// Method to invoke when the OpenRecentlyUsedItem command is executed.
        /// </summary>
        private async void OnOpenRecentlyUsedItemExecute(string parameter)
        {
            if (this.CanSave())
            {
                switch (await this.messageService.ShowAsync("Do you want to save changes?", "Save changes?", MessageButton.YesNoCancel))
                {
                    case MessageResult.Cancel:
                        return;

                    case MessageResult.Yes:
                        if (!this.SaveProject())
                        {
                            return;
                        }
                        break;

                    case MessageResult.No:
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            var failed = false;

            try
            {
                this.model.CurrentProject = new JsProject(parameter);
                this.recentlyUsedItemsService.AddItem(new RecentlyUsedItem(parameter, DateTime.Now));
            }
            catch (Exception)
            {
                failed = true;
            }

            if (failed)
            {
                if (await this.messageService.ShowAsync("The directory does not exist or has been removed. Would you like to remove it from the recently used list?", "Remove from recently used items?", MessageButton.YesNo) == MessageResult.Yes)
                {
                    var recentlyUsedItem = this.recentlyUsedItemsService.Items.FirstOrDefault(x => string.Equals(x.Name, parameter));
                    if (recentlyUsedItem != null)
                    {
                        this.recentlyUsedItemsService.RemoveItem(recentlyUsedItem);
                    }
                }
            }
        }

        private async void OpenProject()
        {
            if (this.CanSave())
            {
                switch (await this.messageService.ShowAsync("Do you want to save changes?", "Save changes?", MessageButton.YesNoCancel))
                {
                    case MessageResult.Cancel:
                        return;

                    case MessageResult.Yes:
                        if (!this.SaveProject())
                        {
                            return;
                        }
                        break;

                    case MessageResult.No:
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            
            // TODO add more files if we decide to work with "projects"
            this.openFileService.Filter = "Single script files|*.js";
            if (this.openFileService.DetermineFile())
            {
                this.model.CurrentProject = new JsProject(this.openFileService.FileName);
                this.recentlyUsedItemsService.AddItem(new RecentlyUsedItem(this.openFileService.FileName, DateTime.Now));
            }
        }

        private bool SaveAsProject()
        {
            this.saveFileService.Filter = "Single script files|*.js";
            if (this.saveFileService.DetermineFile())
            {
                this.model.CurrentProject.SaveProject(this.saveFileService.FileName);
                return true;
            }

            return false;
        }

        private bool SaveProject()
        {
            if (this.model.CurrentProject.IsNew)
            {
                return this.SaveAsProject();
            }

            this.model.CurrentProject.SaveProject(null);
            return true;
        }

        private bool CanSave()
        {
            return this.mementoService.CanUndo || this.model.CurrentProject.HasChanges;
        }

        /// <summary>
        /// Method to invoke when the Exit command is executed. 
        /// </summary>
        private async void Exit()
        {
            if (this.CanSave())
            {
                switch (await this.messageService.ShowAsync("Do you want to save changes?", "Save changes?", MessageButton.YesNoCancel))
                {
                    case MessageResult.Cancel:
                        return;

                    case MessageResult.Yes:
                        if (!this.SaveProject())
                        {
                            return;
                        }
                        break;

                    case MessageResult.No:
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            Application.Current.Shutdown();
        }

        /// <summary>
        /// Starts debugging session. 
        /// </summary>
        private void Start()
        {
            this.model.StartSession();
        }

        private bool CanStart()
        {
            return !this.model.IsSessionActive;
        }

        private void OnProjectPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.SaveProjectCommand.RaiseCanExecuteChanged();
            this.UndoCommand.RaiseCanExecuteChanged();
            this.RedoCommand.RaiseCanExecuteChanged();
        }
    }
}
