// -----------------------------------------------------------------------
// <copyright file="RibbonViewModel.cs">
// Copyright (c) 2015 Andrew Zavgorodniy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyStudio.ViewModels
{
    using System;
    using System.Collections.Generic;

    using Catel;
    using Catel.MVVM;

    using MyStudio.Models;
    using MyStudio.Services;

    using Orchestra.Models;
    using Orchestra.Services;

    public class RibbonViewModel : ViewModelBase
    {
        private StudioStateModel model;

        private ICommandsService commandsService;

        private IRecentlyUsedItemsService recentlyUsedItemsService;

        public RibbonViewModel(StudioStateModel model,
            ICommandsService commandsService,
            IRecentlyUsedItemsService recentlyUsedItemsService)
        {
            Argument.IsNotNull(() => model);
            Argument.IsNotNull(() => commandsService);
            Argument.IsNotNull(() => recentlyUsedItemsService);

            this.model = model;
            this.commandsService = commandsService;
            this.recentlyUsedItemsService = recentlyUsedItemsService;

            this.RecentlyUsedItems = new List<RecentlyUsedItem>(this.recentlyUsedItemsService.Items);
            this.PinnedItems = new List<RecentlyUsedItem>(this.recentlyUsedItemsService.PinnedItems);

            this.recentlyUsedItemsService.Updated += this.OnRecentlyUsedItemsServiceUpdated;
        }


        public Command StartCommand
        {
            get
            {
                return this.commandsService != null ? this.commandsService.StartCommand : null;
            }
        }

        public Command UndoCommand
        {
            get
            {
                return this.commandsService != null ? this.commandsService.UndoCommand : null;
            }
        }

        public Command RedoCommand
        {
            get
            {
                return this.commandsService != null ? this.commandsService.RedoCommand : null;
            }
        }

        public List<RecentlyUsedItem> RecentlyUsedItems { get; private set; }

        public List<RecentlyUsedItem> PinnedItems { get; private set; }

        private void OnRecentlyUsedItemsServiceUpdated(object sender, EventArgs e)
        {
            this.RecentlyUsedItems = new List<RecentlyUsedItem>(this.recentlyUsedItemsService.Items);
            this.PinnedItems = new List<RecentlyUsedItem>(this.recentlyUsedItemsService.PinnedItems);
        }
    }
}
