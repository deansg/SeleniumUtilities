using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SeleniumUtilities
{
    /// <summary>
    /// A class containing methods for common waiting operations in Selenium
    /// </summary>
    public class WaitUntil
    {
        #region Static Constructor
        
        static WaitUntil()
        {
            DefaultTimeout = TimeSpan.FromSeconds(20);
            PollingInterval = TimeSpan.FromMilliseconds(200);
        }

        #endregion

        #region Static Properties

        /// <summary>
        /// The default timeout for the waiting operations
        /// </summary>
        public static TimeSpan DefaultTimeout { get; set; }

        /// <summary>
        /// The default polling interval used in implementing the waiting operations
        /// </summary>
        public static TimeSpan PollingInterval { get; set; }

        #endregion

        #region Fields

        private readonly IWebDriver _driver;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the WaitUntil class linked to the given IWebDriver object
        /// </summary>
        /// <param name="driver"></param>
        public WaitUntil(IWebDriver driver)
        {
            _driver = driver;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Waits until an element satisfying the given By condition exists
        /// </summary>
        /// <param name="by">Used to search for the element</param>
        /// <param name="timeout">Maximum timeout for the operation</param>
        /// <returns>The element, once it is found</returns>
        /// <exception cref="WebDriverTimeoutException"/>
        public IWebElement ElementExists(By by, TimeSpan timeout = default(TimeSpan))
        {
            SetTimeout(ref timeout);
            return new WebDriverWait(_driver, timeout).Until(ExpectedConditions.ElementExists(by));
        }

        /// <summary>
        /// Waits until no element satisfies the given By condition
        /// </summary>
        /// <param name="by">Used to search for elements</param>
        /// <param name="timeout">Maximum timeout for the operation</param>
        /// <exception cref="WebDriverTimeoutException"/>
        public void ElementDoesNotExist(By by, TimeSpan timeout = default(TimeSpan))
        {
            SetTimeout(ref timeout);
            new WebDriverWait(_driver, timeout).Until(dr => dr.FindElements(by).Count == 0);
        }

        #endregion

        #region Private Methods

        private static void SetTimeout(ref TimeSpan timeout)
        {
            timeout = timeout == default(TimeSpan) ? DefaultTimeout : timeout;
        }

        private static IWebElement ElementSatisfiesCondition(IWebElement element, string failureMessage, 
            Func<IWebElement, bool> predicate, TimeSpan timeout = default(TimeSpan))
        {
            SetTimeout(ref timeout);
            var stopwatch = Stopwatch.StartNew();
            while (stopwatch.Elapsed < timeout)
            {
                if (predicate(element))
                    return element;
                Thread.Sleep(PollingInterval);
            }
            throw new WebDriverTimeoutException($"{failureMessage} after a timeout of {timeout.TotalSeconds} seconds");
        }

        #endregion
    }
}