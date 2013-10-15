using Microsoft.Xaml.Interactivity;
using System;
using System.Reflection;
using Windows.Graphics.Display;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;

namespace XperiAndri.Interactivity
{
    /// <summary>
    /// Represents a trigger behavior that performs actions when orientation of the device changes.
    /// </summary>
    [ContentProperty(Name = "Actions")]
    public class OrientationTriggerBehavior : DependencyObject, IBehavior
    {
        #region DependencyProperties

        #region Actions

        /// <summary>
        /// Identifies the
        /// <see cref="P:Microsoft.Xaml.Interactions.Core.EventTriggerBehavior.Actions"/>
        /// dependency property.
        /// </summary>
        public static readonly DependencyProperty ActionsProperty = DependencyProperty.Register("Actions", typeof(ActionCollection), typeof(OrientationTriggerBehavior), new PropertyMetadata(null));

        /// <summary>
        /// Gets the collection of actions associated with the behavior. This is a dependency
        /// property.
        /// </summary>
        /// <value>
        /// The collection of actions associated with the behavior.
        /// </value>
        public ActionCollection Actions
        {
            get
            {
                ActionCollection actionList = (ActionCollection)this.GetValue(ActionsProperty);
                if (actionList == null)
                {
                    actionList = new ActionCollection();
                    this.SetValue(ActionsProperty, actionList);
                }

                return actionList;
            }
        }

        #endregion Actions

        #endregion DependencyProperties

        #region Properties

        /// <summary>
        /// Gets the <see cref="T:Windows.UI.Xaml.DependencyObject"/> to which
        /// the <see cref="T:Microsoft.Xaml.Interactivity.IBehavior"/> is attached.
        /// </summary>
        public DependencyObject AssociatedObject { get; private set; }

        #endregion Properties

        /// <summary>
        /// Attaches to the specified object.
        /// </summary>
        /// <param name="associatedObject">
        /// <see cref="T:Windows.UI.Xaml.DependencyObject"/> to which
        /// <see cref="T:Microsoft.Xaml.Interactivity.IBehavior"/> will be attached.
        /// </param>
        public async void Attach(DependencyObject associatedObject)
        {
            this.AssociatedObject = associatedObject;
            DisplayInformation.GetForCurrentView().OrientationChanged += OnOrientationChanged;
            //await Window.Current.Content.Dispatcher.RunAsync(CoreDispatcherPriority.Idle, () => OnExecute());
            await Window.Current.Content.Dispatcher.RunIdleAsync(e => OnExecute());
            //OnExecute();
        }

        private void OnOrientationChanged(DisplayInformation sender, object result)
        {
            OnExecute();
        }

        private void OnExecute()
        {
            Interaction.ExecuteActions(this.AssociatedObject, this.Actions, DisplayInformation.GetForCurrentView().CurrentOrientation);
        }

        /// <summary>
        /// Detaches this instance from its associated object.
        /// </summary>
        public void Detach()
        {
            DisplayInformation.GetForCurrentView().OrientationChanged -= OnOrientationChanged;
            this.AssociatedObject = null;
        }
    }
}