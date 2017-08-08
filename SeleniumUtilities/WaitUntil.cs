using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}