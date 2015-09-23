// -----------------------------------------------------------------------
// <copyright file="BindableEditorText.cs">
// Copyright (c) 2015 Andrew Zavgorodniy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyStudio.Behaviors
{
    using System;
    using System.Reactive.Linq;
    using System.Windows;

    using ICSharpCode.AvalonEdit;

    public static class BindableEditorText
    {
        /// <summary>
        /// The is persistent layout enabled property.
        /// </summary>
        public static readonly DependencyProperty BindableTextProperty =
            DependencyProperty.RegisterAttached(
                "BindableText",
                typeof(string),
                typeof(BindableEditorText),
                new PropertyMetadata(string.Empty, OnBindableTextPropertyChanged));

        private static readonly DependencyProperty TextChangedSubscriptionProperty =
            DependencyProperty.RegisterAttached(
                "TextChangedSubscription",
                typeof(IDisposable),
                typeof(BindableEditorText),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets current bind text.
        /// </summary>
        /// <param name="obj">
        /// The object.
        /// </param>
        /// <returns>
        /// The text.
        /// </returns>
        public static string GetBindableText(DependencyObject obj)
        {
            return (string)obj.GetValue(BindableTextProperty);
        }

        /// <summary>
        /// Sets bindable text.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetBindableText(DependencyObject obj, string value)
        {
            obj.SetValue(BindableTextProperty, value);
        }


        private static IDisposable GetTextChangedSubscription(DependencyObject obj)
        {
            return (IDisposable)obj.GetValue(TextChangedSubscriptionProperty);
        }

        private static void SetTextChangedSubscription(DependencyObject obj, IDisposable value)
        {
            obj.SetValue(TextChangedSubscriptionProperty, value);
        }

        /// <summary>
        /// The on is persistent layout enabled property changed.
        /// </summary>
        /// <param name="obj">
        /// The object.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private static void OnBindableTextPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var textEditor = obj as TextEditor;
            if (textEditor == null)
            {
                return;
            }

            var newValue = e.NewValue as string;
            if (textEditor.Text == newValue)
            {
                return;
            }


            textEditor.Text = newValue;

            if (GetTextChangedSubscription(textEditor) != null)
            {
                return;
            }

            // if we change text for the first time it means 
            var subscription = Observable
                .FromEventPattern<EventArgs>(textEditor, "TextChanged")
                .Subscribe(
                    ev => {
                            var currentText = GetBindableText(textEditor);
                            if (textEditor.Text != currentText)
                            {
                                SetBindableText(textEditor, textEditor.Text);
                            }
                        });

            SetTextChangedSubscription(textEditor, subscription);
        }
    }
}
