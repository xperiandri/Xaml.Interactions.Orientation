using System;
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
using Microsoft.Expression.Prototyping.WindowsPhone.Mockups;

namespace XperiAndri.Interactivity
{
    /// <summary>
    /// Represents a trigger that performs actions when orientaion of the device have changed. 
    /// </summary>
    public class OrientationTrigger : System.Windows.Interactivity.TriggerBase<FrameworkElement>
    {
        private WindowsPhoneChrome chrome;

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.Loaded += AssociatedObjectLoaded;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            if (chrome != null)
            {
                chrome.OrientationChanged -= OrientationChanged;
                chrome = null;
            }
        }

        void AssociatedObjectLoaded(object sender, RoutedEventArgs e)
        {
            InitWhenReady();
            AssociatedObject.Loaded -= AssociatedObjectLoaded;
        }

        private void InitWhenReady()
        {
            WindowsPhoneChrome chrome = null;
            Dispatcher.BeginInvoke(() =>
            {
                FrameworkElement element;
                FrameworkElement parent = AssociatedObject;
                do
                {
                    element = parent;
                    parent = element.Parent as FrameworkElement;
                    if (parent == null)
                        parent = VisualTreeHelper.GetParent(element) as FrameworkElement;
                    chrome = parent as WindowsPhoneChrome;
                }
                while (chrome == null && parent != null);
                if (chrome != null)
                {
                    chrome.OrientationChanged += OrientationChanged;
                    InvokeActions(chrome.PageOrientation);
                }
                else //If rootvisual is still not set, wait some more
                    InitWhenReady();
            });
            this.chrome = chrome;
        }

        private void OrientationChanged(object sender, OrientationChangedEventArgs orientationChangedEventArgs)
        {
            InvokeActions(orientationChangedEventArgs.PageOrientation);
        }
    }
}

//    public abstract class EventTriggerBase : System.Windows.Interactivity.TriggerBase
//    {
//        // Fields
//        private MethodInfo eventHandlerMethodInfo;
//        private bool isSourceChangedRegistered;
//        public static readonly DependencyProperty SourceNameProperty = DependencyProperty.Register("SourceName", typeof(string), typeof(EventTriggerBase), new PropertyMetadata(new PropertyChangedCallback(EventTriggerBase.OnSourceNameChanged)));
//        private NameResolver sourceNameResolver;
//        public static readonly DependencyProperty SourceObjectProperty = DependencyProperty.Register("SourceObject", typeof(object), typeof(EventTriggerBase), new PropertyMetadata(new PropertyChangedCallback(EventTriggerBase.OnSourceObjectChanged)));
//        private Type sourceTypeConstraint;

//        // Methods
//        internal EventTriggerBase(Type sourceTypeConstraint)
//            : base(typeof(DependencyObject))
//        {
//            this.sourceTypeConstraint = sourceTypeConstraint;
//            this.sourceNameResolver = new NameResolver();
//            this.RegisterSourceChanged();
//        }

//        protected abstract string GetEventName();

//        private static bool IsValidEvent(EventInfo eventInfo)
//        {
//            Type eventHandlerType = eventInfo.EventHandlerType;
//            if (!typeof(Delegate).IsAssignableFrom(eventInfo.EventHandlerType))
//            {
//                return false;
//            }
//            ParameterInfo[] parameters = eventHandlerType.GetMethod("Invoke").GetParameters();
//            return (((parameters.Length == 2) && typeof(object).IsAssignableFrom(parameters[0].ParameterType)) && typeof(EventArgs).IsAssignableFrom(parameters[1].ParameterType));
//        }

