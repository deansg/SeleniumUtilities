using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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
        /// <param name="timeout">Maximal timeout for the operation</param>
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
        /// <param name="timeout">Maximal timeout for the operation</param>
        /// <exception cref="WebDriverTimeoutException"/>
        public void ElementDoesNotExist(By by, TimeSpan timeout = default(TimeSpan))
        {
            SetTimeout(ref timeout);
            new WebDriverWait(_driver, timeout).Until(dr => dr.FindElements(by).Count == 0);
        }

        /// <summary>
        /// Waits until an element satisfying the given By condition exists and is visible
        /// </summary>
        /// <param name="by">Used to search for the element</param>
        /// <param name="timeout">Maximal timeout for the operation</param>
        /// <returns>The element, once it is found</returns>
        /// <exception cref="WebDriverTimeoutException"/>
        public IWebElement ElementIsVisible(By by, TimeSpan timeout = default(TimeSpan))
        {
            SetTimeout(ref timeout);
            return new WebDriverWait(_driver, timeout).Until(ExpectedConditions.ElementIsVisible(by));
        }

        /// <summary>
        /// Waits until the IWebDriver's url satisfies the given condition
        /// </summary>
        /// <param name="urlPredicate">The predicate to be run against the current URL</param>
        /// <param name="timeout">Maximal timeout for the operation</param>
        /// <returns>The url satisfying the given condition</returns>
        /// <exception cref="WebDriverTimeoutException"/>
        public string UrlSatisfiesCondition(Func<string, bool> urlPredicate, TimeSpan timeout = default(TimeSpan))
        {
            SetTimeout(ref timeout);
            return new WebDriverWait(_driver, timeout).Until(dr =>
            {
                string url = dr.Url;

                // returning the url will always terminate the wait as driver.url cannot be null
                if (urlPredicate(url))
                    return url;
                return null;
            });
        }

        /// <summary>
        /// Waits until the url is not empty. Particularly useful when switching to a newly opened tab.
        /// </summary>
        /// <param name="timeout">Maximal timeout for the operation</param>
        /// <returns>The non-empty url</returns>
        /// <exception cref="WebDriverTimeoutException"/>
        public string UrlIsNotEmpty(TimeSpan timeout = default(TimeSpan))
        {
            return UrlSatisfiesCondition(url => !string.IsNullOrEmpty(url));
        }

        /// <summary>
        /// Waits until the url is equal to the expected url
        /// </summary>
        /// <param name="expectedUrl">The expected url</param>
        /// <param name="timeout">Maximal timeout for the operation</param>
        /// <returns>The expected url</returns>
        /// <exception cref="WebDriverTimeoutException"/>
        public string UrlEquals(string expectedUrl, TimeSpan timeout = default(TimeSpan))
        {
            return UrlSatisfiesCondition(url => url == expectedUrl);
        }

        /// <summary>
        /// Waits until the url starts with the given prefix
        /// </summary>
        /// <param name="expectedUrlPrefix">The expected prefix of the url</param>
        /// <param name="timeout">Maximal timeout for the operation</param>
        /// <returns>The final url</returns>
        /// <exception cref="WebDriverTimeoutException"/>
        public string UrlStartsWith(string expectedUrlPrefix, TimeSpan timeout = default(TimeSpan))
        {
            return UrlSatisfiesCondition(url => url.StartsWith(expectedUrlPrefix));
        }

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Waits until the given element becomes stale
        /// </summary>
        /// <param name="element"></param>
        /// <param name="timeout">Maximal timeout for the operation</param>
        /// <exception cref="WebDriverTimeoutException"/>
        public static void ElementIsStale(IWebElement element, TimeSpan timeout = default(TimeSpan))
        {
            SetTimeout(ref timeout);
            ElementSatisfiesCondition(element, "Element didn't become stale", el =>
            {
                try
                {
                    el.GetAttribute("id");
                    return false;
                }
                catch (StaleElementReferenceException)
                {
                    return true;
                }
            });
        }

        /// <summary>
        /// Waits until an element is found using the given search contet and by objects
        /// </summary>
        /// <param name="searchContext">The search context used to search for the element</param>
        /// <param name="by">The By object used to search for the element</param>
        /// <param name="timeout">Maximal timeout for the operation</param>
        /// <returns>The newly found element</returns>
        /// <exception cref="WebDriverTimeoutException"/>
        public static IWebElement ElementExists(ISearchContext searchContext, By by,
            TimeSpan timeout = default(TimeSpan))
        {
            SetTimeout(ref timeout);
            var sw = Stopwatch.StartNew();
            bool isFirstSearch = true;
            IWebElement returnedElement;
            do
            {
                if (!isFirstSearch)
                    Thread.Sleep(PollingInterval);
                returnedElement = searchContext.FindElements(by).FirstOrDefault();
                if (isFirstSearch)
                    isFirstSearch = false;
            } while (returnedElement == null && sw.Elapsed < timeout);
            if (returnedElement == null)
            {
                throw new WebDriverTimeoutException(
                    $"Element coudln't be found using by {by} after {timeout.TotalSeconds}");
            }
            return returnedElement;
        }

        /// <summary>
        /// Waits until some elements are found using the given search contet and by objects
        /// </summary>
        /// <param name="searchContext">The search context used to search for the elements</param>
        /// <param name="by">The By object used to search for the elements</param>
        /// <param name="timeout">Maximal timeout for the operation</param>
        /// <returns>The newly found elements</returns>
        /// <exception cref="WebDriverTimeoutException"/>
        public static IReadOnlyList<IWebElement> ElementsExist(ISearchContext searchContext, By by,
            TimeSpan timeout = default(TimeSpan))
        {
            SetTimeout(ref timeout);
            var sw = Stopwatch.StartNew();
            bool isFirstSearch = true;
            IReadOnlyList<IWebElement> returnedElements;
            do
            {
                if (!isFirstSearch)
                    Thread.Sleep(PollingInterval);
                returnedElements = searchContext.FindElements(by);
                if (isFirstSearch)
                    isFirstSearch = false;
            } while (returnedElements.Count == 0 && sw.Elapsed < timeout);
            if (returnedElements.Count == 0)
            {
                throw new WebDriverTimeoutException(
                    $"Elements coudln't be found using by {by} after {timeout.TotalSeconds}");
            }
            return returnedElements;
        }

        /// <summary>
        /// Waits until no elements are found using the given search contet and by objects
        /// </summary>
        /// <param name="searchContext">The search context used to search for the elements</param>
        /// <param name="by">The By object used to search for the elements</param>
        /// <param name="timeout">Maximal timeout for the operation</param>
        /// <exception cref="WebDriverTimeoutException"/>
        public static void NoElementsExist(ISearchContext searchContext, By by, TimeSpan timeout = default(TimeSpan))
        {
            SetTimeout(ref timeout);
            var sw = Stopwatch.StartNew();
            bool isFirstSearch = true;
            IReadOnlyList<IWebElement> elements;
            do
            {
                if (!isFirstSearch)
                    Thread.Sleep(PollingInterval);
                elements = searchContext.FindElements(by);
                if (isFirstSearch)
                    isFirstSearch = false;
            } while (elements.Count > 0 && sw.Elapsed < timeout);
            if (elements.Count > 0)
            {
                throw new WebDriverTimeoutException(
                    $"Elements are still found using by {by} after {timeout.TotalSeconds}");
            }
        }

        /// <summary>
        /// Waits until the element's "Displayed" property is true
        /// </summary>
        /// <param name="element">The inspected element</param>
        /// <param name="timeout">Maximal timeout for the operation</param>
        /// <returns>The given element</returns>
        /// <exception cref="WebDriverTimeoutException"/>
        public static IWebElement ElementIsDisplayed(IWebElement element, TimeSpan timeout = default(TimeSpan))
        {
            return ElementSatisfiesCondition(element, "Element wasn't displayed", e => e.Displayed, timeout);
        }

        /// <summary>
        /// Waits until the element's "Displayed" property is false
        /// </summary>
        /// <param name="element">The inspected element</param>
        /// <param name="timeout">Maximal timeout for the operation</param>
        /// <returns>The given element</returns>
        /// <exception cref="WebDriverTimeoutException"/>
        public static IWebElement ElementIsUndisplayed(IWebElement element, TimeSpan timeout = default(TimeSpan))
        {
            return ElementSatisfiesCondition(element, "Element is still displayed", e => !e.Displayed, timeout);
        }

        /// <summary>
        /// Waits until the element's "Enabled" property is true
        /// </summary>
        /// <param name="element">The inspected element</param>
        /// <param name="timeout">Maximal timeout for the operation</param>
        /// <returns>The given element</returns>
        /// <exception cref="WebDriverTimeoutException"/>
        public static IWebElement ElementIsEnabled(IWebElement element, TimeSpan timeout = default(TimeSpan))
        {
            return ElementSatisfiesCondition(element, "Element wasn't enabled", e => e.Enabled, timeout);
        }

        /// <summary>
        /// Waits until the element's "Enabled" property is false
        /// </summary>
        /// <param name="element">The inspected element</param>
        /// <param name="timeout">Maximal timeout for the operation</param>
        /// <returns>The given element</returns>
        /// <exception cref="WebDriverTimeoutException"/>
        public static IWebElement ElementIsDisabled(IWebElement element, TimeSpan timeout = default(TimeSpan))
        {
            return ElementSatisfiesCondition(element, "Element is still enabled", e => !e.Enabled, timeout);
        }

        /// <summary>
        /// Waits until the element has the given class name
        /// </summary>
        /// <param name="element">The inspected element</param>
        /// <param name="className">The class name to search for in the element</param>
        /// <param name="timeout">Maximal timeout for the operation</param>
        /// <returns>The given element</returns>
        /// <exception cref="WebDriverTimeoutException"/>
        public static IWebElement ElementHasClass(IWebElement element, string className,
            TimeSpan timeout = default(TimeSpan))
        {
            return ElementSatisfiesCondition(element, "Element still does not have class " + className,
                e => e.GetClasses().Contains(className), timeout);
        }

        /// <summary>
        /// Waits until the element does not have the given class name
        /// </summary>
        /// <param name="element">The inspected element</param>
        /// <param name="className">The class name to search for in the element</param>
        /// <param name="timeout">Maximal timeout for the operation</param>
        /// <returns>The given element</returns>
        /// <exception cref="WebDriverTimeoutException"/>
        public static IWebElement ElementDoesNotHaveClass(IWebElement element, string className,
            TimeSpan timeout = default(TimeSpan))
        {
            return ElementSatisfiesCondition(element, "Element still has class " + className,
                e => !e.GetClasses().Contains(className), timeout);
        }

        /// <summary>
        /// Waits until the given element has a given expected value for a given CSS property
        /// </summary>
        /// <param name="element">The inspected element</param>
        /// <param name="cssProperty">The CSS property to test</param>
        /// <param name="expectedValue">The expected value of the given CSS property</param>
        /// <param name="timeout">Maximal timeout for the operation</param>
        /// <returns>The given element</returns>
        /// <exception cref="WebDriverTimeoutException"/>
        public static IWebElement ElementHasCSSValue(IWebElement element, string cssProperty, string expectedValue,
            TimeSpan timeout = default(TimeSpan))
        {
            return ElementSatisfiesCondition(element,
                $"Element's {cssProperty} CSS property still doesn't have value {expectedValue}",
                e => e.GetCssValue(cssProperty) == expectedValue, timeout);
        }

        /// <summary>
        /// Waits until the given element has the value "visible" in its "overflow" property (common use case with 
        /// elements that are expanded)
        /// </summary>
        /// <param name="element">The inspected element</param>
        /// <param name="timeout">Maximal timeout for the operation</param>
        /// <returns>The given element</returns>
        /// <exception cref="WebDriverTimeoutException"/>
        public static IWebElement ElementOverflowIsVisible(IWebElement element, TimeSpan timeout = default(TimeSpan))
        {
            return ElementHasCSSValue(element, "overflow", "visible", timeout);
        }

        /// <summary>
        /// Waits until the given element satisfies the given predicate
        /// </summary>
        /// <param name="element">The inspected element</param>
        /// <param name="predicate">The predicate to run against the element</param>
        /// <param name="timeout">Maximal timeout for the operation</param>
        /// <returns>The given element</returns>
        /// <exception cref="WebDriverTimeoutException"/>
        public static IWebElement ElementSatisfiesCondition(IWebElement element, Func<IWebElement, bool> predicate,
            TimeSpan timeout = default(TimeSpan))
        {
            return ElementSatisfiesCondition(element, "Element didn't satisfy the given predicate", predicate,
                timeout);
        }

        /// <summary>
        /// Waits until the given element's position starts changing
        /// </summary>
        /// <param name="element">The inspected element</param>
        /// <param name="timeout">Maximal timeout for the operation</param>
        /// <param name="pollingInterval">The interval between two consecutive inspections of the element's location. Default is 500 milliseconds</param>
        /// <returns>The given element</returns>
        /// <exception cref="WebDriverTimeoutException"/>
        public static IWebElement ElementStartsMoving(IWebElement element, TimeSpan timeout = default(TimeSpan),
            TimeSpan pollingInterval = default(TimeSpan))
        {
            return ElementSatisfiesPositionPredicate(element, (pos1, pos2) => !pos1.Equals(pos2), timeout,
                pollingInterval, (pos1, pos2) =>
                    $"Element is still for {timeout.TotalSeconds} seconds at position {pos2}");
        }

        /// <summary>
        /// Waits until the given element's position does not seem to change anymore
        /// </summary>
        /// <param name="element">The inspected element</param>
        /// <param name="timeout">Maximal timeout for the operation</param>
        /// <param name="pollingInterval">The interval between two consecutive inspections of the element's location. Default is 500 milliseconds</param>
        /// <returns>The given element</returns>
        /// <exception cref="WebDriverTimeoutException"/>
        public static IWebElement ElementStopsMoving(IWebElement element, TimeSpan timeout = default(TimeSpan),
            TimeSpan pollingInterval = default(TimeSpan))
        {
            return ElementSatisfiesPositionPredicate(element, (pos1, pos2) => pos1.Equals(pos2), timeout,
                pollingInterval, (pos1, pos2) =>
                    $"Element is still moving after {timeout.TotalSeconds} seconds. Latest position is {pos2}");
        }

        /// <summary>
        /// Waits until the given element's size starts changing
        /// </summary>
        /// <param name="element">The inspected element</param>
        /// <param name="timeout">Maximal timeout for the operation</param>
        /// <param name="pollingInterval">The interval between two consecutive inspections of the element's size. Default is 500 milliseconds</param>
        /// <returns>The given element</returns>
        /// <exception cref="WebDriverTimeoutException"/>
        public static IWebElement ElementStartsResizing(IWebElement element, TimeSpan timeout = default(TimeSpan),
            TimeSpan pollingInterval = default(TimeSpan))
        {
            return ElementSatisfiesSizePredicate(element, (size1, size2) => !size1.Equals(size2), timeout,
                pollingInterval, (size1, size2) =>
                    $"Element maintained size {size1} for {timeout.TotalSeconds} seconds");
        }

        /// <summary>
        /// Waits until the given element's size stops changing
        /// </summary>
        /// <param name="element">The inspected element</param>
        /// <param name="timeout">Maximal timeout for the operation</param>
        /// <param name="pollingInterval">The interval between two consecutive inspections of the element's size. Default is 500 milliseconds</param>
        /// <returns>The given element</returns>
        /// <exception cref="WebDriverTimeoutException"/>
        public static IWebElement ElementStopsResizing(IWebElement element, TimeSpan timeout = default(TimeSpan),
            TimeSpan pollingInterval = default(TimeSpan))
        {
            return ElementSatisfiesSizePredicate(element, (size1, size2) => size1.Equals(size2), timeout,
                pollingInterval, (size1, size2) =>
                    $"Element kept changing size for {timeout.TotalSeconds} seconds. Latest size is {size2}");
        }

        /// <summary>
        /// Waits until the element has non-zero width and length
        /// </summary>
        /// <param name="element">The inspected element</param>
        /// <param name="timeout">Maximal timeout for the operation</param>
        /// <returns>The given element</returns>
        public static IWebElement ElementHasSize(IWebElement element, TimeSpan timeout = default(TimeSpan))
        {
            return ElementSatisfiesCondition(element, "Element still has zero size", e =>
            {
                Size size = e.Size;
                return size.Height * size.Width != 0;
            }, timeout);
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

        private static IWebElement ElementSatisfiesChangingPropertyPredicate<T>(IWebElement element,
            Func<IWebElement, T> getProperty, Func<T, T, bool> propertyPredicate, TimeSpan timeout, 
            TimeSpan pollingInterval, Func<T, T, string> getFailureMessage)
        {
            SetTimeout(ref timeout);
            pollingInterval = pollingInterval == default(TimeSpan) ? TimeSpan.FromMilliseconds(500) : pollingInterval;
            T initialVal;
            T secondVal = getProperty(element);
            Stopwatch stopwatch = Stopwatch.StartNew();
            do
            {
                initialVal = secondVal;
                Thread.Sleep(pollingInterval);
                secondVal = getProperty(element);
            } while (!propertyPredicate(initialVal, secondVal) && stopwatch.Elapsed < timeout);
            if (!propertyPredicate(initialVal, secondVal))
            {
                throw new WebDriverTimeoutException(getFailureMessage(initialVal, secondVal));
            }
            return element;
        }

        private static IWebElement ElementSatisfiesPositionPredicate(IWebElement element,
            Func<Point, Point, bool> positionPredicate, TimeSpan timeout, TimeSpan pollingInterval, 
            Func<Point, Point, string> getFailureMessage)
        {
            return ElementSatisfiesChangingPropertyPredicate(element, e => e.Location, positionPredicate, timeout,
                pollingInterval, getFailureMessage);
        }

        private static IWebElement ElementSatisfiesSizePredicate(IWebElement element,
            Func<Size, Size, bool> positionPredicate, TimeSpan timeout, TimeSpan pollingInterval, 
            Func<Size, Size, string> getFailureMessage)
        {
            return ElementSatisfiesChangingPropertyPredicate(element, e => e.Size, positionPredicate, timeout,
                pollingInterval, getFailureMessage);
        }

        #endregion
    }
}