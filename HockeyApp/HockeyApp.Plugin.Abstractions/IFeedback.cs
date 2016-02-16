using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Plugin.HockeyApp.Abstractions
{
    public interface IFeedback
    {
        void ComposeFeedback();
        void ShowFeedback();
    }
}
