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
    /// An action that will transition a FrameworkElement to a specified VisualStates depending on current
    /// application view state.
    /// </summary>
    /// <remarks>
    /// If the TargetName property is set, this action will attempt to change the state of the targeted element. 
    /// If not, it walks the element tree in an attempt to locate an alternative target that defines states.
    /// ControlTemplate and UserControl are two common possibilities.
    /// </remarks>
    public class OrientationToStateAction : TargetedTriggerAction<FrameworkElement>
    {
        #region DependencyProperties

        //#region UseTransitions

        ///// <summary>
        ///// Identifies the
        ///// <see cref="P:XperiAndri.Interactivity.OrientationToStateAction.UseTransitions" />
        ///// dependency property.
        ///// </summary>
        ///// <value>
        ///// The identifier for the
        ///// <see cref="P:XperiAndri.Interactivity.OrientationToStateAction.UseTransitions" />
        ///// dependency property.
        ///// </value>
        //public static readonly DependencyProperty UseTransitionsProperty = DependencyProperty.Register("UseTransitions", typeof(bool), typeof(OrientationToStateAction), new PropertyMetadata(true));

        ///// <summary>
        ///// Determines whether or not to use a <see cref="T:System.Windows.VisualTransition"/> 
        ///// to transition between states.
        ///// </summary>
        ///// <value>
        ///// True if Use a <see cref="T:System.Windows.VisualTransition"/> to transition 
        ///// between states; otherwise false. Default value is true. 
        ///// </value>
        //public bool UseTransitions
        //{
        //    get
        //    {
        //        return (bool)GetValue(UseTransitionsProperty);
        //    }
        //    set
        //    {
        //        SetValue(UseTransitionsProperty, value);
        //    }
        //}

        //#endregion UseTransitions

        #region FullScreenLandscapeStateName

        /// <summary>
        /// Identifies the
        /// <see cref="P:XperiAndri.Interactivity.OrientationToStateAction.FullScreenLandscapeStateName" />
        /// dependency property.
        /// </summary>
        /// <value>
        /// The identifier for the
        /// <see cref="P:XperiAndri.Interactivity.OrientationToStateAction.FullScreenLandscapeStateName" />
        /// dependency property.
        /// </value>
        public static readonly DependencyProperty FullScreenLandscapeStateNameProperty = DependencyProperty.Register("FullScreenLandscapeStateName", typeof(string), typeof(OrientationToStateAction), new PropertyMetadata(default(string)));

        //[CustomPropertyValueEditor(CustomPropertyValueEditor.StateName)]
        /// <summary>
        /// Gets or sets the name of the full screen landscape VisualState.
        /// </summary>
        /// <value>
        /// The name of the full screen landscape VisualState.
        /// </value>
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

        /// <summary>
        /// Identifies the
        /// <see cref="P:XperiAndri.Interactivity.OrientationToStateAction.FullScreenPortraitStateName" />
        /// dependency property.
        /// </summary>
        /// <value>
        /// The identifier for the
        /// <see cref="P:XperiAndri.Interactivity.OrientationToStateAction.FullScreenPortraitStateName" />
        /// dependency property.
        /// </value>
        public static readonly DependencyProperty FullScreenPortraitStateNameProperty = DependencyProperty.Register("FullScreenPortraitStateName", typeof(string), typeof(OrientationToStateAction), new PropertyMetadata(default(string)));

        //[CustomPropertyValueEditor(CustomPropertyValueEditor.StateName)]
        /// <summary>
        /// Gets or sets the name of the full screen portrait VisualState.
        /// </summary>
        /// <value>
        /// The name of the full screen portrait VisualState.
        /// </value>
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

        /// <summary>
        /// Identifies the
        /// <see cref="P:XperiAndri.Interactivity.OrientationToStateAction.FilledStateName" />
        /// dependency property.
        /// </summary>
        /// <value>
        /// The identifier for the
        /// <see cref="P:XperiAndri.Interactivity.OrientationToStateAction.FilledStateName" />
        /// dependency property.
        /// </value>
        public static readonly DependencyProperty FilledStateNameProperty = DependencyProperty.Register("FilledStateName", typeof(string), typeof(OrientationToStateAction), new PropertyMetadata(default(string)));

        //[CustomPropertyValueEditor(CustomPropertyValueEditor.StateName)]
        /// <summary>
        /// Gets or sets the name of the filled VisualState.
        /// </summary>
        /// <value>
        /// The name of the filled VisualState.
        /// </value>
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

        /// <summary>
        /// Identifies the
        /// <see cref="P:XperiAndri.Interactivity.OrientationToStateAction.SnappedStateName" />
        /// dependency property.
        /// </summary>
        /// <value>
        /// The identifier for the
        /// <see cref="P:XperiAndri.Interactivity.OrientationToStateAction.SnappedStateName" />
        /// dependency property.
        /// </value>
        public static readonly DependencyProperty SnappedStateNameProperty = DependencyProperty.Register("SnappedStateName", typeof(string), typeof(OrientationToStateAction), new PropertyMetadata(default(string)));

        //[CustomPropertyValueEditor(CustomPropertyValueEditor.StateName)]
        /// <summary>
        /// Gets or sets the name of the snapped VisualState.
        /// </summary>
        /// <value>
        /// The name of the snapped VisualState.
        /// </value>
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

        /// <summary>
        /// Invokes the action.
        /// </summary>
        /// <param name="parameter">
        /// New application view state of type 
        /// <see cref="T:Windows.UI.ViewManagement.ApplicationViewState"/>.
        /// </param>
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

                //VisualStateUtilities.GoToState(this.StateTarget, stateName, this.UseTransitions);
                VisualStateUtilities.GoToState(this.StateTarget, stateName, false);

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

        /// <summary>
        /// Called when the target property changes.
        /// </summary>
        /// <param name="oldTarget">The old target.</param>
        /// <param name="newTarget">The new target.</param>
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