using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using Microsoft.Phone.Controls;
using Microsoft.Expression.Interactivity.Core;
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

        #region UseTransitions

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

        #endregion UseTransitions

        #region PortraitUpStateName

        public static readonly DependencyProperty PortraitUpStateNameProperty = DependencyProperty.Register("PortraitUpStateName", typeof(string), typeof(OrientationToStateAction), new PropertyMetadata(null));

        /// <summary>
        /// The name of the portrait up VisualState.
        /// </summary>
        [CustomPropertyValueEditor(CustomPropertyValueEditor.StateName)]
        public string PortraitUpStateName
        {
            get
            {
                return (string)GetValue(PortraitUpStateNameProperty);
            }
            set
            {
                SetValue(PortraitUpStateNameProperty, value);
            }
        }

        #endregion PortraitUpStateName

        #region PortraitDownStateName

        public static readonly DependencyProperty PortraitDownStateNameProperty = DependencyProperty.Register("PortraitDownStateName", typeof(string), typeof(OrientationToStateAction), new PropertyMetadata(null));

        /// <summary>
        /// The name of the portrait down VisualState.
        /// </summary>
        [CustomPropertyValueEditor(CustomPropertyValueEditor.StateName)]
        public string PortraitDownStateName
        {
            get
            {
                return (string)GetValue(PortraitDownStateNameProperty);
            }
            set
            {
                SetValue(PortraitDownStateNameProperty, value);
            }
        }

        #endregion PortraitDownStateName

        #region LandscapeLeftStateName

        public static readonly DependencyProperty LandscapeLeftStateNameProperty = DependencyProperty.Register("LandscapeLeftStateName", typeof(string), typeof(OrientationToStateAction), new PropertyMetadata(null));

        /// <summary>
        /// The name of the landscape left VisualState.
        /// </summary>
        [CustomPropertyValueEditor(CustomPropertyValueEditor.StateName)]
        public string LandscapeLeftStateName
        {
            get
            {
                return (string)GetValue(LandscapeLeftStateNameProperty);
            }
            set
            {
                SetValue(LandscapeLeftStateNameProperty, value);
            }
        }

        #endregion LandscapeLeftStateName

        #region LandscapeRightStateName

        public static readonly DependencyProperty LandscapeRightStateNameProperty = DependencyProperty.Register("LandscapeRightStateName", typeof(string), typeof(OrientationToStateAction), new PropertyMetadata(null));

        /// <summary>
        /// The name of the landscape right VisualState.
        /// </summary>
        [CustomPropertyValueEditor(CustomPropertyValueEditor.StateName)]
        public string LandscapeRightStateName
        {
            get
            {
                return (string)GetValue(LandscapeRightStateNameProperty);
            }
            set
            {
                SetValue(LandscapeRightStateNameProperty, value);
            }
        }

        #endregion LandscapeRightStateName

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
                    case PageOrientation.PortraitUp:
                        stateName = PortraitUpStateName;
                        break;
                    case PageOrientation.PortraitDown:
                        stateName = PortraitDownStateName;
                        break;
                    case PageOrientation.LandscapeLeft:
                        stateName = LandscapeLeftStateName;
                        break;
                    case PageOrientation.LandscapeRight:
                        stateName = LandscapeRightStateName;
                        break;
                    default:
                        stateName = string.Empty;
                        break;
                }

                VisualStateUtilities.GoToState(this.StateTarget, stateName, this.UseTransitions);

                //Control stateTarget = this.StateTarget as Control;
                //if (stateTarget != null && string.IsNullOrEmpty(stateName))
                //{
                //    stateTarget.ApplyTemplate();
                //    VisualStateManager.GoToState(stateTarget, stateName, this.UseTransitions);
                //}
                //else
                //{
                //    ExtendedVisualStateManager.GoToElementState(this.StateTarget, stateName, this.UseTransitions);
                //}
            }
        }

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