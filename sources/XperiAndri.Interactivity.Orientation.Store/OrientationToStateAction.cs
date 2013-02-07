using XperiAndri.Expression.Interactivity;
using XperiAndri.Expression.Interactivity.Core;
using System;
using System.Diagnostics;
using System.Globalization;
using Windows.UI.Interactivity;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace XperiAndri.Interactivity
{
    
    //[DefaultTrigger(typeof(UIElement), typeof(OrientationTrigger), null)]
    /// <summary>
    /// An action that will transition a FrameworkElement to a specified VisualStates depending on
    /// devicу orientation.
    /// </summary>
    /// <remarks>
    /// If the TargetName property is set, this action will attempt to change the state of the targeted element. 
    /// If not, it walks the element tree in an attempt to locate an alternative target that defines states.
    /// ControlTemplate and UserControl are two common possibilities.
    /// </remarks>
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

        #region FullScreenLandscapeStateName

        public static readonly DependencyProperty FullScreenLandscapeStateNameProperty = DependencyProperty.Register("FullScreenLandscapeStateName", typeof(string), typeof(OrientationToStateAction), new PropertyMetadata(default(string)));

        //[CustomPropertyValueEditor(CustomPropertyValueEditor.StateName)]
        /// <summary>
        /// The name of the full screen landscape VisualState.
        /// </summary>
        public string FullScreenLandscapeStateName
        {
            get
            {
                return (string)GetValue(FullScreenLandscapeStateNameProperty);
            }
            set
            {
                SetValue(FullScreenLandscapeStateNameProperty, value);
            }
        }

        #endregion PortraitUpStateName

        #region FullScreenPortraitStateName

        public static readonly DependencyProperty FullScreenPortraitStateNameProperty = DependencyProperty.Register("FullScreenPortraitStateName", typeof(string), typeof(OrientationToStateAction), new PropertyMetadata(default(string)));

        //[CustomPropertyValueEditor(CustomPropertyValueEditor.StateName)]
        /// <summary>
        /// The name of the full screen portrait VisualState.
        /// </summary>
        public string FullScreenPortraitStateName
        {
            get
            {
                return (string)GetValue(FullScreenPortraitStateNameProperty);
            }
            set
            {
                SetValue(FullScreenPortraitStateNameProperty, value);
            }
        }

        #endregion PortraitDownStateName

        #region FilledStateName

        public static readonly DependencyProperty FilledStateNameProperty = DependencyProperty.Register("FilledStateName", typeof(string), typeof(OrientationToStateAction), new PropertyMetadata(default(string)));

        //[CustomPropertyValueEditor(CustomPropertyValueEditor.StateName)]
        /// <summary>
        /// The name of the filled VisualState.
        /// </summary>
        public string FilledStateName
        {
            get
            {
                return (string)GetValue(FilledStateNameProperty);
            }
            set
            {
                SetValue(FilledStateNameProperty, value);
            }
        }

        #endregion LandscapeLeftStateName

        #region SnappedStateName

        public static readonly DependencyProperty SnappedStateNameProperty = DependencyProperty.Register("SnappedStateName", typeof(string), typeof(OrientationToStateAction), new PropertyMetadata(default(string)));

        //[CustomPropertyValueEditor(CustomPropertyValueEditor.StateName)]
        /// <summary>
        /// The name of the snapped VisualState.
        /// </summary>
        public string SnappedStateName
        {
            get
            {
                return (string)GetValue(SnappedStateNameProperty);
            }
            set
            {
                SetValue(SnappedStateNameProperty, value);
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
            if (this.StateTarget != null && Enum.IsDefined(typeof(ApplicationViewState), parameter))
            {
                string stateName;

                var viewState = (ApplicationViewState)parameter;
                switch (viewState)
                {
                    case ApplicationViewState.FullScreenLandscape:
                        stateName = FullScreenLandscapeStateName;
                        break;
                    case ApplicationViewState.FullScreenPortrait:
                        stateName = FullScreenPortraitStateName;
                        break;
                    case ApplicationViewState.Filled:
                        stateName = FilledStateName;
                        break;
                    case ApplicationViewState.Snapped:
                        //if (Window.Current.Bounds.Left > 0)
                        //    Debug.WriteLine("Right side");
                        //else
                        //    Debug.WriteLine("Left side");
                        stateName = SnappedStateName;
                        break;
                    default:
                        stateName = string.Empty;
                        break;
                }

                if (string.IsNullOrEmpty(stateName))
                    stateName = viewState.ToString();

                VisualStateUtilities.GoToState(this.StateTarget, stateName, this.UseTransitions);

                //Control stateTarget = this.StateTarget as Control;
                //if (stateTarget != null)
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