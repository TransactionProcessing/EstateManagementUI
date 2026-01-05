# Migration Guide: Selenium to Playwright

This guide helps developers understand the differences between the legacy Selenium-based tests and the new Playwright-based tests for the Blazor UI.

## Overview

| Aspect | Legacy Tests (Selenium) | New Tests (Playwright) |
|--------|------------------------|------------------------|
| **Target UI** | EstateManagementUI (ASP.NET Core) | EstateManagementUI.BlazorServer (Blazor Server) |
| **Test Framework** | Selenium WebDriver | Microsoft Playwright |
| **Project** | EstateManagementUI.IntegrationTests | EstateManagementUI.BlazorIntegrationTests |
| **Browser API** | Synchronous (with some async) | Fully Async |
| **Main Interface** | `IWebDriver` | `IPage` |

## Key Differences

### 1. Browser API Objects

**Selenium:**
```csharp
IWebDriver webDriver;
IWebElement element = webDriver.FindElement(By.Id("myElement"));
```

**Playwright:**
```csharp
IPage page;
ILocator locator = page.Locator("#myElement");
```

### 2. Async/Await

Playwright is fully asynchronous, requiring `await` for all interactions.

**Selenium:**
```csharp
public void ClickButton()
{
    var button = webDriver.FindElement(By.Id("submitButton"));
    button.Click();
}
```

**Playwright:**
```csharp
public async Task ClickButton()
{
    var button = page.Locator("#submitButton");
    await button.ClickAsync();
}
```

### 3. Element Locators

**Selenium:**
```csharp
// By ID
var element = webDriver.FindElement(By.Id("myId"));

// By Name
var element = webDriver.FindElement(By.Name("myName"));

// By CSS Selector
var element = webDriver.FindElement(By.CssSelector(".myClass"));

// By XPath
var element = webDriver.FindElement(By.XPath("//div[@id='myId']"));
```

**Playwright:**
```csharp
// By ID
var locator = page.Locator("#myId");

// By Name
var locator = page.Locator("[name='myName']");

// By CSS Selector
var locator = page.Locator(".myClass");

// By Role (Recommended for accessibility)
var button = page.GetByRole(AriaRole.Button, new() { Name = "Submit" });
var link = page.GetByRole(AriaRole.Link, new() { Name = "Home" });
```

### 4. Form Interactions

**Selenium:**
```csharp
// Fill text input
element.SendKeys("Hello World");

// Clear and fill
element.Clear();
element.SendKeys("New Value");

// Select dropdown
var select = new SelectElement(element);
select.SelectByText("Option 1");
```

**Playwright:**
```csharp
// Fill text input
await locator.FillAsync("Hello World");

// Clear and fill (FillAsync clears automatically)
await locator.FillAsync("New Value");

// Select dropdown
await locator.SelectOptionAsync(new SelectOptionValue { Label = "Option 1" });
// or by value:
await locator.SelectOptionAsync("optionValue");
```

### 5. Getting Element Properties

**Selenium:**
```csharp
String text = element.Text;
String value = element.GetDomProperty("value");
Boolean isDisplayed = element.Displayed;
Boolean isEnabled = element.Enabled;
```

**Playwright:**
```csharp
String text = await locator.TextContentAsync();
String value = await locator.InputValueAsync();
Boolean isVisible = await locator.IsVisibleAsync();
Boolean isEnabled = await locator.IsEnabledAsync();
```

### 6. Waiting for Elements

**Selenium:**
```csharp
WebDriverWait wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(10));
IWebElement element = wait.Until(driver => 
    driver.FindElement(By.Id("myElement"))
);
```

**Playwright:**
```csharp
// Auto-waiting built-in for most actions
await page.Locator("#myElement").ClickAsync(); // Waits automatically

// Explicit wait
await page.Locator("#myElement").WaitForAsync(new LocatorWaitForOptions 
{ 
    State = WaitForSelectorState.Visible,
    Timeout = 10000 
});
```

### 7. Navigation

**Selenium:**
```csharp
webDriver.Navigate().GoToUrl("https://example.com");
```

**Playwright:**
```csharp
await page.GotoAsync("https://example.com");
```

### 8. Page Title

**Selenium:**
```csharp
String title = webDriver.Title;
```

**Playwright:**
```csharp
String title = await page.TitleAsync();
```

### 9. Screenshots

**Selenium:**
```csharp
var screenshot = ((ITakesScreenshot)webDriver).GetScreenshot();
screenshot.SaveAsFile("screenshot.png");
```

**Playwright:**
```csharp
await page.ScreenshotAsync(new PageScreenshotOptions 
{ 
    Path = "screenshot.png",
    FullPage = true 
});
```

### 10. Browser Setup

**Selenium (Hooks.cs):**
```csharp
ChromeOptions options = new();
options.AddArgument("--headless");
options.AcceptInsecureCertificates = true;
webDriver = new ChromeDriver(options);
```

**Playwright (Hooks.cs):**
```csharp
// BeforeTestRun - Install browsers
playwright = await Playwright.CreateAsync();

// BeforeScenario - Create browser
browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
{
    Headless = true,
    Args = new[] { "--ignore-certificate-errors" }
});

var context = await browser.NewContextAsync(new BrowserNewContextOptions
{
    IgnoreHTTPSErrors = true,
    ViewportSize = new ViewportSize { Width = 1920, Height = 1080 }
});

var page = await context.NewPageAsync();
```