//        protected override void OnAttached()
//        {
//            base.OnAttached();
//            DependencyObject associatedObject = base.AssociatedObject;
//            Behavior behavior = associatedObject as Behavior;
//            FrameworkElement element = associatedObject as FrameworkElement;
//            this.RegisterSourceChanged();
//            if (behavior != null)
//            {
//                associatedObject = ((IAttachedObject)behavior).AssociatedObject;
//                behavior.AssociatedObjectChanged += new EventHandler(this.OnBehaviorHostChanged);
//            }
//            else if ((this.SourceObject != null) || (element == null))
//            {
//                try
//                {
//                    this.OnSourceChanged(null, this.Source);
//                }
//                catch (InvalidOperationException)
//                {
//                }
//            }
//            else
//            {
//                this.SourceNameResolver.NameScopeReferenceElement = element;
//            }
//            if (((string.Compare(this.GetEventName(), "Loaded", StringComparison.Ordinal) == 0) && (element != null)) && !IsElementLoaded(element))
//            {
//                this.RegisterLoaded(element);
//            }
//        }

//        internal static bool IsElementLoaded(FrameworkElement element)
//        {
//            UIElement rootVisual = Application.Current.RootVisual;
//            DependencyObject parent = element.Parent;
//            if (parent == null)
//            {
//                parent = VisualTreeHelper.GetParent(element);
//            }
//            return ((parent != null) || ((rootVisual != null) && (element == rootVisual)));
//        }

//        private void OnBehaviorHostChanged(object sender, EventArgs e)
//        {
//            this.SourceNameResolver.NameScopeReferenceElement = ((IAttachedObject)sender).AssociatedObject as FrameworkElement;
//        }

//        protected override void OnDetaching()
//        {
//            base.OnDetaching();
//            Behavior associatedObject = base.AssociatedObject as Behavior;
//            FrameworkElement associatedElement = base.AssociatedObject as FrameworkElement;
//            try
//            {
//                this.OnSourceChanged(this.Source, null);
//            }
//            catch (InvalidOperationException)
//            {
//            }
//            this.UnregisterSourceChanged();
//            if (associatedObject != null)
//            {
//                associatedObject.AssociatedObjectChanged -= new EventHandler(this.OnBehaviorHostChanged);
//            }
//            this.SourceNameResolver.NameScopeReferenceElement = null;
//            if ((string.Compare(this.GetEventName(), "Loaded", StringComparison.Ordinal) == 0) && (associatedElement != null))
//            {
//                this.UnregisterLoaded(associatedElement);
//            }
//        }

//        protected virtual void OnEvent(EventArgs eventArgs)
//        {
//            base.InvokeActions(eventArgs);
//        }

//        private void OnEventImpl(object sender, EventArgs eventArgs)
//        {
//            this.OnEvent(eventArgs);
//        }

//        internal void OnEventNameChanged(string oldEventName, string newEventName)
//        {
//            if (base.AssociatedObject != null)
//            {
//                FrameworkElement source = this.Source as FrameworkElement;
//                if ((source != null) && (string.Compare(oldEventName, "Loaded", StringComparison.Ordinal) == 0))
//                {
//                    this.UnregisterLoaded(source);
//                }
//                else if (!string.IsNullOrEmpty(oldEventName))
//                {
//                    this.UnregisterEvent(this.Source, oldEventName);
//                }
//                if ((source != null) && (string.Compare(newEventName, "Loaded", StringComparison.Ordinal) == 0))
//                {
//                    this.RegisterLoaded(source);
//                }
//                else if (!string.IsNullOrEmpty(newEventName))
//                {
//                    this.RegisterEvent(this.Source, newEventName);
//                }
//            }
//        }

//        private void OnSourceChanged(object oldSource, object newSource)
//        {
//            if (base.AssociatedObject != null)
//            {
//                this.OnSourceChangedImpl(oldSource, newSource);
//            }
//        }

//        internal virtual void OnSourceChangedImpl(object oldSource, object newSource)
//        {
//            if (!string.IsNullOrEmpty(this.GetEventName()) && (string.Compare(this.GetEventName(), "Loaded", StringComparison.Ordinal) != 0))
//            {
//                if ((oldSource != null) && this.SourceTypeConstraint.IsAssignableFrom(oldSource.GetType()))
//                {
//                    this.UnregisterEvent(oldSource, this.GetEventName());
//                }
//                if ((newSource != null) && this.SourceTypeConstraint.IsAssignableFrom(newSource.GetType()))
//                {
//                    this.RegisterEvent(newSource, this.GetEventName());
//                }
//            }
//        }

