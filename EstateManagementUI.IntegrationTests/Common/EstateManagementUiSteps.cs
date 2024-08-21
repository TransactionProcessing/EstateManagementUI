using System.Collections.ObjectModel;
using System.Text;
using EstateManagementUI.IntegrationTests.Steps;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using Shared.IntegrationTesting;
using Shouldly;

namespace EstateManagementUI.IntegrationTests.Common;

public class EstateManagementUiHelpers{
    private readonly IWebDriver WebDriver;

    private readonly Int32 EstateManagementUiPort;

    public EstateManagementUiHelpers(IWebDriver webDriver, Int32 estateManagementUiPort){
        this.WebDriver = webDriver;
        this.EstateManagementUiPort = estateManagementUiPort;
    }

    private void VerifyPageTitle(String expectedTitle) {
        this.WebDriver.Title.ShouldBe($"{expectedTitle} - Estate Management");
    }

    public void NavigateToHomePage(){
        this.WebDriver.Navigate().GoToUrl($"https://localhost:{this.EstateManagementUiPort}");
        this.VerifyPageTitle("Welcome");
    }

    public async Task ClickContractsSidebarOption(){
        await this.WebDriver.ClickButtonById("contractsLink");
    }

    public async Task ClickMyEstateSidebarOption(){
        await this.WebDriver.ClickButtonById("estateDetailsLink");
    }

    public async Task ClickMyMerchantsSidebarOption(){
        await this.WebDriver.ClickButtonById("merchantsLink");
    }

    public async Task ClickMyOperatorsSidebarOption(){
        await this.WebDriver.ClickButtonById("operatorsLink");
    }

    public async Task ClickOnTheSignInButton(){
        await this.WebDriver.ClickButtonById("loginButton");
    }

    public async Task VerifyOnTheMakeMerchantDepositScreen(){
        await Retry.For(async () => { this.WebDriver.Title.ShouldBe("Make Merchant Deposit"); });
    }

    public async Task VerifyOnTheTheMerchantDetailsScreen(String merchantName){

        await Retry.For(async () => {
                            IWebElement element = this.WebDriver.FindElement(By.Id("MerchantName"));
                            element.ShouldNotBeNull();
                            String elementValue = element.GetDomProperty("value");
                            elementValue.ShouldBe(merchantName);
                        });
    }

    public async Task VerifyOnTheNewContractScreen(){
        await Retry.For(async () => { this.WebDriver.Title.ShouldBe("New Contract Details"); });
    }

    public async Task VerifyOnTheNewMerchantScreen(){
        await Retry.For(async () => { this.WebDriver.Title.ShouldBe("New Merchant Details"); });
    }

    public async Task VerifyOnTheOperatorDialog(){
        await Retry.For(async () => {
            IWebElement element = this.WebDriver.FindElement(By.Id("OperatorDialog"));
            element.ShouldNotBeNull();
        });
    }

    public async Task VerifyOnTheContractsListScreen(){
        await Retry.For(async () => { this.VerifyPageTitle("View Contracts"); });
    }

    public async Task VerifyOnTheNewProductScreen()
    {
        await Retry.For(async () => { this.WebDriver.Title.ShouldBe("New Contract Product Details"); });
    }

    public async Task VerifyOnTheNewTransactionFeeScreen()
    {
        await Retry.For(async () => { this.WebDriver.Title.ShouldBe("New Transaction Fee Details"); });
    }

    public async Task VerifyOnTheLoginScreen(){
        await Retry.For(async () => {
                            IWebElement loginButton = await this.WebDriver.FindButtonByText("Login", TimeSpan.FromMinutes(2));
                            loginButton.ShouldNotBeNull();
                        });
    }

    public async Task VerifyOnTheDashboard()
    {
        await Retry.For(async () => {
            //if (this.WebDriver.Title != "Dashboard  - Estate Management")
            //{
            //    var screenshot = this.WebDriver.TakeScreenshot();
            //    var stringVersion = screenshot.AsBase64EncodedString;
            //    Console.WriteLine(stringVersion);
            //}

            await Retry.For(async () => { this.VerifyPageTitle("Dashboard"); });
        });
    }

    public async Task VerifyOnTheEstateDetailsScreen()
    {
        await Retry.For(async () => { this.VerifyPageTitle("View Estate"); });
    }

