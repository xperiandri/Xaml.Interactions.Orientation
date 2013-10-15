using Microsoft.Xaml.Interactivity;
using System;
using System.Globalization;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace XperiAndri.Interactivity
{
    //[DefaultTrigger(typeof(UIElement), typeof(OrientationTrigger), null)]
    /// <summary>
    /// An action that will transition a Control to a specified VisualStates depending on
    /// current device orientation.
    /// </summary>
    /// <remarks>
    /// If the TargetName property is set, this action will attempt to change the state of the targeted element.
    /// If not, it walks the element tree in an attempt to locate an alternative target that defines states.
    /// ControlTemplate and UserControl are two common possibilities.
    /// </remarks>
    public class OrientationToStateAction : DependencyObject, IAction
    {
        #region DependencyProperties

        #region TargetObject

        /// <summary>
        /// Identifies the
        /// <see cref="P:XperiAndri.Interactivity.OrientationToStateAction.TargetObject" />
        /// dependency property.
        /// </summary>
        /// <value>
        /// The identifier for the
        /// <see cref="P:XperiAndri.Interactivity.OrientationToStateAction.TargetObject" />
        /// dependency property.
        /// </value>
        public static readonly DependencyProperty TargetObjectProperty = DependencyProperty.Register("TargetObject", typeof(FrameworkElement), typeof(OrientationToStateAction), new PropertyMetadata(null/*, new PropertyChangedCallback(OnTargetObjectChanged)*/));
        //private static void OnTargetObjectChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        //{
        //    OrientationToStateAction orientationToStateAction = o as OrientationToStateAction;
        //    if (orientationToStateAction != null)
        //        orientationToStateAction.OnTargetObjectChanged((FrameworkElement)e.OldValue, (FrameworkElement)e.NewValue);
        //}

        //protected virtual void OnTargetObjectChanged(FrameworkElement oldTarget, FrameworkElement newTarget)
        //{
        //    FrameworkElement resolvedControl = null;
        //    if (!this.IsTargetObjectSet)
        //    {
        //        resolvedControl = VisualStateUtilities.FindNearestStatefulControl(base.AssociatedObject as FrameworkElement);
        //        if (resolvedControl != null)
        //        {
        //            throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Target {0} does not define any VisualStateGroups.", new object[] { resolvedControl.Name }));
        //        }
        //    }
        //    else
        //    {
        //        resolvedControl = newTarget;
        //    }
        //    this.StateTarget = resolvedControl;
        //}
        /// <summary>
        /// Gets or sets the target object for the action.
        /// </summary>
        /// <value>
        /// The target object of the action. Default value is null.
        /// </value>
        public FrameworkElement TargetObject
        {
            get
            {
                return (FrameworkElement)GetValue(TargetObjectProperty);
            }
            set
            {
                SetValue(TargetObjectProperty, value);
            }
        }

        #endregion TargetObject

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
        //public static readonly DependencyProperty UseTransitionsProperty = DependencyProperty.Register("UseTransitions", typeof(bool), typeof(OrientationToStateAction), new PropertyMetadata(false));

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

        #region LandscapeStateName

        /// <summary>
        /// Identifies the
        /// <see cref="P:XperiAndri.Interactivity.OrientationToStateAction.LandscapeStateName" />
        /// dependency property.
        /// </summary>
        /// <value>
        /// The identifier for the
        /// <see cref="P:XperiAndri.Interactivity.OrientationToStateAction.LandscapeStateName" />
        /// dependency property.
        /// </value>
        public static readonly DependencyProperty LandscapeStateNameProperty = DependencyProperty.Register("LandscapeStateName", typeof(string), typeof(OrientationToStateAction), new PropertyMetadata(default(string)));

        /// <summary>
        /// Gets or sets the name of the landscape VisualState.
        /// </summary>
        /// <value>
        /// The name of the landscape VisualState.
        /// </value>
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

        #endregion LandscapeStateName

        #region PortraitStateName

        /// <summary>
        /// Identifies the
        /// <see cref="P:XperiAndri.Interactivity.OrientationToStateAction.PortraitStateName" />
        /// dependency property.
        /// </summary>
        /// <value>
        /// The identifier for the
        /// <see cref="P:XperiAndri.Interactivity.OrientationToStateAction.PortraitStateName" />
        /// dependency property.
        /// </value>
        public static readonly DependencyProperty PortraitStateNameProperty = DependencyProperty.Register("PortraitStateName", typeof(string), typeof(OrientationToStateAction), new PropertyMetadata(default(string)));

        /// <summary>
        /// Gets or sets the name of the portrait VisualState.
        /// </summary>
        /// <value>
        /// The name of the portrait VisualState.
        /// </value>
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

        #endregion PortraitStateName

        #region LandscapeFlippedStateName

        /// <summary>
        /// Identifies the
        /// <see cref="P:XperiAndri.Interactivity.OrientationToStateAction.LandscapeFlippedStateName" />
        /// dependency property.
        /// </summary>
        /// <value>
        /// The identifier for the
        /// <see cref="P:XperiAndri.Interactivity.OrientationToStateAction.LandscapeFlippedStateName" />
        /// dependency property.
        /// </value>
        public static readonly DependencyProperty LandscapeFlippedStateNameProperty = DependencyProperty.Register("LandscapeFlippedStateName", typeof(string), typeof(OrientationToStateAction), new PropertyMetadata("Landscape"));

        /// <summary>
        /// Gets or sets the name of the flipped landscape VisualState.
        /// </summary>
        /// <value>
        /// The name of the flipped landscape VisualState.
        /// </value>
        [CustomPropertyValueEditor(CustomPropertyValueEditor.StateName)]
        public string LandscapeFlippedStateName
        {
            get
            {
                return (string)GetValue(LandscapeFlippedStateNameProperty);
            }
            set
            {
                SetValue(LandscapeFlippedStateNameProperty, value);
            }
        }

        #endregion LandscapeFlippedStateName

        #region PortraitFlippedStateName

        /// <summary>
        /// Identifies the
        /// <see cref="P:XperiAndri.Interactivity.OrientationToStateAction.PortraitFlippedStateName" />
        /// dependency property.
        /// </summary>
        /// <value>
        /// The identifier for the
        /// <see cref="P:XperiAndri.Interactivity.OrientationToStateAction.PortraitFlippedStateName" />
        /// dependency property.
        /// </value>
        public static readonly DependencyProperty PortraitFlippedStateNameProperty = DependencyProperty.Register("PortraitFlippedStateName", typeof(string), typeof(OrientationToStateAction), new PropertyMetadata("Portrait"));

        /// <summary>
        /// Gets or sets the name of the flipped portrait VisualState.
        /// </summary>
        /// <value>
        /// The name of the flipped portrait VisualState.
        /// </value>
        [CustomPropertyValueEditor(CustomPropertyValueEditor.StateName)]
        public string PortraitFlippedStateName
        {
            get
            {
                return (string)GetValue(PortraitFlippedStateNameProperty);
            }
            set
            {
                SetValue(PortraitFlippedStateNameProperty, value);
            }
        }

        #endregion PortraitFlippedStateName

        #endregion DependencyProperties

        //#region Properties

        //private bool IsTargetObjectSet
        //{
        //    get
        //    {
        //        return (base.ReadLocalValue(OrientationToStateAction.TargetObjectProperty) != DependencyProperty.UnsetValue);
        //    }
        //}

        //private FrameworkElement StateTarget { get; set; }

        //#endregion Properties

        /// <summary>
        /// Transitions the control to a specified state depending on display orientation.
        /// </summary>
        /// <param name="sender">
        /// <see cref="T:Windows.UI.Xaml.DependencyObject"/> to which trigger is attached.
        /// </param>
        /// <param name="parameter">
        /// New device display orientation of type 
        /// <see cref="T:Windows.Graphics.Display.DisplayOrientations"/>.
        /// </param>
        /// <returns>
        /// True if the action is successfully executed and control is transitioned to the new state 
        /// depending on orientation; otherwise, false.
        /// </returns>
        public object Execute(object sender, object parameter)
        {
            if (!Enum.IsDefined(typeof(DisplayOrientations), parameter))
                return false;

            string stateName;
            var viewState = (DisplayOrientations)parameter;
            switch (viewState)
            {
                case DisplayOrientations.Landscape:
                    stateName = LandscapeStateName;
                    break;

                case DisplayOrientations.Portrait:
                    stateName = PortraitStateName;
                    break;

                case DisplayOrientations.LandscapeFlipped:
                    stateName = LandscapeFlippedStateName;
                    break;

                case DisplayOrientations.PortraitFlipped:
                    stateName = PortraitFlippedStateName;
                    break;

                default:
                    stateName = string.Empty;
                    break;
            }

            if (string.IsNullOrEmpty(stateName))
                stateName = viewState.ToString();

            FrameworkElement targetObject;
            if (base.ReadLocalValue(TargetObjectProperty) != DependencyProperty.UnsetValue)
            {
                targetObject = this.TargetObject;
            }
            else
            {
                targetObject = sender as FrameworkElement;
            }
            if (targetObject == null)
            {
                return false;
            }
            Control control = VisualStateUtilities.FindNearestStatefulControl(targetObject);
            if (control == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Target {0} does not define any VisualStateGroups.", new object[] { targetObject.Name }));
            }

            //return VisualStateUtilities.GoToState(control, stateName, this.UseTransitions);
            return VisualStateUtilities.GoToState(control, stateName, false);
        }

        //public static Control FindNearestStatefulControl(FrameworkElement element)
        //{
        //    if (element == null)
        //    {
        //        throw new ArgumentNullException("element");
        //    }
        //    for (FrameworkElement element2 = element.Parent as FrameworkElement; !HasVisualStateGroupsDefined(element) && ShouldContinueTreeWalk(element2); element2 = element2.Parent as FrameworkElement)
        //    {
        //        element = element2;
        //    }
        //    if (!HasVisualStateGroupsDefined(element))
        //    {
        //        return null;
        //    }
        //    Control parent = Windows.UI.Xaml.Media.VisualTreeHelper.GetParent(element) as Control;
        //    if (parent != null)
        //    {
        //        return parent;
        //    }
        //    return (element as Control);
        //}

        //private static bool HasVisualStateGroupsDefined(FrameworkElement element)
        //{
        //    return ((element != null) && (VisualStateManager.GetVisualStateGroups(element).Count != 0));
        //}

        //private static bool ShouldContinueTreeWalk(FrameworkElement element)
        //{
        //    if (element == null)
        //    {
        //        return false;
        //    }
        //    if (element is UserControl)
        //    {
        //        return false;
        //    }
        //    if (element.Parent == null)
        //    {
        //        FrameworkElement parent = Windows.UI.Xaml.Media.VisualTreeHelper.GetParent(element) as FrameworkElement;
        //        if ((parent == null) || (!(parent is Control) && !(parent is ContentPresenter)))
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //}
    }
}