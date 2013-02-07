using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using Microsoft.Expression.Interactivity.Core;
using Microsoft.Expression.Prototyping.WindowsPhone.Mockups;
using Microsoft.Expression.Interactivity;
using System.Globalization;

namespace XperiAndri.Interactivity
{
    /// <summary>
    /// An action that will transition a FrameworkElement to a specified VisualStates depending on
    /// devicу orientation.
    /// </summary>
    /// <remarks>
    /// If the TargetName property is set, this action will attempt to change the state of the targeted element. 
    /// If not, it walks the element tree in an attempt to locate an alternative target that defines states.
    /// ControlTemplate and UserControl are two common possibilities.
    /// </remarks>
    [DefaultTrigger(typeof(UIElement), typeof(OrientationTrigger), null)]
    public class OrientationToStateAction : TargetedTriggerAction<FrameworkElement>
    {
        #region DependencyProperties

        public static readonly DependencyProperty UseTransitionsProperty = DependencyProperty.Register("UseTransitions", typeof(bool), typeof(OrientationToStateAction), new PropertyMetadata(true));

        /// <summary>
        /// Determines whether or not to use a VisualTransition to transition between states.
        /// </summary>
        public bool UseTransitions
        {
            get
            {
                return (bool)GetValue(UseTransitionsProperty);
            }
            set
            {
                SetValue(UseTransitionsProperty, value);
            }
        }

        public static readonly DependencyProperty PortraitStateNameProperty = DependencyProperty.Register("PortraitStateName", typeof(string), typeof(OrientationToStateAction), new PropertyMetadata(null));

        /// <summary>
        /// The name of the portrait VisualState.
        /// </summary>
        [CustomPropertyValueEditor(CustomPropertyValueEditor.StateName)]
        public string PortraitStateName
        {
            get
            {
                return (string)GetValue(PortraitStateNameProperty);
            }
            set
            {
                SetValue(PortraitStateNameProperty, value);
            }
        }

        public static readonly DependencyProperty LandscapeStateNameProperty = DependencyProperty.Register("LandscapeStateName", typeof(string), typeof(OrientationToStateAction), new PropertyMetadata(null));

        /// <summary>
        /// The name of the landscape VisualState.
        /// </summary>
        [CustomPropertyValueEditor(CustomPropertyValueEditor.StateName)]
        public string LandscapeStateName
        {
            get
            {
                return (string)GetValue(LandscapeStateNameProperty);
            }
            set
            {
                SetValue(LandscapeStateNameProperty, value);
            }
        }
        
        #endregion DependencyProperties

        #region Properties

        private bool IsTargetObjectSet
        {
            get
            {
                return (base.ReadLocalValue(TargetedTriggerAction.TargetObjectProperty) != DependencyProperty.UnsetValue);
            }
        }

        private FrameworkElement StateTarget { get; set; }

        #endregion Properties

        protected override void Invoke(object parameter)
        {
            if (this.StateTarget != null && Enum.IsDefined(typeof(PageOrientation), parameter))
            {
                string stateName;

                switch ((PageOrientation)parameter)
                {
                    case PageOrientation.Portrait:
                        stateName = PortraitStateName;
                        break;
                    case PageOrientation.Landscape:
                        stateName = LandscapeStateName;
                        break;
                    default:
                        stateName = string.Empty;
                        break;
                }

                Control stateTarget = this.StateTarget as Control;
                if (stateTarget != null)
                {
                    stateTarget.ApplyTemplate();
                    VisualStateManager.GoToState(stateTarget, stateName, this.UseTransitions);
                }
                else
                {
                    ExtendedVisualStateManager.GoToElementState(this.StateTarget, stateName, this.UseTransitions);
                }
            }
        }

        //protected override void OnTargetChanged(FrameworkElement oldTarget, FrameworkElement newTarget)
        //{
        //    base.OnTargetChanged(oldTarget, newTarget);
        //    FrameworkElement target = null;
        //    if (!string.IsNullOrEmpty(base.TargetName) || this.IsTargetObjectSet)
        //    {
        //        target = base.Target;
        //    }
        //    this.StateTarget = target;
        //}
        protected override void OnTargetChanged(FrameworkElement oldTarget, FrameworkElement newTarget)
        {
            base.OnTargetChanged(oldTarget, newTarget);
            FrameworkElement resolvedControl = null;
            if (string.IsNullOrEmpty(base.TargetName) && !this.IsTargetObjectSet)
            {
                if (!VisualStateUtilities.TryFindNearestStatefulControl(base.AssociatedObject as FrameworkElement, out resolvedControl) && (resolvedControl != null))
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Target {0} does not define any VisualStateGroups.", new object[] { resolvedControl.Name }));
                }
            }
            else
            {
                resolvedControl = base.Target;
            }
            this.StateTarget = resolvedControl;
        }
    }
}