    public async Task VerifyOnTheMerchantsListScreen()
    {
        await Retry.For(async () => { this.VerifyPageTitle("View Merchants"); });
    }

    public async Task VerifyOnTheOperatorsListScreen(){
        await Retry.For(async () => { this.VerifyPageTitle("View Operators"); });
    }

    public async Task VerifyOnTheProductsListScreen()
    {
        await Retry.For(async () => { this.WebDriver.Title.ShouldContain("Products for Contract - "); });
    }

    public async Task VerifyOnTheTransactionFeeListScreen()
    {
        await Retry.For(async () => { this.WebDriver.Title.ShouldContain("Transaction Fees for Product - "); });
    }

    public async Task VerifyTheCorrectEstateDetailsAreDisplayed(String estateName, String estateReference){
        await Retry.For(async () => {
                            IWebElement element = this.WebDriver.FindElement(By.Id("Estate_Name"));
                            element.ShouldNotBeNull();
                            String elementValue = element.GetDomProperty("value");
                            elementValue.ShouldBe(estateName);

                            element = this.WebDriver.FindElement(By.Id("Estate_Reference"));
                            element.ShouldNotBeNull();
                            elementValue = element.GetDomProperty("value");
                            elementValue.ShouldBe(estateReference);
        });
    }

    public async Task VerifyTheAvailableBalanceIsDisplayed(Decimal availableBalance){
        await Retry.For(async () => {
                            IWebElement element = this.WebDriver.FindElement(By.Id("merchantAvailableBalanceLabel"));
                            element.ShouldNotBeNull();
                            String elementValue = element.GetDomProperty("value");
                            Decimal actualBalance = Decimal.Parse(elementValue);
                            actualBalance.ShouldBe(availableBalance);
                        },
                        TimeSpan.FromMinutes(2),
                        TimeSpan.FromSeconds(30));
    }

    public async Task VerifyTheContractDetailsAreInTheList(List<(String, String, Int32)> contractDescriptions){
        await Retry.For(async () => {
                            Int32 foundRowCount = 0;
                            IWebElement tableElement = this.WebDriver.FindElement(By.Id("contractList"));
                            IList<IWebElement> rows = tableElement.FindElements(By.TagName("tr"));

                            rows.Count.ShouldBe(contractDescriptions.Count + 1);
                            foreach ((String, String, Int32) contractDescription in contractDescriptions){
                                IList<IWebElement> rowTD;
                                foreach (IWebElement row in rows){
                                    ReadOnlyCollection<IWebElement> rowTH = row.FindElements(By.TagName("th"));

                                    if (rowTH.Any()){
                                        // header row so skip
                                        continue;
                                    }

                                    rowTD = row.FindElements(By.TagName("td"));

                                    if (rowTD[0].Text == contractDescription.Item1){
                                        // Compare other fields
                                        rowTD[0].Text.ShouldBe(contractDescription.Item1);
                                        rowTD[1].Text.ShouldBe(contractDescription.Item2);
                                        rowTD[2].Text.ShouldBe(contractDescription.Item3.ToString());

                        // We have found the row
                        foundRowCount++;
                                        break;
                                    }
                                }
                            }

                            foundRowCount.ShouldBe(contractDescriptions.Count);
                        },
                        TimeSpan.FromSeconds(120));
    }

    public async Task VerifyTheFeeDetailsAreInTheList(List<String> feeDescriptions)
    {
        await Retry.For(async () => {
                            Int32 foundRowCount = 0;
                            IWebElement tableElement = this.WebDriver.FindElement(By.Id("contractProductTransactionFeeList"));
                            IList<IWebElement> rows = tableElement.FindElements(By.TagName("tr"));

                            rows.Count.ShouldBe(feeDescriptions.Count + 1);
                            foreach (String feeDescription in feeDescriptions)
                            {
                                IList<IWebElement> rowTD;
                                foreach (IWebElement row in rows)
                                {
                                    ReadOnlyCollection<IWebElement> rowTH = row.FindElements(By.TagName("th"));

                                    if (rowTH.Any())
                                    {
                                        // header row so skip
                                        continue;
                                    }

                                    rowTD = row.FindElements(By.TagName("td"));

                                    if (rowTD[0].Text == feeDescription)
                                    {
                                        // Compare other fields
                                        rowTD[0].Text.ShouldBe(feeDescription);

                                        // We have found the row
                                        foundRowCount++;
                                        break;
                                    }
                                }
                            }

                            foundRowCount.ShouldBe(feeDescriptions.Count);
                        },
                        TimeSpan.FromSeconds(120));
    }

