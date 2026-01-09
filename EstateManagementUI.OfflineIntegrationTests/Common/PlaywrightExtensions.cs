using Microsoft.Playwright;
using Shouldly;
using System;
using System.Threading.Tasks;

namespace EstateManagementUI.OfflineIntegrationTests.Common
{
    public static class PlaywrightExtensions
    {
        public static async Task FillIn(this IPage page,
                                        String selector,
                                        String value,
                                        Boolean clearExistingText = false,
                                        TimeSpan? timeout = null)
        {
            await Retry.For(async () =>
            {
                var locator = page.Locator($"[name='{selector}']");
                await locator.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 30000 });
                
                if (clearExistingText)
                {
                    await locator.ClearAsync();
                }

                if (!String.IsNullOrEmpty(value))
                {
                    await locator.FillAsync(value);
                }
            }, timeout);
        }

        public static async Task FillInNumeric(this IPage page,
                                        String selector,
                                        String value,
                                        TimeSpan? timeout = null)
        {
            await Retry.For(async () =>
            {
                var locator = page.Locator($"[name='{selector}']");
                await locator.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 30000 });
                
                await locator.ClickAsync();
                await page.Keyboard.PressAsync("Control+A");
                await page.Keyboard.PressAsync("Delete");
                await locator.FillAsync(value);
            }, timeout);
        }

        public static async Task FillInById(this IPage page,
                                        String elementId,
                                        String value,
                                        Boolean clearExistingText = false,
                                        TimeSpan? timeout = null)
        {
            await Retry.For(async () =>
            {
                var locator = page.Locator($"#{elementId}");
                await locator.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 30000 });
                
                if (clearExistingText)
                {
                    await locator.ClearAsync();
                }
                
                if (!String.IsNullOrEmpty(value))
                {
                    await locator.FillAsync(value);
                }
            }, timeout);
        }

        public static async Task<ILocator> FindButtonById(this IPage page,
                                                         String buttonId,
                                                         TimeSpan? timeout = null)
        {
            ILocator locator = null;
            await Retry.For(async () =>
            {
                locator = page.Locator($"#{buttonId}");
                await locator.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 30000 });
                locator.ShouldNotBeNull();
            }, timeout);
            return locator;
        }

        public static async Task<ILocator> FindButtonByText(this IPage page,
                                                           String buttonText,
                                                           TimeSpan? timeout = null)
        {
            ILocator locator = null;
            await Retry.For(async () =>
            {
                locator = page.GetByRole(AriaRole.Button, new() { Name = buttonText });
                await locator.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 30000 });
                locator.ShouldNotBeNull();
            }, timeout);
            return locator;
        }

        public static async Task ClickButtonById(this IPage page, String buttonId)
        {
            var locator = await page.FindButtonById(buttonId);
            await locator.ClickAsync();
        }

        public static async Task ClickButtonByText(this IPage page, String buttonText)
        {
            var locator = await page.FindButtonByText(buttonText);
            await locator.ClickAsync();
        }

        public static async Task ClickLinkByText(this IPage page, String linkText, TimeSpan? timeout = null)
        {
            await Retry.For(async () =>
            {
                var locator = page.GetByRole(AriaRole.Link, new() { Name = linkText });
                await locator.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 30000 });
                await locator.ClickAsync();
            }, timeout);
        }

        public static async Task SelectDropDownByIdAndText(this IPage page,
                                                          String dropDownId,
                                                          String selectionText,
                                                          TimeSpan? timeout = null)
        {
            await Retry.For(async () =>
            {
                var locator = page.Locator($"#{dropDownId}");
                await locator.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 30000 });
                await locator.SelectOptionAsync(new[] { selectionText });
            }, timeout);
        }

        public static async Task SelectDropDownByIdAndValue(this IPage page,
                                                           String dropDownId,
                                                           String selectionValue,
                                                           TimeSpan? timeout = null)
        {
            await Retry.For(async () =>
            {
                var locator = page.Locator($"#{dropDownId}");
                await locator.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 30000 });
                await locator.SelectOptionAsync(new[] { selectionValue });
            }, timeout);
        }

        public static async Task SelectDropDownItemByText(this IPage page,
                                                          String dropDownName,
                                                          String selectionText,
                                                          TimeSpan? timeout = null)
        {
            await Retry.For(async () =>
            {
                var locator = page.Locator($"[name='{dropDownName}']");
                await locator.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 30000 });
                await locator.SelectOptionAsync(new SelectOptionValue { Label = selectionText });
            }, timeout);
        }

        public static async Task<String> GetDropDownItemText(this IPage page, String dropDownName, TimeSpan? timeout = null)
        {
            String selectedText = null;
            await Retry.For(async () =>
            {
                var locator = page.Locator($"[name='{dropDownName}']");
                await locator.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 30000 });
                var selectedValue = await locator.InputValueAsync();
                
                // Get the text of the selected option
                var selectedOption = locator.Locator($"option[value='{selectedValue}']");
                selectedText = await selectedOption.TextContentAsync();
            }, timeout);
            return selectedText;
        }

        public static async Task<String> GetTextById(this IPage page, String elementId, TimeSpan? timeout = null)
        {
            String text = null;
            await Retry.For(async () =>
            {
                var locator = page.Locator($"#{elementId}");
                await locator.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 30000 });
                text = await locator.TextContentAsync();
            }, timeout);
            return text;
        }

        public static async Task<String> GetValueById(this IPage page, String elementId, TimeSpan? timeout = null)
        {
            String value = null;
            await Retry.For(async () =>
            {
                var locator = page.Locator($"#{elementId}");
                await locator.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 30000 });
                value = await locator.InputValueAsync();
            }, timeout);
            return value;
        }

        public static async Task WaitForElement(this IPage page, String selector, TimeSpan? timeout = null)
        {
            await Retry.For(async () =>
            {
                var locator = page.Locator(selector);
                await locator.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 30000 });
            }, timeout);
        }

        public static async Task<bool> IsElementVisible(this IPage page, String selector)
        {
            try
            {
                var locator = page.Locator(selector);
                return await locator.IsVisibleAsync();
            }
            catch
            {
                return false;
            }
        }
    }
}