//        private static void OnSourceNameChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
//        {
//            EventTriggerBase base2 = (EventTriggerBase)obj;
//            base2.SourceNameResolver.Name = (string)args.NewValue;
//        }

//        private void OnSourceNameResolverElementChanged(object sender, NameResolvedEventArgs e)
//        {
//            if (this.SourceObject == null)
//            {
//                this.OnSourceChanged(e.OldObject, e.NewObject);
//            }
//        }

//        private static void OnSourceObjectChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
//        {
//            EventTriggerBase base2 = (EventTriggerBase)obj;
//            object newSource = base2.SourceNameResolver.Object;
//            if (args.NewValue == null)
//            {
//                base2.OnSourceChanged(args.OldValue, newSource);
//            }
//            else
//            {
//                if ((args.OldValue == null) && (newSource != null))
//                {
//                    base2.UnregisterEvent(newSource, base2.GetEventName());
//                }
//                base2.OnSourceChanged(args.OldValue, args.NewValue);
//            }
//        }

//        private void RegisterEvent(object obj, string eventName)
//        {
//            EventInfo eventInfo = obj.GetType().GetEvent(eventName);
//            if (eventInfo == null)
//            {
//                if (this.SourceObject != null)
//                {
//                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, ExceptionStringTable.EventTriggerCannotFindEventNameExceptionMessage, new object[] { eventName, obj.GetType().Name }));
//                }
//            }
//            else if (!IsValidEvent(eventInfo))
//            {
//                if (this.SourceObject != null)
//                {
//                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, ExceptionStringTable.EventTriggerBaseInvalidEventExceptionMessage, new object[] { eventName, obj.GetType().Name }));
//                }
//            }
//            else
//            {
//                this.eventHandlerMethodInfo = typeof(EventTriggerBase).GetMethod("OnEventImpl", BindingFlags.NonPublic | BindingFlags.Instance);
//                eventInfo.AddEventHandler(obj, Delegate.CreateDelegate(eventInfo.EventHandlerType, this, this.eventHandlerMethodInfo));
//            }
//        }

//        private void RegisterLoaded(FrameworkElement associatedElement)
//        {
//            if (!this.IsLoadedRegistered && (associatedElement != null))
//            {
//                associatedElement.Loaded += new RoutedEventHandler(this.OnEventImpl);
//                this.IsLoadedRegistered = true;
//            }
//        }

//        private void RegisterSourceChanged()
//        {
//            if (!this.IsSourceChangedRegistered)
//            {
//                this.SourceNameResolver.ResolvedElementChanged += new EventHandler<NameResolvedEventArgs>(this.OnSourceNameResolverElementChanged);
//                this.IsSourceChangedRegistered = true;
//            }
//        }

//        private void UnregisterEvent(object obj, string eventName)
//        {
//            if (string.Compare(eventName, "Loaded", StringComparison.Ordinal) == 0)
//            {
//                FrameworkElement associatedElement = obj as FrameworkElement;
//                if (associatedElement != null)
//                {
//                    this.UnregisterLoaded(associatedElement);
//                }
//            }
//            else
//            {
//                this.UnregisterEventImpl(obj, eventName);
//            }
//        }

//        private void UnregisterEventImpl(object obj, string eventName)
//        {
//            Type type = obj.GetType();
//            if (this.eventHandlerMethodInfo != null)
//            {
//                EventInfo info = type.GetEvent(eventName);
//                info.RemoveEventHandler(obj, Delegate.CreateDelegate(info.EventHandlerType, this, this.eventHandlerMethodInfo));
//                this.eventHandlerMethodInfo = null;
//            }
//        }

//        private void UnregisterLoaded(FrameworkElement associatedElement)
//        {
//            if (this.IsLoadedRegistered && (associatedElement != null))
//            {
//                associatedElement.Loaded -= new RoutedEventHandler(this.OnEventImpl);
//                this.IsLoadedRegistered = false;
//            }
//        }