    public async Task VerifyMerchantDetailsAreInTheList(List<MerchantDetails> merchantDetails){
        await Retry.For(async () => {
                            Int32 foundRowCount = 0;
                            IWebElement tableElement = this.WebDriver.FindElement(By.Id("merchantList"));
                            IList<IWebElement> rows = tableElement.FindElements(By.TagName("tr"));

                            rows.Count.ShouldBe(merchantDetails.Count + 1);
                            foreach (MerchantDetails merchant in merchantDetails)
                            {
                                IList<IWebElement> rowTD;
                                foreach (IWebElement row in rows)
                                {
                                    ReadOnlyCollection<IWebElement> rowTH = row.FindElements(By.TagName("th"));

                                    if (rowTH.Any())
                                    {
                                        // header row so skip
                                        continue;
                                    }

                                    rowTD = row.FindElements(By.TagName("td"));

                                    if (rowTD[0].Text == merchant.MerchantName) {
                                        // Compare other fields
                                        rowTD[2].Text.ShouldBe(merchant.SettlementSchedule);
                                        rowTD[3].Text.ShouldBe(merchant.ContactName);
                                        rowTD[4].Text.ShouldBe(merchant.AddressLine1);
                                        rowTD[5].Text.ShouldBe(merchant.Town);

                                        // We have found the row
                                        foundRowCount++;
                                        break;
                                    }
                                }
                            }

                            foundRowCount.ShouldBe(merchantDetails.Count);
                        },
                        TimeSpan.FromSeconds(180));
    }

    public async Task VerifyOperatorDetailsAreInTheList(List<(String, String, String)> operatorDetails) {
        await Retry.For(async () => {
            Int32 foundRowCount = 0;
            IWebElement tableElement = this.WebDriver.FindElement(By.Id("operatorList"));
            IList<IWebElement> rows = tableElement.FindElements(By.TagName("tr"));

            rows.Count.ShouldBe(operatorDetails.Count + 1);
            StringBuilder sb = new StringBuilder();
            foreach ((String, String, String) operatorDetail in operatorDetails) {
                IList<IWebElement> rowTD;
                foreach (IWebElement row in rows) {
                    ReadOnlyCollection<IWebElement> rowTH = row.FindElements(By.TagName("th"));

                    if (rowTH.Any()) {
                        // header row so skip
                        continue;
                    }

                    rowTD = row.FindElements(By.TagName("td"));

                    if (rowTD[0].Text == operatorDetail.Item1) {
                        // Compare other fields
                        rowTD[0].Text.ShouldBe(operatorDetail.Item1);
                        rowTD[1].Text.ShouldBe(operatorDetail.Item2);
                        rowTD[2].Text.ShouldBe(operatorDetail.Item3);
                        // We have found the row
                        foundRowCount++;
                        sb.AppendLine($"Found {operatorDetail.Item1}");
                        break;
                    }
                    else {
                        sb.AppendLine($"Not Found {operatorDetail.Item1}");
                    }
                }
            }

            foundRowCount.ShouldBe(operatorDetails.Count, sb.ToString());
        }, TimeSpan.FromSeconds(120));
    }

    public async Task VerifyProductDetailsAreInTheList(List<String> productDetails)
    {
        await Retry.For(async () => {
                            Int32 foundRowCount = 0;
                            IWebElement tableElement = this.WebDriver.FindElement(By.Id("contractProductList"));
                            IList<IWebElement> rows = tableElement.FindElements(By.TagName("tr"));

                            rows.Count.ShouldBe(productDetails.Count + 1);
                            foreach (String productDetail in productDetails)
                            {
                                IList<IWebElement> rowTD;
                                foreach (IWebElement row in rows)
                                {
                                    ReadOnlyCollection<IWebElement> rowTH = row.FindElements(By.TagName("th"));

                                    if (rowTH.Any())
                                    {
                                        // header row so skip
                                        continue;
                                    }

                                    rowTD = row.FindElements(By.TagName("td"));

                                    if (rowTD[0].Text == productDetail)
                                    {
                                        // Compare other fields
                                        rowTD[0].Text.ShouldBe(productDetail);

                                        // We have found the row
                                        foundRowCount++;
                                        break;
                                    }
                                }
                            }

                            foundRowCount.ShouldBe(productDetails.Count);
                        },
                        TimeSpan.FromSeconds(120));
    }

