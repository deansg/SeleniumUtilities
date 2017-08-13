# SeleniumUtilities
Enhancements and utilities for users of Selenium Webdriver's C# API.

Written in 2014-15 for a web automation project. 
Most of the functionality is provided by the WaitUntil and Extensions classes. WaitUntil can help implement waiting operations when using
Selenium in a more natural and convenient way, and on the way save a lot of code on common waiting operations. Extensions provides other
useful extension methods for many types commonly used in Selenium, such as IWebElement and ISearchContext.

In large automation projects, I would usually have one class deriving from WebsiteWrapperBase, representing the main website we would like 
to automate. Then, other classes, that represents parts of the website (like a specific page or sub-window) would all derive from 
DriverUser, and get the IWebDriver from their parent via the constructor. Then, each class in the automation project would have convenient
access to WaitUntil's instance AND static methods simply by typing "WaitUntil.". Special operations that require the IWebDriver directly
from test code would use the instance deriving from WebsiteWrapperBase.
