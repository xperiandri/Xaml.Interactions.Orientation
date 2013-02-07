using System;
using System.ComponentModel;
using System.Globalization;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace XperiAndri.Interactivity
{
    /// <summary>
    /// Represents a trigger that performs actions when orientaion of the device have changed. 
    /// </summary>
    public class OrientationTrigger : TriggerBase<FrameworkElement>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += AssociatedObjectLoaded;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            (Application.Current.RootVisual as PhoneApplicationFrame).OrientationChanged -= OrientationChanged;
        }

        private void AssociatedObjectLoaded(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                var frame = Application.Current.RootVisual as PhoneApplicationFrame;
                frame.OrientationChanged += OrientationChanged;
                InvokeActions(frame.Orientation);
            });
            AssociatedObject.Loaded -= AssociatedObjectLoaded;
        }

        private void OrientationChanged(object sender, OrientationChangedEventArgs orientationChangedEventArgs)
        {
            InvokeActions(orientationChangedEventArgs.Orientation);
        }
    }
}