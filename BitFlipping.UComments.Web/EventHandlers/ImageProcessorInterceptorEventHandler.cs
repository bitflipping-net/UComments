using ImageProcessor.Web.HttpModules;
using Umbraco.Core;

namespace BaseSite.Web
{
    /// <summary>
    /// Taps into ImageProcessor.Web events to intecept image requests to adjust output quality file size.
    /// </summary>
    public class ImageProcessorInterceptorEventHandler : IApplicationEventHandler
    {
        #region events
        public void OnApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
        }
        public void OnApplicationInitialized(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
        }
        #endregion

        /// <summary>
        /// Boot-up is completed, this allows you to perform any other boot-up logic required for the application.
        /// Resolution is frozen so now they can be used to resolve instances.
        /// </summary>
        /// <param name="umbracoApplication">The current <see cref="UmbracoApplicationBase"/></param>
        /// <param name="applicationContext">The Umbraco <see cref="ApplicationContext"/> for the current application.</param>
        public void OnApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            // Intercept ImageProcessor requests to alter output quality and file size.
            ImageProcessingModule.ValidatingRequest += (sender, args) =>
            {
                if ((args.Context.Request.RawUrl.Contains(".jpg") || args.Context.Request.RawUrl.Contains(".jpeg")) && !args.QueryString.Contains("quality="))
                {
                    args.QueryString += "&quality=85";
                }

                if (args.Context.Request.RawUrl.Contains(".png") && !args.QueryString.Contains("format=png8"))
                {
                    args.QueryString += "&format=png8";
                }
            };
        }
    }
}