//        private void UnregisterSourceChanged()
//        {
//            if (this.IsSourceChangedRegistered)
//            {
//                this.SourceNameResolver.ResolvedElementChanged -= new EventHandler<NameResolvedEventArgs>(this.OnSourceNameResolverElementChanged);
//                this.IsSourceChangedRegistered = false;
//            }
//        }

//        // Properties
//        protected sealed override Type AssociatedObjectTypeConstraint
//        {
//            get
//            {
//                object[] customAttributes = base.GetType().GetCustomAttributes(typeof(TypeConstraintAttribute), true);
//                int index = 0;
//                while (index < customAttributes.Length)
//                {
//                    TypeConstraintAttribute attribute = (TypeConstraintAttribute)customAttributes[index];
//                    return attribute.Constraint;
//                }
//                return typeof(DependencyObject);
//            }
//        }

//        private bool IsLoadedRegistered { get; set; }

//        private bool IsSourceChangedRegistered
//        {
//            get
//            {
//                return this.isSourceChangedRegistered;
//            }
//            set
//            {
//                this.isSourceChangedRegistered = value;
//            }
//        }

//        private bool IsSourceNameSet
//        {
//            get
//            {
//                if (string.IsNullOrEmpty(this.SourceName))
//                {
//                    return (base.ReadLocalValue(SourceNameProperty) != DependencyProperty.UnsetValue);
//                }
//                return true;
//            }
//        }

//        public object Source
//        {
//            get
//            {
//                object associatedObject = base.AssociatedObject;
//                if (this.SourceObject != null)
//                {
//                    return this.SourceObject;
//                }
//                if (this.IsSourceNameSet)
//                {
//                    associatedObject = this.SourceNameResolver.Object;
//                    if ((associatedObject != null) && !this.SourceTypeConstraint.IsAssignableFrom(associatedObject.GetType()))
//                    {
//                        throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, ExceptionStringTable.RetargetedTypeConstraintViolatedExceptionMessage, new object[] { base.GetType().Name, associatedObject.GetType(), this.SourceTypeConstraint, "Source" }));
//                    }
//                }
//                return associatedObject;
//            }
//        }

//        public string SourceName
//        {
//            get
//            {
//                return (string)base.GetValue(SourceNameProperty);
//            }
//            set
//            {
//                base.SetValue(SourceNameProperty, value);
//            }
//        }

//        private NameResolver SourceNameResolver
//        {
//            get
//            {
//                return this.sourceNameResolver;
//            }
//        }

//        public object SourceObject
//        {
//            get
//            {
//                return base.GetValue(SourceObjectProperty);
//            }
//            set
//            {
//                base.SetValue(SourceObjectProperty, value);
//            }
//        }

//        protected Type SourceTypeConstraint
//        {
//            get
//            {
//                return this.sourceTypeConstraint;
//            }
//        }
//    }

//    internal sealed class NameResolvedEventArgs : EventArgs
//    {
//        // Fields
//        private object newObject;
//        private object oldObject;

//        // Methods
//        public NameResolvedEventArgs(object oldObject, object newObject)
//        {
//            this.oldObject = oldObject;
//            this.newObject = newObject;
//        }

//        // Properties
//        public object NewObject
//        {
//            get
//            {
//                return this.newObject;
//            }
//        }

//        public object OldObject
//        {
//            get
//            {
//                return this.oldObject;
//            }
//        }
//    }

//    internal sealed class NameResolver
//    {
//        // Fields
//        private string name;
//        private FrameworkElement nameScopeReferenceElement;

//        // Events
//        public event EventHandler<NameResolvedEventArgs> ResolvedElementChanged;

//        // Methods
//        private FrameworkElement GetActualNameScopeReference(FrameworkElement initialReferenceElement)
//        {
//            FrameworkElement element = initialReferenceElement;
//            if (!this.IsNameScope(initialReferenceElement))
//            {
//                return element;
//            }
//            return ((initialReferenceElement.Parent as FrameworkElement) ?? element);
//        }

//        private bool IsNameScope(FrameworkElement frameworkElement)
//        {
//            FrameworkElement parent = frameworkElement.Parent as FrameworkElement;
//            return ((parent != null) && (parent.FindName(this.Name) != null));
//        }

