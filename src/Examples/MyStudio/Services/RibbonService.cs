// -----------------------------------------------------------------------
// <copyright file="RibbonService.cs">
// Copyright (c) 2015 Andrew Zavgorodniy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyStudio.Services
{
    using System.Windows;

    using MyStudio.Views;

    using Orchestra.Services;

    public class RibbonService : IRibbonService
    {
        public FrameworkElement GetMainView()
        {
            return new MainView();
        }

        public FrameworkElement GetStatusBar()
        {
            return new StatusBarView();
        }

        public FrameworkElement GetRibbon()
        {
            return new RibbonView();
        }
    }
}
