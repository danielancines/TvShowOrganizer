using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Labs.WPF.Core.Behaviors
{
    public class KeyUpBehavior : Behavior<UIElement>
    {
        #region Behavior

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.KeyUp += AssociatedObject_KeyUp;
        }

        private void AssociatedObject_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != this.Key || this.Command == null)
                return;

            var textProperty = this.AssociatedObject.GetType().GetProperty("Text");
            if (textProperty != null)
                this.Command.Execute(textProperty.GetValue(this.AssociatedObject));
            else
                this.Command.Execute(e.Key);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.KeyUp -= AssociatedObject_KeyUp;
        }

        #endregion

        #region Properties

        public Key Key { get; set; }

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(KeyUpBehavior), null);
        public ICommand Command
        {
            get
            {
                return (ICommand)this.GetValue(CommandProperty);
            }
            set
            {
                this.SetValue(CommandProperty, value);
            }
        }

        #endregion
    }
}
