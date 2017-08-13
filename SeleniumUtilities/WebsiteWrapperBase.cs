using OpenQA.Selenium;
using System;

namespace SeleniumUtilities
{
    /// <summary>
    /// An abstract base class for classes that represent a logical website automated by Selenium. It inherits the
    /// DriverUser class, but is also responsible for Disposing the IWebDriver object, and increases its visibilty
    /// </summary>
    public abstract class WebsiteWrapperBase : DriverUser, IDisposable
    {
        /// <summary>
        /// Creates a new instance of the WebsiteWrapperBase class based on the given IWebDriver
        /// </summary>
        /// <param name="driver">The IWebDriver instance to use</param>
        protected WebsiteWrapperBase(IWebDriver driver) : base(driver)
        {
        }

        /// <summary>
        /// The IWebDriver used for most Selenium operations
        /// </summary>
        public new IWebDriver Driver { get { return base.Driver; } }

        /// <summary>
        /// Disposes the current IWebDriver if it isn't null
        /// </summary>
        public void Dispose()
        {
            if (Driver != null)
            {
                Driver.Dispose();
            }
        }
    }
}