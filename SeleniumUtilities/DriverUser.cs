using OpenQA.Selenium;

namespace SeleniumUtilities
{
    /// <summary>
    /// An abstract base class for classes that use an IWebDriver. These can usually include logical automation
    /// representations of different pages or windows in a website, but usually not the website-wrapper itself.
    /// </summary>
    public abstract class DriverUser
    {
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

        public IWebDriver Driver { get; }

        public WaitUntil WaitUntil { get; }
    }
}