## Extension Methods Comparison

### Legacy (Selenium Extensions)

```csharp
// EstateManagementUI.IntegrationTests/Common/Extensions.cs
await webDriver.FillIn("fieldName", "value");
await webDriver.ClickButtonById("buttonId");
IWebElement button = await webDriver.FindButtonByText("Login");
```

### New (Playwright Extensions)

```csharp
// EstateManagementUI.BlazorIntegrationTests/Common/PlaywrightExtensions.cs
await page.FillIn("fieldName", "value");
await page.ClickButtonById("buttonId");
ILocator button = await page.FindButtonByText("Login");
```

## UI Helpers Comparison

### Legacy

```csharp
// EstateManagementUI.IntegrationTests/Common/EstateManagementUiSteps.cs
public class EstateManagementUiHelpers
{
    private readonly IWebDriver WebDriver;
    
    public void NavigateToHomePage()
    {
        this.WebDriver.Navigate().GoToUrl($"https://localhost:{port}");
        this.WebDriver.Title.ShouldBe("Welcome - Estate Management");
    }
}
```

### New

```csharp
// EstateManagementUI.BlazorIntegrationTests/Common/BlazorUiHelpers.cs
public class BlazorUiHelpers
{
    private readonly IPage Page;
    
    public async Task NavigateToHomePage()
    {
        await this.Page.GotoAsync($"https://localhost:{port}");
        var title = await this.Page.TitleAsync();
        title.ShouldBe("Welcome - Estate Management");
    }
}
```

## Step Definitions Comparison

### Legacy

```csharp
// EstateManagementUI.IntegrationTests/Steps/Gimp.cs
public EstateManagementUiSteps(ScenarioContext scenarioContext, ...)
{
    var webDriver = scenarioContext.ScenarioContainer.Resolve<IWebDriver>(...);
    this.UiHelpers = new EstateManagementUiHelpers(webDriver, ...);
}

[Given(@"I am on the application home page")]
public void GivenIAmOnTheApplicationHomePage()
{
    this.UiHelpers.NavigateToHomePage();
}
```

### New

```csharp
// EstateManagementUI.BlazorIntegrationTests/Steps/BlazorUiSteps.cs
public BlazorUiSteps(ScenarioContext scenarioContext, ...)
{
    var page = scenarioContext.ScenarioContainer.Resolve<IPage>(...);
    this.UiHelpers = new BlazorUiHelpers(page, ...);
}

[Given(@"I am on the application home page")]
public async Task GivenIAmOnTheApplicationHomePage()
{
    await this.UiHelpers.NavigateToHomePage();
}
```

## Docker Configuration Differences

The main difference is in environment variable names for authentication:

### Legacy UI (EstateManagementUI)
```csharp
environmentVariables.Add("AppSettings:Authority", ...);
environmentVariables.Add("AppSettings:ClientId", "estateUIClient");
environmentVariables.Add("AppSettings:ClientSecret", "Secret1");
```

### Blazor UI (EstateManagementUI.BlazorServer)
```csharp
environmentVariables.Add("Authentication:Authority", ...);
environmentVariables.Add("Authentication:ClientId", "estateUIClient");
environmentVariables.Add("Authentication:ClientSecret", "Secret1");
```

## Best Practices with Playwright

1. **Use role-based selectors** when possible for better accessibility and stability:
   ```csharp
   await page.GetByRole(AriaRole.Button, new() { Name = "Submit" }).ClickAsync();
   ```

2. **Auto-waiting**: Playwright automatically waits for elements to be actionable. No need for explicit waits in most cases.

3. **Locators are lazy**: They don't query the DOM until an action is performed.

4. **Always use await**: All Playwright methods are async and must be awaited.

5. **Screenshots on failure**: Already implemented in `Hooks.AfterScenario`.

6. **Browser context isolation**: Each test gets its own browser context, ensuring test isolation.

## Running Tests

### Legacy Tests
```bash
cd EstateManagementUI.IntegrationTests
dotnet test
```

### New Tests
```bash
cd EstateManagementUI.BlazorIntegrationTests
# First time only: Install Playwright browsers
pwsh bin/Debug/net10.0/playwright.ps1 install

dotnet test
```

## Troubleshooting

### Playwright Installation Issues

If you get errors about missing browsers:
```bash
# Install browsers manually
dotnet tool install --global Microsoft.Playwright.CLI
playwright install
```

### Timeout Issues

Playwright has a default timeout of 30 seconds. If you need longer:
```csharp
await page.Locator("#slowElement").WaitForAsync(new LocatorWaitForOptions 
{ 
    Timeout = 60000 // 60 seconds
});
```

## Additional Resources

- [Playwright for .NET Documentation](https://playwright.dev/dotnet/docs/intro)
- [Playwright API Reference](https://playwright.dev/dotnet/docs/api/class-playwright)
- [Selenium to Playwright Migration Guide](https://playwright.dev/docs/selenium-grid)
