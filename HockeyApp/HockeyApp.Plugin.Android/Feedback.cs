using System;
using Plugin.HockeyApp.Abstractions;

namespace Plugin.HockeyApp
{
    public class Feedback : IFeedback
    {
        public void ComposeFeedback()
        {
            global::HockeyApp.FeedbackManager.ShowFeedbackActivity(global::Android.App.Application.Context);
        }

        public void ShowFeedback()
        {
            global::HockeyApp.FeedbackManager.ShowFeedbackActivity(global::Android.App.Application.Context);
        }
    }
}