//        private void OnNameScopeReferenceElementChanged(FrameworkElement oldNameScopeReference)
//        {
//            if (this.PendingReferenceElementLoad)
//            {
//                oldNameScopeReference.Loaded -= new RoutedEventHandler(this.OnNameScopeReferenceLoaded);
//                this.PendingReferenceElementLoad = false;
//            }
//            this.HasAttempedResolve = false;
//            this.UpdateObjectFromName(this.Object);
//        }

//        private void OnNameScopeReferenceLoaded(object sender, RoutedEventArgs e)
//        {
//            this.PendingReferenceElementLoad = false;
//            this.NameScopeReferenceElement.Loaded -= new RoutedEventHandler(this.OnNameScopeReferenceLoaded);
//            this.UpdateObjectFromName(this.Object);
//        }

//        private void OnObjectChanged(DependencyObject oldTarget, DependencyObject newTarget)
//        {
//            if (this.ResolvedElementChanged != null)
//            {
//                this.ResolvedElementChanged(this, new NameResolvedEventArgs(oldTarget, newTarget));
//            }
//        }

//        private void UpdateObjectFromName(DependencyObject oldObject)
//        {
//            DependencyObject obj2 = null;
//            this.ResolvedObject = null;
//            if (this.NameScopeReferenceElement != null)
//            {
//                if (!IsElementLoaded(this.NameScopeReferenceElement))
//                {
//                    this.NameScopeReferenceElement.Loaded += new RoutedEventHandler(this.OnNameScopeReferenceLoaded);
//                    this.PendingReferenceElementLoad = true;
//                    return;
//                }
//                if (!string.IsNullOrEmpty(this.Name))
//                {
//                    FrameworkElement actualNameScopeReferenceElement = this.ActualNameScopeReferenceElement;
//                    if (actualNameScopeReferenceElement != null)
//                    {
//                        obj2 = actualNameScopeReferenceElement.FindName(this.Name) as DependencyObject;
//                    }
//                }
//            }
//            this.HasAttempedResolve = true;
//            this.ResolvedObject = obj2;
//            if (oldObject != this.Object)
//            {
//                this.OnObjectChanged(oldObject, this.Object);
//            }
//        }

//        // Properties
//        private FrameworkElement ActualNameScopeReferenceElement
//        {
//            get
//            {
//                if ((this.NameScopeReferenceElement != null) && IsElementLoaded(this.NameScopeReferenceElement))
//                {
//                    return this.GetActualNameScopeReference(this.NameScopeReferenceElement);
//                }
//                return null;
//            }
//        }

//        private bool HasAttempedResolve { get; set; }

//        public string Name
//        {
//            get
//            {
//                return this.name;
//            }
//            set
//            {
//                DependencyObject oldObject = this.Object;
//                this.name = value;
//                this.UpdateObjectFromName(oldObject);
//            }
//        }

//        public FrameworkElement NameScopeReferenceElement
//        {
//            get
//            {
//                return this.nameScopeReferenceElement;
//            }
//            set
//            {
//                FrameworkElement nameScopeReferenceElement = this.NameScopeReferenceElement;
//                this.nameScopeReferenceElement = value;
//                this.OnNameScopeReferenceElementChanged(nameScopeReferenceElement);
//            }
//        }

//        public DependencyObject Object
//        {
//            get
//            {
//                if (string.IsNullOrEmpty(this.Name) && this.HasAttempedResolve)
//                {
//                    return this.NameScopeReferenceElement;
//                }
//                return this.ResolvedObject;
//            }
//        }

//        private bool PendingReferenceElementLoad { get; set; }

//        private DependencyObject ResolvedObject { get; set; }

//        internal static bool IsElementLoaded(FrameworkElement element)
//        {
//            UIElement rootVisual = Application.Current.RootVisual;
//            DependencyObject parent = element.Parent;
//            if (parent == null)
//            {
//                parent = VisualTreeHelper.GetParent(element);
//            }
//            return ((parent != null) || ((rootVisual != null) && (element == rootVisual)));
//        }
//    }
//}