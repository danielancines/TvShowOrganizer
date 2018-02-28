using System;
using System.Collections.Generic;
using System.Windows;

namespace Labs.WPF.Core.Handlers
{
    public class ViewsHandler
    {
        #region Constructor

        private ViewsHandler()
        {
            this.Views = new Dictionary<Guid, UIElement>();
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

        private Dictionary<Guid, UIElement> Views { get; set; }

        #endregion

        #region Methods

        public Guid RegisterView(UIElement window)
        {
            return this.RegisterView(window, Guid.NewGuid());
        }

        public Guid RegisterView(UIElement window, Guid elementId)
        {
            if (!this.Views.ContainsKey(elementId))
                this.Views.Add(elementId, window);

            return elementId;
        }

        public UIElement GetView(Guid elementId)
        {
            if (this.Views.ContainsKey(elementId))
                return this.Views[elementId];

            return null;
        }

        #endregion
    }
}