    public async Task VerifyMerchantsSettlementSchedule(String settlementSchedule){
        String selectedText = await this.WebDriver.GetDropDownItemText("settlementScheduleList");
        selectedText.ShouldBe(settlementSchedule);
    }

    public async Task ClickAddNewContractButton(){
        await Retry.For(async () => { await this.WebDriver.ClickButtonById("newContractButton"); });
    }

    public async Task ClickAddNewMerchantButton()
    {
        await Retry.For(async () => { await this.WebDriver.ClickButtonById("newMerchantButton"); });
    }

    public async Task ClickAddNewProductButton()
    {
        await Retry.For(async () => { await this.WebDriver.ClickButtonById("newContractProductButton"); });
    }

    public async Task ClickAddNewTransactionFeeButton(){
        await Retry.For(async () => { await this.WebDriver.ClickButtonById("newContractProductTransactionFeeButton"); });
    }

    public async Task ClickTheCreateContractButton(){
        await this.WebDriver.ClickButtonById("createContractButton");
    }

    public async Task ClickTheCreateMerchantButton()
    {
        await this.WebDriver.ClickButtonById("createMerchantButton");
    }

    public async Task ClickTheNewOperatorButton()
    {
        await this.WebDriver.ClickButtonById("newOperatorButton");
    }

    public async Task ClickTheSaveNewOperatorButton()
    {
        await this.WebDriver.ClickButtonById("saveNewOperatorButton");
    }

    public async Task ClickTheCreateProductButton()
    {
        await this.WebDriver.ClickButtonById("createContractProductButton");
    }

    public async Task ClickTheCreateTransactionFeeButton()
    {
        await this.WebDriver.ClickButtonById("createTransactionFeeButton");
    }

    public async Task ClickTheProductsLinkForContract(String contractDescription){
        await this.ClickElementInTable("contractList", contractDescription, "numberOfProductsLink");
    }

    public async Task ClickTheTransactionFeesLinkForTheProduct(String productName)
    {
        await this.ClickElementInTable("contractProductList", productName, "numberOfTransactionFeesLink");
    }

    public async Task ClickTheMakeDepositButtonForTheMerchant(String merchantName)
    {
        await this.ClickElementInTable("merchantList", merchantName, "makeDepositLink");
    }

    public async Task EnterContractDetails(String contractDescription, String operatorName){
        await this.WebDriver.FillIn("contractDescription", contractDescription);
        await this.WebDriver.SelectDropDownItemByText("operatorList", operatorName);
    }

    public async Task EnterMerchantDetails(String merchantName, String addressLine1, String town, String region, String postCode, String country, String contactName, String contactEmail, String contactPhoneNumber, String settlementSchedule){
        await this.WebDriver.FillIn("merchantName", merchantName);
        await this.WebDriver.FillIn("addressLine1", addressLine1);
        await this.WebDriver.FillIn("town", town);
        await this.WebDriver.FillIn("region", region);
        await this.WebDriver.FillIn("postalCode", postCode);
        await this.WebDriver.FillIn("country", country);
        await this.WebDriver.FillIn("contactName", contactName);
        await this.WebDriver.FillIn("contactEmailAddress", contactEmail);
        await this.WebDriver.FillIn("contactPhoneNumber", contactPhoneNumber);
        await this.WebDriver.SelectDropDownItemByText("settlementScheduleList", settlementSchedule);
    }

    public async Task EnterOperatorDetails(String operatorName,
                                           Boolean requireCustomMerchantNumber,
                                           Boolean requireCustomTerminalNumber) {
        await this.WebDriver.FillInById("operatorName", operatorName, true);
        Boolean requireCustomMerchantNumberIsChecked = await this.WebDriver.IsCheckboxChecked("requireCustomMerchantNumber");
        Boolean clickRequireCustomMerchantNumberCheckBox = (requireCustomMerchantNumber, requireCustomMerchantNumberIsChecked) switch {
            (true, true) => false,
            (true, false) => true,
            (false, false) => false,
            _ => true
        };
        
        if (clickRequireCustomMerchantNumberCheckBox) {
            await this.WebDriver.ClickCheckBoxById("requireCustomMerchantNumber");
        }

        Boolean requireCustomTerminalNumberIsChecked = await this.WebDriver.IsCheckboxChecked("requireCustomTerminalNumber");
        Boolean clickRequireCustomTerminalNumberCheckBox = (requireCustomTerminalNumber, requireCustomTerminalNumberIsChecked) switch
        {
            (true, true) => false,
            (true, false) => true,
            (false, false) => false,
            _ => true
        };

        if (clickRequireCustomTerminalNumberCheckBox) {
            await this.WebDriver.ClickCheckBoxById("requireCustomTerminalNumber");
        }
    }

    

