using OpenQA.Selenium;

namespace SeleniumUtilities
{
    /// <summary>
    /// An abstract base class for classes that use an IWebDriver. These can usually include logical automation
    /// representations of different pages or windows in a website. The website-wrapper itself should inherit from
    /// the deriving subclass, WebsiteWrapperBase
    /// </summary>
    public abstract class DriverUser
    {
        #region Constructors

        protected DriverUser(IWebDriver driver)
        {
            Driver = driver;
            WaitUntil = new WaitUntil(driver);
        }

        protected DriverUser(DriverUser driverUser)
        {
            Driver = driverUser.Driver;
            WaitUntil = driverUser.WaitUntil;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The inner IWebDriver used for most Selenium operations
        /// </summary>
        protected IWebDriver Driver { get; }

        /// <summary>
        /// A WaitUntil object linked to the Driver object
        /// </summary>
        protected WaitUntil WaitUntil { get; }

        #endregion
    }
}
