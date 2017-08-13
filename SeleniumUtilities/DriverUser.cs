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

        /// <summary>
        /// Creates a new DriverUser instance based on the given IWebDriver
        /// </summary>
        /// <param name="driver">The IWebDriver instance to use</param>
        protected DriverUser(IWebDriver driver)
        {
            Driver = driver;
            WaitUntil = new WaitUntil(driver);
        }

        /// <summary>
        /// Creates a new DriverUser instance based on the given DriverUser's IWebDriver
        /// </summary>
        /// <param name="driverUser">The DriverUser whose IWebDriver is to be used</param>
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
