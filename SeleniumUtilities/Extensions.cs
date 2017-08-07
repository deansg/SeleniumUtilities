using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace SeleniumUtilities
{
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
        public static IReadOnlyList<string> Texts(this IList<IWebElement> elements)
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
        /// <param name="url"></param>
        /// <param name="https"></param>
        /// <returns></returns>
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
        /// <param name="driver"></param>
        /// <returns></returns>
        public static IWebElement GetFrameElement(this IWebDriver driver)
        {
            object frameElement = ((IJavaScriptExecutor)driver).ExecuteScript("return window.frameElement");
            return frameElement == null ? null : (IWebElement)frameElement;
        }

        #endregion
    }
}