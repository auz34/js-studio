namespace MyStudio.Behaviors
{
    using System.Windows;

    using ICSharpCode.AvalonEdit;

    using Xceed.Wpf.AvalonDock.Layout;

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

        public static void SetBindableText(DependencyObject obj, string value)
        {
            obj.SetValue(BindableTextProperty, value);
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

            textEditor.Text = e.NewValue as string;
        }
    }
}
