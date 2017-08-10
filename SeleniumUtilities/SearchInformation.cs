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

        public ISearchContext SearchContext { get; }

        public By By { get; }

        public IWebElement FindElement()
        {
            return By.FindElement(SearchContext);
        }

        public IReadOnlyList<IWebElement> FindElemenst()
        {
            return By.FindElements(SearchContext);
        }

        public IWebElement WaitUntilElementExists(TimeSpan timeout = default(TimeSpan))
        {
            return WaitUntil.ElementExists(SearchContext, By, timeout);
        }

        public void WaitUntilNoElementsExist(TimeSpan timeout = default(TimeSpan))
        {
            WaitUntil.NoElementsExist(SearchContext, By, timeout);
        }
    }
}