using System;
using Plugin.HockeyApp.Abstractions;

namespace Plugin.HockeyApp
{
    public class Feedback : IFeedback
    {
        public void ComposeFeedback()
        {
            global::HockeyApp.BITHockeyManager.SharedHockeyManager.FeedbackManager.ShowFeedbackComposeView();

        }

        public void ShowFeedback()
        {
            global::HockeyApp.BITHockeyManager.SharedHockeyManager.FeedbackManager.ShowFeedbackListView();
        }
    }
}
