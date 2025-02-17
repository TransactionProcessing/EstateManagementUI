using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Shared.IntegrationTesting;
using Shouldly;

namespace EstateManagementUI.IntegrationTests.Common
{
    public static class Extensions
    {
        public static async Task FillIn(this IWebDriver webDriver,
                                        String elementName,
                                        String value,
                                        Boolean clearExistingText = false,
                                        TimeSpan? timeout = null)
        {
            await Retry.For(async () =>
                            {
                                IWebElement webElement = webDriver.FindElement(By.Name(elementName));
                                webElement.ShouldNotBeNull();
                                webElement.Displayed.ShouldBe(true);
                                webElement.Enabled.ShouldBe(true);
                                if (clearExistingText)
                                {
                                    webElement.Clear();
                                }

                                if (String.IsNullOrEmpty(value) == false) {
                                    webElement.SendKeys(value);
                                }
                            }, timeout);
        }

        public static async Task FillInById(this IWebDriver webDriver,
                                        String elementId,
                                        String value,
                                        Boolean clearExistingText = false,
                                        TimeSpan? timeout = null)
        {
            await Retry.For(async () =>
            {
                IWebElement webElement = webDriver.FindElement(By.Id(elementId));
                webElement.ShouldNotBeNull();
                webElement.Displayed.ShouldBe(true);
                webElement.Enabled.ShouldBe(true);
                if (clearExistingText)
                {
                    webElement.Clear();
                }
                if (String.IsNullOrEmpty(value) == false)
                {
                    webElement.SendKeys(value);
                }
            }, timeout);
        }

        public static async Task<IWebElement> FindButtonById(this IWebDriver webDriver,
                                                             String buttonId,
                                                             TimeSpan? timeout = null)
        {
            IWebElement element = null;
            await Retry.For(async () =>
                            {
                                element = webDriver.FindElement(By.Id(buttonId));

                                element.ShouldNotBeNull();
                            }, timeout);
            return element;
        }

        public static async Task<IWebElement> FindButtonByText(this IWebDriver webDriver,
                                                               String buttonText,
                                                               TimeSpan? timeout = null)
        {
            IWebElement element = null;
            await Retry.For(async () =>
                            {
                                ReadOnlyCollection<IWebElement> elements = webDriver.FindElements(By.TagName("button"));

                                List<IWebElement> e = elements.Where(e => e.GetAttribute("innerText") == buttonText).ToList();

                                e.ShouldHaveSingleItem();

                                element = e.Single();
                            },timeout);
            return element;
        }

        public static void ClickLink(this IWebDriver webDriver,
                                     String linkText)
        {
            IWebElement webElement = webDriver.FindElement(By.LinkText(linkText));
            webElement.ShouldNotBeNull();
            webElement.Click();
        }

        public static async Task ClickCheckBox(this IWebDriver webDriver,
                                               String elementName,
                                               TimeSpan? timeout = null) {
            await Retry.For(async () => {
                IWebElement webElement = webDriver.FindElement(By.Name(elementName));
                webElement.ShouldNotBeNull();
                webElement.Displayed.ShouldBe(true);
                webElement.Enabled.ShouldBe(true);
                webElement.Click();
            }, timeout);
        }

        public static async Task ClickCheckBoxById(this IWebDriver webDriver,
                                               String elementId,
                                               TimeSpan? timeout = null)
        {
            await Retry.For(async () => {
                IWebElement webElement = webDriver.FindElement(By.Id(elementId));
                webElement.ShouldNotBeNull();
                webElement.Displayed.ShouldBe(true);
                webElement.Enabled.ShouldBe(true);
                webElement.Click();
            }, timeout);
        }

        public static async Task<Boolean> IsCheckboxChecked(this IWebDriver webDriver,
                                                   String elementId,
                                                   TimeSpan? timeout = null) {
            Boolean isChecked = false;
            await Retry.For(async () => {
                IWebElement webElement = webDriver.FindElement(By.Id(elementId));
                webElement.ShouldNotBeNull();
                webElement.Displayed.ShouldBe(true);
                webElement.Enabled.ShouldBe(true);
                isChecked = webElement.Selected;
            }, timeout);
            return isChecked;
        }

        public static async Task ClickButtonById(this IWebDriver webDriver,
                                                 String buttonId) {
            await Retry.For(async () => {
                IWebElement webElement = await webDriver.FindButtonById(buttonId);
                webElement.ShouldNotBeNull();
                webElement.Displayed.ShouldBe(true);
                webElement.Enabled.ShouldBe(true);
                webElement.Click();
            });
        }

        public static async Task ClickButtonByText(this IWebDriver webDriver,
                                             String buttonText)
        {
            IWebElement webElement = await webDriver.FindButtonByText(buttonText);
            webElement.ShouldNotBeNull();
            webElement.Click();
        }

        public static async Task SelectDropDownItemByText(this IWebDriver webDriver, String dropdownId, String textToSelect,
                                                          TimeSpan? timeout = null)
        {
            IWebElement element = null;
            await Retry.For(async () =>
                            {
                                element = webDriver.FindElement(By.Id(dropdownId));

                                element.ShouldNotBeNull();

                                SelectElement dropdown = new SelectElement(element);
                                dropdown.SelectByText(textToSelect);
                            }, timeout);
        }

        public static async Task<String> GetDropDownItemText(this IWebDriver webDriver, String dropdownId,
                                                             TimeSpan? timeout = null)
        {
            String selectedText = null;
            await Retry.For(async () =>
                            {
                                IWebElement element = webDriver.FindElement(By.Id(dropdownId));

                                element.ShouldNotBeNull();

                                SelectElement dropdown = new SelectElement(element);
                                selectedText = dropdown.SelectedOption.Text;
                            },timeout);

            return selectedText;
        }
    }
}