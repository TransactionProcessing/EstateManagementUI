using System.Collections.ObjectModel;
using System.Text;
using EstateManagementUI.IntegrationTests.Steps;
using EventStore.Client;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using Reqnroll;
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
        await Retry.For(async () => { this.VerifyPageTitle("New Contract"); });
    }

    public async Task VerifyOnTheNewMerchantScreen(){
        await Retry.For(async () => { this.VerifyPageTitle("New Merchant"); });
    }

    public async Task VerifyOnTheEditMerchantScreen()
    {
        await Retry.For(async () => { this.VerifyPageTitle("Edit Merchant"); });
    }

    public async Task VerifyOnTheViewMerchantScreen()
    {
        await Retry.For(async () => { this.VerifyPageTitle("View Merchant"); });
    }

    public async Task VerifyOnTheNewOperatorScreen()
    {
        await Retry.For(async () => { this.VerifyPageTitle("New Operator"); });
    }

    public async Task VerifyOnTheEditOperatorScreen()
    {
        await Retry.For(async () => { this.VerifyPageTitle("Edit Operator"); });
    }

    public async Task VerifyOnTheContractsListScreen(){
        await Retry.For(async () => { this.VerifyPageTitle("View Contracts"); });
    }

    public async Task VerifyOnTheContractProductsListScreen()
    {
        await Retry.For(async () => { this.VerifyPageTitle("View Contract Products"); });
    }

    public async Task VerifyOnTheContractProductsFeesListScreen() {
        await Retry.For(async () => { this.VerifyPageTitle("View Contract Product Fees"); });
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

    public async Task ClickOnTheMerchantOperatorsTab()
    {
        await this.ClickTab("nav-operators-tab");
    }

    public async Task ClickOnTheMerchantContractsTab()
    {
        await this.ClickTab("nav-contracts-tab");
    }

    public async Task VerifyOnMerchantOperatorsTab() {
        IWebElement element = this.WebDriver.FindElement(By.Id("merchantOperatorList"));
        element.ShouldNotBeNull();
    }

    public async Task VerifyOnMerchantContractsTab()
    {
        IWebElement element = this.WebDriver.FindElement(By.Id("merchantContractList"));
        element.ShouldNotBeNull();
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

    public async Task VerifyTheCorrectEstateDetailsAreDisplayed(String estateName){
        await Retry.For(async () => {
                            IWebElement element = this.WebDriver.FindElement(By.Id("Estate_Name"));
                            element.ShouldNotBeNull();
                            String elementValue = element.GetDomProperty("value");
                            elementValue.ShouldBe(estateName);

                            element = this.WebDriver.FindElement(By.Id("Estate_Reference"));
                            element.ShouldNotBeNull();
                            elementValue = element.GetDomProperty("value");
                            elementValue.ShouldNotBeNull();
                            elementValue.ShouldNotBeEmpty();
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

    public async Task VerifyTheContractProductDetailsAreInTheList(List<(String, String, String, String)> contractProductDescriptions)
    {
        await Retry.For(async () => {
                Int32 foundRowCount = 0;
                IWebElement tableElement = this.WebDriver.FindElement(By.Id("contractProductList"));
                IList<IWebElement> rows = tableElement.FindElements(By.TagName("tr"));

                rows.Count.ShouldBe(contractProductDescriptions.Count + 1);
                foreach ((String, String, String,String) contractProductDescription in contractProductDescriptions)
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

                        if (rowTD[0].Text == contractProductDescription.Item1)
                        {
                            // Compare other fields
                            rowTD[0].Text.ShouldBe(contractProductDescription.Item1);
                            rowTD[1].Text.ShouldBe(contractProductDescription.Item2);
                            rowTD[2].Text.ShouldBe(contractProductDescription.Item3.ToString());
                            rowTD[3].Text.ShouldBe(contractProductDescription.Item4.ToString());

                        // We have found the row
                        foundRowCount++;
                            break;
                        }
                    }
                }

                foundRowCount.ShouldBe(contractProductDescriptions.Count);
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

    public async Task VerifyOperatorDetailsAreInTheList(String tableId,  List<(String, String, String, String)> operatorDetails) {
        await Retry.For(async () => {
            Int32 foundRowCount = 0;
            IWebElement tableElement = this.WebDriver.FindElement(By.Id(tableId));
            IList<IWebElement> rows = tableElement.FindElements(By.TagName("tr"));

            rows.Count.ShouldBe(operatorDetails.Count + 1);
            StringBuilder sb = new StringBuilder();
            foreach ((String, String, String, String) operatorDetail in operatorDetails) {
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
                        if (String.IsNullOrEmpty(operatorDetail.Item4) == false) {
                            rowTD[3].Text.ShouldBe(operatorDetail.Item4);
                        }

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

    public async Task VerifyContractDetailsAreInTheList(String tableId, List<(String, String)> contractDetails)
    {
        await Retry.For(async () => {
            Int32 foundRowCount = 0;
            IWebElement tableElement = this.WebDriver.FindElement(By.Id(tableId));
            IList<IWebElement> rows = tableElement.FindElements(By.TagName("tr"));

            rows.Count.ShouldBe(contractDetails.Count + 1);
            StringBuilder sb = new StringBuilder();
            foreach ((String, String) contractDetail in contractDetails)
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

                    if (rowTD[0].Text == contractDetail.Item1)
                    {
                        // Compare other fields
                        rowTD[0].Text.ShouldBe(contractDetail.Item1);
                        rowTD[1].Text.ShouldBe(contractDetail.Item2);
                        
                        // We have found the row
                        foundRowCount++;
                        sb.AppendLine($"Found {contractDetail.Item1}");
                        break;
                    }
                    else
                    {
                        sb.AppendLine($"Not Found {contractDetail.Item1}");
                    }
                }
            }

            foundRowCount.ShouldBe(contractDetails.Count, sb.ToString());
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

    public async Task ClickTheNewMerchantButton()
    {
        await this.WebDriver.ClickButtonById("newMerchantButton");
    }

    public async Task ClickTheNewOperatorButton()
    {
        await this.WebDriver.ClickButtonById("newOperatorButton");
    }

    public async Task ClickTheSaveOperatorButton()
    {
        await this.WebDriver.ClickButtonById("saveOperatorButton");
    }

    public async Task ClickTheAssignOperatorButton()
    {
        await this.WebDriver.ClickButtonById("saveMerchantOperatorButton");
    }

    public async Task ClickTheAssignContractButton()
    {
        await this.WebDriver.ClickButtonById("saveMerchantContractButton");
    }

    public async Task ClickTheSaveMerchantButton()
    {
        await this.WebDriver.ClickButtonById("saveMerchantButton");
    }

    public async Task ClickTheSaveContractButton()
    {
        await this.WebDriver.ClickButtonById("saveContractButton");
    }

    public async Task ClickTheNewContractButton()
    {
        await this.WebDriver.ClickButtonById("newContractButton");
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
        await this.WebDriver.FillIn("Name", contractDescription);
        await this.WebDriver.SelectDropDownItemByText("operatorList", operatorName);
    }

    public async Task ClickTab(String tabName) {
        await this.WebDriver.ClickButtonById(tabName);
    }

    public async Task EnterMerchantUpdateDetails(List<EstateManagementUiSteps.MerchantUpdate> updates) {
        foreach (EstateManagementUiSteps.MerchantUpdate update in updates) {
            // Navigate to the tab
            switch (update.tab) {
                case "Details":
                    await this.ClickTab("nav-merchantdetails-tab");
                    break;
                case "Address":
                    await this.ClickTab("nav-address-tab");
                    break;
                case "Contact":
                    await this.ClickTab("nav-contacts-tab");
                    break;
            }
            // Fill in the field
            switch (update.field) {
                case "Name":
                    await this.WebDriver.FillIn("Name", update.value, true);
                    break;
                case "AddressLine1":
                    await this.WebDriver.FillIn("Address.AddressLine1", update.value, true);
                    break;
                case "ContactName":
                    await this.WebDriver.FillIn("Contact.ContactName", update.value, true);
                    break;
            }
        }
    }

    public async Task EnterMerchantDetails(String merchantName, String addressLine1, String town, String region, String postCode, String country, String contactName, String contactEmail, String contactPhoneNumber, String settlementSchedule){
        await this.WebDriver.FillIn("Name", merchantName);
        await this.WebDriver.FillIn("Address.AddressLine1", addressLine1);
        await this.WebDriver.FillIn("Address.Town", town);
        await this.WebDriver.FillIn("Address.Region", region);
        await this.WebDriver.FillIn("Address.PostCode", postCode);
        await this.WebDriver.FillIn("Address.Country", country);
        await this.WebDriver.FillIn("Contact.ContactName", contactName);
        await this.WebDriver.FillIn("Contact.EmailAddress", contactEmail);
        await this.WebDriver.FillIn("Contact.PhoneNumber", contactPhoneNumber);
        await this.WebDriver.SelectDropDownItemByText("settlementSchedule", settlementSchedule);
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

    public async Task EnterOperatorDetails(String operatorName,
                                           String merchantNumber,
                                           String terminalNumber)
    {
        await this.WebDriver.SelectDropDownItemByText("operatorName", operatorName);
        await this.WebDriver.FillInById("merchantNumber", merchantNumber, true);
        await this.WebDriver.FillInById("terminalNumber", terminalNumber, true);
    }

    public async Task EnterContractDetails(String contractName)
    {
        await this.WebDriver.SelectDropDownItemByText("contractName", contractName);
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
                                           String elementToClickId) {
        Boolean foundRow = false;
        IWebElement itemrow = null;
        await Retry.For(async () => {
            IWebElement tableElement = this.WebDriver.FindElement(By.Id(tableId));
            IList<IWebElement> rows = tableElement.FindElements(By.TagName("tr"));

            rows.ShouldNotBeNull();
            rows.Any().ShouldBeTrue();
            IList<IWebElement> rowTD;
            foreach (IWebElement row in rows) {
                ReadOnlyCollection<IWebElement> rowTH = row.FindElements(By.TagName("th"));

                if (rowTH.Any()) {
                    // header row so skip
                    continue;
                }

                rowTD = row.FindElements(By.TagName("td"));

                if (rowTD[0].Text == textToSearchFor) {
                    itemrow = row;
                    foundRow = true;
                    break;
                }
            }


            foundRow.ShouldBeTrue();
            itemrow.ShouldNotBeNull();
        }, TimeSpan.FromSeconds(120)).ConfigureAwait(false);


        await Retry.For(async () => {
            IWebElement element = itemrow.FindElement(By.Id(elementToClickId));
            if (element.Displayed) {
                element.Click();
            }
            else {
                this.WebDriver.ExecuteJavaScript($"document.getElementById('{elementToClickId}').click();");
            }
        }, TimeSpan.FromSeconds(120));
    }

    private async Task<IWebElement> GetButtonInDropdown(IWebElement dropdownButton,
                                                        String buttonId) {
        IWebElement result = null;
        dropdownButton.Click();
        IWebElement button = this.WebDriver.FindElement(By.Id(buttonId));
        if (button.Displayed && button.Enabled) {
            result = button;
        }
        else {
            dropdownButton.Click();
        }
        
        return result;
    }

    public async Task ClickTheEditOperatorButton(String operatorName) {
        IWebElement tableElement = this.WebDriver.FindElement(By.Id("operatorList"));
        var x = tableElement.FindElements(By.Id("dropdownMenuButton"));
        IWebElement editButton = null;
        foreach (IWebElement webElement in x) {
            var gg = await GetButtonInDropdown(webElement, $"{operatorName}Edit");
            if (gg != null) {
                editButton = gg;
                break;
            }
        }

        if (editButton != null) {
            editButton.Click();
        }
        else {
            throw new Exception($"Edit button not found for operator {operatorName}");
        }
    }

    public async Task ClickTheRemoveOperatorButton(String operatorName)
    {
        IWebElement tableElement = this.WebDriver.FindElement(By.Id("merchantOperatorList"));
        var x = tableElement.FindElements(By.Id("dropdownMenuButton"));
        IWebElement editButton = null;
        foreach (IWebElement webElement in x) {
            var gg = await GetButtonInDropdown(webElement, $"{operatorName}Remove");
            if (gg != null) {
                editButton = gg;
                break;
            }
        }

        if (editButton != null) {
            editButton.Click();
        }
        else {
            throw new Exception($"Remove button not found for operator {operatorName}");
        }
    }

    public async Task ClickTheRemoveContractButton(String contractName)
    {
        IWebElement tableElement = this.WebDriver.FindElement(By.Id("merchantContractList"));
        var x = tableElement.FindElements(By.Id("dropdownMenuButton"));
        IWebElement editButton = null;
        foreach (IWebElement webElement in x)
        {
            var gg = await GetButtonInDropdown(webElement, $"{contractName}Remove");
            if (gg != null)
            {
                editButton = gg;
                break;
            }
        }

        if (editButton != null)
        {
            editButton.Click();
        }
        else
        {
            throw new Exception($"Remove button not found for contract {contractName}");
        }
    }

    public async Task ClickTheEditMerchantButton(String merchantName)
    {
        IWebElement tableElement = this.WebDriver.FindElement(By.Id("merchantList"));
        var x = tableElement.FindElements(By.Id("dropdownMenuButton"));
        IWebElement editButton = null;
        foreach (IWebElement webElement in x)
        {
            var gg = await GetButtonInDropdown(webElement, $"{merchantName}Edit");
            if (gg != null)
            {
                editButton = gg;
                break;
            }
        }

        if (editButton != null)
        {
            editButton.Click();
        }
        else
        {
            throw new Exception($"Edit button not found for merchant {merchantName}");
        }
    }

    public async Task ClickTheViewMerchantButton(String merchantName)
    {
        IWebElement tableElement = this.WebDriver.FindElement(By.Id("merchantList"));
        var x = tableElement.FindElements(By.Id("dropdownMenuButton"));
        IWebElement editButton = null;
        foreach (IWebElement webElement in x)
        {
            var gg = await GetButtonInDropdown(webElement, $"{merchantName}View");
            if (gg != null)
            {
                editButton = gg;
                break;
            }
        }

        if (editButton != null)
        {
            editButton.Click();
        }
        else
        {
            throw new Exception($"View button not found for merchant {merchantName}");
        }
    }

    public async Task ClickTheViewContractProductsButton(String contractName)
    {
        IWebElement tableElement = this.WebDriver.FindElement(By.Id("contractList"));
        var x = tableElement.FindElements(By.Id("dropdownMenuButton"));
        IWebElement editButton = null;
        foreach (IWebElement webElement in x)
        {
            var gg = await GetButtonInDropdown(webElement, $"{contractName}ViewProducts");
            if (gg != null)
            {
                editButton = gg;
                break;
            }
        }

        if (editButton != null)
        {
            editButton.Click();
        }
        else
        {
            throw new Exception($"View Products button not found for contract {contractName}");
        }
    }

    public async Task ClickTheViewContractProductFeesButton(String productName)
    {
        //IWebElement tableElement = this.WebDriver.FindElement(By.Id("contractProductList"));
        //var dropdownMenuButton = tableElement.FindElement(By.Id("dropdownMenuButton"));
        //dropdownMenuButton.Click();
        //IWebElement editButton = this.WebDriver.FindElement(By.Id($"{productName.Replace(" ", "")}ViewFees"));
        //editButton.Click();
        IWebElement tableElement = this.WebDriver.FindElement(By.Id("contractProductList"));
        var x = tableElement.FindElements(By.Id("dropdownMenuButton"));
        IWebElement editButton = null;
        foreach (IWebElement webElement in x)
        {
            var gg = await GetButtonInDropdown(webElement, $"{productName.Replace(" ", "")}ViewFees");
            if (gg != null)
            {
                editButton = gg;
                break;
            }
        }

        if (editButton != null)
        {
            editButton.Click();
        }
        else
        {
            throw new Exception($"View Fees button not found for product {productName}");
        }
    }

    public async Task ClickTheAddOperatorButton() {
        await this.WebDriver.ClickButtonById("addOperatorButton");
    }

    public async Task ClickTheAddContractButton()
    {
        await this.WebDriver.ClickButtonById("addContractButton");
    }


    public async Task VerifyAssignOperatorDialogIsDisplayed() {
        await Retry.For(async () => {
            IWebElement element = this.WebDriver.FindElement(By.Id("AssignOperatorDialog"));
            element.ShouldNotBeNull();
        });
    }

    public async Task VerifyAssignContractDialogIsDisplayed()
    {
        await Retry.For(async () => {
            IWebElement element = this.WebDriver.FindElement(By.Id("AssignContractDialog"));
            element.ShouldNotBeNull();
        });
    }
}