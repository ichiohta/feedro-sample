using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Display;
using Windows.UI.Core;
using Windows.UI.ViewManagement;

namespace Feedro.View.Util
{
    public static class ViewStatus
    {
        public const double NarrowViewMaximumWidth = 400;
        public const double ModerateViewMaximumWidth = 800;

        public enum ViewMode
        {
            Narrow,
            Moderate,
            Wide
        }

        public static ViewMode CurrentViewMode
        {
            get
            {
                ApplicationView view = ApplicationView.GetForCurrentView();
                double width = CoreWindow.GetForCurrentThread().Bounds.Width;

                if (view.Orientation == ApplicationViewOrientation.Portrait)
                    return ViewMode.Moderate;
                else if (width <= NarrowViewMaximumWidth)
                    return ViewMode.Narrow;
                else if (width <= ModerateViewMaximumWidth)
                    return ViewMode.Moderate;
                else
                    return ViewMode.Wide;
            }
        }
    }
}
