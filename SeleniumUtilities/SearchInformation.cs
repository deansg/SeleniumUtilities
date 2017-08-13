using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace SeleniumUtilities
{
    /// <summary>
    /// Represents a logical combination of the two components required for retrieving IWebElements: an
    /// ISearchContext instance and a By instance. Can be used to more elegantly prevent code duplication when
    /// performing several types of searches using the same ISearchContext and By insntances
    /// </summary>
    public class SearchInformation
    {
        /// <summary>
        /// Creates a new instance of the SearchInformation class using the given ISearchContext and By instances
        /// </summary>
        /// <param name="searchContext"></param>
        /// <param name="by"></param>
        public SearchInformation(ISearchContext searchContext, By by)
        {
            SearchContext = searchContext;
            By = by;
        }

        /// <summary>
        /// The SearchContext part of the current SearchInformation instance
        /// </summary>
        public ISearchContext SearchContext { get; }

        /// <summary>
        /// The By part of the current SearchInformation instance
        /// </summary>
        public By By { get; }

        /// <summary>
        /// Finds the first element matching the criteria
        /// </summary>
        /// <returns></returns>
        public IWebElement FindElement()
        {
            return By.FindElement(SearchContext);
        }

        /// <summary>
        /// Finds all elements matching the criteria
        /// </summary>
        /// <returns></returns>
        public IReadOnlyList<IWebElement> FindElemenst()
        {
            return By.FindElements(SearchContext);
        }

        /// <summary>
        /// Waits until at least one element matching the criteria exists
        /// </summary>
        /// <param name="timeout">Maximal timeout for the entire operation</param>
        /// <returns></returns>
        public IWebElement WaitUntilElementExists(TimeSpan timeout = default(TimeSpan))
        {
            return WaitUntil.ElementExists(SearchContext, By, timeout);
        }

        /// <summary>
        /// Waits until no elements match the criteria
        /// </summary>
        /// <param name="timeout">Maximal timeout for the entire operation</param>
        public void WaitUntilNoElementsExist(TimeSpan timeout = default(TimeSpan))
        {
            WaitUntil.NoElementsExist(SearchContext, By, timeout);
        }
    }
}