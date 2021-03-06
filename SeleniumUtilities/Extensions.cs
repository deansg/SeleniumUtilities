﻿using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace SeleniumUtilities
{
    /// <summary>
    /// Contains several extension methods particularly useful with Selenium WebDriver
    /// </summary>
    public static class Extensions
    {
        #region Public Methods

        /// <summary>
        /// Creates an array of objects of type TOutput using the given selector. This is more efficient than a 
        /// ToArray/ToList function since it counts on the listCollection's count. In addition, it uses the 
        /// listCollection's indexer to guarantee that the new array element order is as intended.
        /// </summary>
        /// <typeparam name="TOriginal"></typeparam>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="listCollection"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        [Pure]
        public static TOutput[] SelectArray<TOriginal, TOutput>(this IReadOnlyList<TOriginal> listCollection,
            Func<TOriginal, TOutput> selector)
        {
            var returnedArr = new TOutput[listCollection.Count];
            for (int i = 0; i < listCollection.Count; i++)
            {
                returnedArr[i] = selector(listCollection[i]);
            }
            return returnedArr;
        }

        /// <summary>
        /// Creates an array of objects of type TOutput using the given selector. This is more efficient than a 
        /// ToArray/ToList function since it counts on the listCollection's count. In addition, it uses the 
        /// listCollection's indexer to guarantee that the new array element order is as intended.
        /// </summary>
        /// <typeparam name="TOriginal"></typeparam>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="listCollection"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        [Pure]
        public static TOutput[] SelectArray<TOriginal, TOutput>(this IReadOnlyList<TOriginal> listCollection,
            Func<TOriginal, int, TOutput> selector)
        {
            var returnedArr = new TOutput[listCollection.Count];
            for (int i = 0; i < listCollection.Count; i++)
            {
                returnedArr[i] = selector(listCollection[i], i);
            }
            return returnedArr;
        }

        /// <summary>
        /// Creates an array of objects of type TOutput using the given selector. This is more efficient than a 
        /// ToArray/ToList function since it counts on the listCollection's count. In addition, it uses the 
        /// listCollection's indexer to guarantee that the new array element order is as intended.
        /// </summary>
        /// <typeparam name="TOriginal"></typeparam>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="listCollection"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        [Pure]
        public static TOutput[] SelectArrayList<TOriginal, TOutput>(this IList<TOriginal> listCollection,
            Func<TOriginal, TOutput> selector)
        {
            // Objects implementing IList should also be implementing IReadOnlyList
            return ((IReadOnlyList<TOriginal>)listCollection).SelectArray(selector);
        }

        /// <summary>
        /// Creates an array of objects of type TOutput using the given selector. This is more efficient than a 
        /// ToArray/ToList function since it counts on the listCollection's count. In addition, it uses the 
        /// listCollection's indexer to guarantee that the new array element order is as intended.
        /// </summary>
        /// <typeparam name="TOriginal"></typeparam>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="listCollection"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        [Pure]
        public static TOutput[] SelectArrayList<TOriginal, TOutput>(this IList<TOriginal> listCollection,
            Func<TOriginal, int, TOutput> selector)
        {
            // Objects implementing IList should also be implementing IReadOnlyList
            return ((IReadOnlyList<TOriginal>)listCollection).SelectArray(selector);
        }

        /// <summary>
        /// Creates an array of objects of type TOutput using the given selector in parallel. This is more efficient 
        /// than a ToArray/ToList function since it counts on the listCollection's count, and also utilizes the array's
        /// memoery to apply the selector in parallel. In addition, it uses the listCollection's indexer to guarantee 
        /// that the new array element order is as intended.
        /// </summary>
        /// <typeparam name="TOriginal"></typeparam>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="listCollection"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        [Pure]
        public static TOutput[] SelectArrayParallel<TOriginal, TOutput>(this IReadOnlyList<TOriginal> listCollection,
            Func<TOriginal, TOutput> selector)
        {
            var returnedArr = new TOutput[listCollection.Count];
            Parallel.For(0, listCollection.Count, i => returnedArr[i] = selector(listCollection[i]));
            return returnedArr;
        }

        /// <summary>
        /// Creates an array of objects of type TOutput using the given selector in parallel. This is more efficient 
        /// than a ToArray/ToList function since it counts on the listCollection's count, and also utilizes the array's
        /// memoery to apply the selector in parallel. In addition, it uses the listCollection's indexer to guarantee 
        /// that the new array element order is as intended.
        /// </summary>
        /// <typeparam name="TOriginal"></typeparam>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="listCollection"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        [Pure]
        public static TOutput[] SelectArrayParallel<TOriginal, TOutput>(this IReadOnlyList<TOriginal> listCollection,
            Func<TOriginal, int, TOutput> selector)
        {
            var returnedArr = new TOutput[listCollection.Count];
            Parallel.For(0, listCollection.Count, i => returnedArr[i] = selector(listCollection[i], i));
            return returnedArr;
        }

        /// <summary>
        /// Creates an array of objects of type TOutput using the given selector in parallel. This is more efficient 
        /// than a ToArray/ToList function since it counts on the listCollection's count, and also utilizes the array's
        /// memoery to apply the selector in parallel. In addition, it uses the listCollection's indexer to guarantee 
        /// that the new array element order is as intended.
        /// </summary>
        /// <typeparam name="TOriginal"></typeparam>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="listCollection"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        [Pure]
        public static TOutput[] SelectArrayParallelList<TOriginal, TOutput>(this IList<TOriginal> 
            listCollection, Func<TOriginal, TOutput> selector)
        {
            var returnedArr = new TOutput[listCollection.Count];
            Parallel.For(0, listCollection.Count, i => returnedArr[i] = selector(listCollection[i]));
            return returnedArr;
        }

        /// <summary>
        /// Creates an array of objects of type TOutput using the given selector in parallel. This is more efficient 
        /// than a ToArray/ToList function since it counts on the listCollection's count, and also utilizes the array's
        /// memoery to apply the selector in parallel. In addition, it uses the listCollection's indexer to guarantee 
        /// that the new array element order is as intended.
        /// </summary>
        /// <typeparam name="TOriginal"></typeparam>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="listCollection"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        [Pure]
        public static TOutput[] SelectArrayParallelList<TOriginal, TOutput>(this IList<TOriginal>
            listCollection, Func<TOriginal, int, TOutput> selector)
        {
            var returnedArr = new TOutput[listCollection.Count];
            Parallel.For(0, listCollection.Count, i => returnedArr[i] = selector(listCollection[i], i));
            return returnedArr;
        }

        /// <summary>
        /// Returns a collection of texts of IWebElements using the SelectArray functions
        /// </summary>
        /// <param name="elements"></param>
        /// <returns></returns>
        [Pure]
        public static IReadOnlyList<string> Texts(this IReadOnlyList<IWebElement> elements)
        {
            return elements.SelectArray(element => element.Text);
        }

        /// <summary>
        /// Returns a collection of texts of IWebElements using the SelectArray functions
        /// </summary>
        /// <param name="elements"></param>
        /// <returns></returns>
        [Pure]
        public static IReadOnlyList<string> TextsList(this IList<IWebElement> elements)
        {
            return elements.SelectArrayList(element => element.Text);
        }

        /// <summary>
        /// Returns a collection of texts of IWebElements using the SelectArrayParallel function
        /// </summary>
        /// <param name="elements"></param>
        /// <returns></returns>
        [Pure]
        public static IReadOnlyList<string> TextsParallel(this IReadOnlyList<IWebElement> elements)
        {
            return elements.SelectArrayParallel(element => element.Text);
        }

        /// <summary>
        /// Returns a collection of texts of IWebElements using the SelectArrayParallelList function
        /// </summary>
        /// <param name="elements"></param>
        /// <returns></returns>
        [Pure]
        public static IReadOnlyList<string> TextsParallel(this IList<IWebElement> elements)
        {
            return elements.SelectArrayParallelList(element => element.Text);
        }

        /// <summary>
        /// Returns the proper and displayed version of the given url, with an http(s) beginning and a slash ending
        /// </summary>
        /// <param name="url">The url to expand</param>
        /// <param name="https">Whether the url should start with http or https</param>
        /// <returns>The final expanded version of the url</returns>
        [Pure]
        public static string NormalizeUrl(this string url, bool https = false)
        {
            var returnedUrl = url;
            if (!url.StartsWith("http"))
            {
                returnedUrl = (https ? "https://" : "http://") + returnedUrl;
            }
            if (!url.EndsWith("/"))
            {
                returnedUrl += "/";
            }
            return returnedUrl;
        }

        /// <summary>
        /// Returns the frame element containing the current window, or null if the current window is top-level
        /// </summary>
        /// <param name="driver">The driver representing the current window</param>
        /// <returns></returns>
        public static IWebElement GetFrameElement(this IWebDriver driver)
        {
            object frameElement = ((IJavaScriptExecutor)driver).ExecuteScript("return window.frameElement");
            return frameElement == null ? null : (IWebElement)frameElement;
        }

        /// <summary>
        /// Returns the classes of the current element
        /// </summary>
        /// <param name="element"></param>
        /// <returns>The classes of the current element</returns>
        public static string[] GetClasses(this IWebElement element)
        {
            return element.GetAttribute("class").Split(' ');
        }

        /// <summary>
        /// Finds the first OpenQa.Selenium.IWebElement using the given method, or returns null if none are found
        /// </summary>
        /// <param name="searchContext">Search context used to search for the IWebElement</param>
        /// <param name="by">The locating mechanism to use</param>
        /// <returns>The first IWebElement found or null if none are found</returns>
        public static IWebElement FindFirstOrDefault(this ISearchContext searchContext, By by)
        {
            IReadOnlyList<IWebElement> elements = searchContext.FindElements(by);
            return elements.Count > 0 ? elements[0] : null;
        }

        /// <summary>
        /// Finds the only OpenQA.Selenium.IWebElement using the given method or throws an InvalidOperationsException 
        /// if there isn't exactly one element
        /// </summary>
        /// <param name="searchContext">Search context used to search for the IWebElement</param>
        /// <param name="by">The locating mechanism to use</param>
        /// <returns>The only element found</returns>
        /// <exception cref="InvalidOperationException">In case more than one element was found</exception>
        /// <exception cref="NoSuchElementException">In case no elements were found</exception>
        public static IWebElement FindSingle(this ISearchContext searchContext, By by)
        {
            IReadOnlyList<IWebElement> elements = searchContext.FindElements(by);
            if (elements.Count == 0)
            {
                throw new NoSuchElementException("No elements were found using " + by);
            }
            AssertUpToOneElementWasFound(elements, by);
            return elements[0];
        }

        /// <summary>
        /// Finds the only OpenQA.Selenium.IWebElement using the given method or returns null if there are none or
        /// throws an InvalidOperationsException if there is more than one element
        /// </summary>
        /// <param name="searchContext">Search context used to search for the IWebElement</param>
        /// <param name="by">The locating mechanism to use</param>
        /// <returns>The only element found</returns>
        /// <exception cref="InvalidOperationException">In case more than one element was found</exception>
        public static IWebElement FindSingleOrDefault(this ISearchContext searchContext, By by)
        {
            IReadOnlyList<IWebElement> elements = searchContext.FindElements(by);
            AssertUpToOneElementWasFound(elements, by);
            return elements.Count > 0 ? elements[0] : null;
        }

        #endregion

        #region Private Methods

        private static void AssertUpToOneElementWasFound(IReadOnlyList<IWebElement> elements, By by)
        {
            if (elements.Count > 1)
            {
                throw new InvalidOperationException("More than one element was found using " + by);
            }
        }

        #endregion
    }
}