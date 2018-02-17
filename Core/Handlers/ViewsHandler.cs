using System.Collections.Generic;
using System.Windows;
using Unity;

namespace Labs.WPF.Core.Handlers
{
    public class ViewsHandler
    {
        #region Constructor

        private ViewsHandler()
        {
            this.Views = new Dictionary<string, UIElement>();
        }

        #endregion

        #region Singleton

        private static ViewsHandler _instance;
        public static ViewsHandler Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ViewsHandler();

                return _instance;
            }
        }

        #endregion

        #region Properties

        private Dictionary<string, UIElement> Views { get; set; }

        #endregion

        #region Methods

        public void RegisterView(UIElement window)
        {
            var uiElementName = window.GetType().Name;
            if (!this.Views.ContainsKey(uiElementName))
                this.Views.Add(uiElementName, window);
        }

        public UIElement GetView(string name)
        {
            if (this.Views.ContainsKey(name))
                return this.Views[name];

            return null;
        }

        //public T CreateView<T>(Type type)
        //{
        //    return null;
        //}

        #endregion
    }
}
