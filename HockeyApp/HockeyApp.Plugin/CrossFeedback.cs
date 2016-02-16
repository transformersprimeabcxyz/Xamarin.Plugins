using System;
using Plugin.HockeyApp.Abstractions;

namespace Plugin.HockeyApp
{
    public static class CrossFeedback
    {

        static Lazy<IFeedback> TTS = new Lazy<IFeedback>(() => CreateFeedback(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

        public static IFeedback Current
        {
            get
            {
                var ret = TTS.Value;
                if (ret == null)
                {
                    throw NotImplementedInReferenceAssembly();
                }
                return ret;
            }
        }

        static IFeedback CreateFeedback()
        {
#if PORTABLE
            return null;
#else
            return new Feedback();
#endif
        }

        internal static Exception NotImplementedInReferenceAssembly()
        {
            return new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the Plugins.HockeyApp NuGet package from your main application project in order to reference the platform-specific implementation.");
        }

    }

}
