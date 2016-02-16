using System;
using Plugin.HockeyApp.Abstractions;
using HockeyApp;

namespace Plugin.HockeyApp
{
    public class Feedback : IFeedback
    {
        public void ComposeFeedback()
        {
            HockeyClient.Current.ShowFeedback();
        }

        public void ShowFeedback()
        {
            HockeyClient.Current.ShowFeedback();
        }
    }
}

// http://support.hockeyapp.net/kb/client-integration-windows-and-windows-phone/hockeyapp-for-windows-store-apps-and-windows-phone-store-apps