    public async Task EnterProductDetails(String productName, String displayText, String productValue, String productType){
        await this.WebDriver.FillIn("productName", productName);
        await this.WebDriver.FillIn("displayText", displayText);
        await this.WebDriver.SelectDropDownItemByText("productTypeList", productType);

        if (String.IsNullOrEmpty(productValue))
        {
            // Set the IsVariable flag
            await this.WebDriver.FillIn("isVariable", "true");
        }
        else
        {
            // Set the product value
            await this.WebDriver.FillIn("value", productValue);
        }
    }

    public async Task Login(String username, String password){
        await this.WebDriver.FillIn("Input.Username", username);
        await this.WebDriver.FillIn("Input.Password", password);
        await this.WebDriver.ClickButtonByText("Login");
    }

    public async Task MakeMerchantDeposit(DateTime depositDate, Decimal depositAmount, String depositReference){
        await this.WebDriver.FillIn("amount", depositAmount.ToString());
        await this.WebDriver.FillIn("depositdate", depositDate.Date.ToString("dd/MM/yyyy"));
        await this.WebDriver.FillIn("reference", depositReference);

        await this.WebDriver.ClickButtonById("makeMerchantDepositButton");
    }


    public async Task EnterTransactionFeeDetails(String description, String calculationType, String feeType, String feeValue)
    {
        await this.WebDriver.FillIn("feeDescription", description);
        await this.WebDriver.SelectDropDownItemByText("calculationTypeList", calculationType);
        await this.WebDriver.SelectDropDownItemByText("feeTypeList", feeType);
        await this.WebDriver.FillIn("value", feeValue, true);
    }

    public async Task ClickTheMerchantLinkForMerchant(String merchantName)
    {
        await this.ClickElementInTable("merchantList", merchantName, "editMerchantLink");
    }


    private async Task ClickElementInTable(String tableId,
                                               String textToSearchFor,
                                               String elementToClickId)
    {
        Boolean foundRow = false;
        IWebElement itemrow = null;
        await Retry.For(async () => {
            IWebElement tableElement = this.WebDriver.FindElement(By.Id(tableId));
            IList<IWebElement> rows = tableElement.FindElements(By.TagName("tr"));

            rows.ShouldNotBeNull();
            rows.Any().ShouldBeTrue();
            IList<IWebElement> rowTD;
            foreach (IWebElement row in rows)
            {
                ReadOnlyCollection<IWebElement> rowTH = row.FindElements(By.TagName("th"));

                if (rowTH.Any())
                {
                    // header row so skip
                    continue;
                }

                rowTD = row.FindElements(By.TagName("td"));

                if (rowTD[0].Text == textToSearchFor)
                {
                    itemrow = row;
                    foundRow = true;
                    break;
                }
            }


            foundRow.ShouldBeTrue();
            itemrow.ShouldNotBeNull();
        },
                        TimeSpan.FromSeconds(120)).ConfigureAwait(false);


        await Retry.For(async () => {
            IWebElement element = itemrow.FindElement(By.Id(elementToClickId));
            if (element.Displayed)
            {
                element.Click();
            }
            else
            {
                this.WebDriver.ExecuteJavaScript($"document.getElementById('{elementToClickId}').click();");
            }
        },
                        TimeSpan.FromSeconds(120));
    }

    public async Task ClickTheEditOperatorButton(String operatorName) {
        IWebElement tableElement = this.WebDriver.FindElement(By.Id("operatorList"));
        var dropdownMenuButton = tableElement.FindElement(By.Id("dropdownMenuButton"));
        dropdownMenuButton.Click();
        IWebElement editButton = this.WebDriver.FindElement(By.Id($"{operatorName}Edit"));
        editButton.Click();
    }
}