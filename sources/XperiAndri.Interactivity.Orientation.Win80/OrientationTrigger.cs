using System;
using System.Reflection;
using Windows.UI.Core;
using Windows.UI.Interactivity;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace XperiAndri.Interactivity
{
    /// <summary>
    /// Represents a trigger that performs actions when orientation of the device changes. 
    /// </summary>
    public class OrientationTrigger : TriggerBase<FrameworkElement>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            Window.Current.SizeChanged += this.WindowSizeChanged;
            InvokeActions(ApplicationView.Value);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            Window.Current.SizeChanged -= this.WindowSizeChanged;
        }

        private void WindowSizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            InvokeActions(ApplicationView.Value);
        }
    }
}