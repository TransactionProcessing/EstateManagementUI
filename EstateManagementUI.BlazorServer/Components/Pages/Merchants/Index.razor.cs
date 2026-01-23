using EstateManagementUI.BlazorServer.Common;
using EstateManagementUI.BlazorServer.Factories;
using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BlazorServer.Permissions;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.Requests;

namespace EstateManagementUI.BlazorServer.Components.Pages.Merchants
{
    public partial class Index
    {
        private bool isLoading = true;
        private List<MerchantListModel>? allMerchants;

        // Filter fields
        private string _filterName = "";
        private string filterName
        {
            get => _filterName;
            set { _filterName = value; currentPage = 1; }
        }

        private string _filterReference = "";
        private string filterReference
        {
            get => _filterReference;
            set { _filterReference = value; currentPage = 1; }
        }

        private string _filterSettlementSchedule = "-1";
        private string filterSettlementSchedule
        {
            get => _filterSettlementSchedule;
            set { _filterSettlementSchedule = value; currentPage = 1; }
        }

        private string _filterRegion = "";
        private string filterRegion
        {
            get => _filterRegion;
            set { _filterRegion = value; currentPage = 1; }
        }

        private string _filterPostcode = "";
        private string filterPostcode
        {
            get => _filterPostcode;
            set { _filterPostcode = value; currentPage = 1; }
        }

        // Pagination fields
        private int _currentPage = 1;
        private int currentPage
        {
            get => _currentPage;
            set { _currentPage = value; }
        }

        private int _pageSize = 10;
        private int pageSize
        {
            get => _pageSize;
            set { _pageSize = value; _currentPage = 1;  }
        }

        // Cached results
        //private List<MerchantListModel> _filteredMerchants = new();
        //private List<MerchantListModel> filteredMerchants => _filteredMerchants;

        private List<MerchantListModel> _pagedMerchants = new();
        private List<MerchantListModel> pagedMerchants => _pagedMerchants;

        private int _totalPages = 1;
        private int totalPages => _totalPages;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
            {
                await base.OnAfterRenderAsync(firstRender);
                return;
            }
            try
            {
                await base.OnInitializedAsync();

                await RequirePermission(PermissionSection.Merchant, PermissionFunction.List);

                await this.LoadMerchants();

            }
            finally
            {
                isLoading = false;
                this.StateHasChanged();
            }
        }

        private async Task LoadMerchants() {
            CorrelationId correlationId = new(Guid.NewGuid());
            Guid estateId = await this.GetEstateId();
            var result = await Mediator.Send(new MerchantQueries.GetMerchantsQuery(correlationId, estateId, this._filterName, this._filterReference, Int32.Parse(_filterSettlementSchedule),
                this._filterRegion, this._filterPostcode));
            if (result.IsFailed)
            {
                this.NavigationManager.NavigateToErrorPage();
            }

            allMerchants = ModelFactory.ConvertFrom(result.Data);
            this.UpdatePagedMerchants();
        }

        //protected override async Task OnInitializedAsync()
        //{
        //    try
        //    {
        //        await RequirePermission(PermissionSection.Merchant, PermissionFunction.List);

        //        var correlationId = new CorrelationId(Guid.NewGuid());
        //        var estateId = await this.GetEstateId();

        //        var result = await Mediator.Send(new Queries.GetMerchantsQuery(correlationId, estateId, this._filterName, this._filterReference, Int32.Parse(_filterSettlementSchedule), 
        //            this._filterRegion, this._filterPostcode));
        //        if (result.IsSuccess)
        //        {
        //            allMerchants = ModelFactory.ConvertFrom(result.Data);
        //            //UpdateFilteredMerchants();
        //        }
        //    }
        //    finally
        //    {
        //        isLoading = false;
        //    }
        //}

        //private void UpdateFilteredMerchants()
        //{
        //    if (allMerchants == null)
        //    {
        //        _filteredMerchants = new List<MerchantModel>();
        //        _totalPages = 1;
        //        UpdatePagedMerchants();
        //        return;
        //    }

        //    // Apply all filters in a single pass using a combined predicate
        //    _filteredMerchants = allMerchants.Where(m =>
        //        (string.IsNullOrWhiteSpace(_filterName) || (m.MerchantName?.Contains(_filterName, StringComparison.OrdinalIgnoreCase) ?? false)) &&
        //        (string.IsNullOrWhiteSpace(_filterReference) || (m.MerchantReference?.Contains(_filterReference, StringComparison.OrdinalIgnoreCase) ?? false)) &&
        //        (string.IsNullOrWhiteSpace(_filterSettlementSchedule) || (m.SettlementSchedule?.Equals(_filterSettlementSchedule, StringComparison.OrdinalIgnoreCase) ?? false)) &&
        //        (string.IsNullOrWhiteSpace(_filterRegion) || (m.Region?.Contains(_filterRegion, StringComparison.OrdinalIgnoreCase) ?? false)) &&
        //        (string.IsNullOrWhiteSpace(_filterPostcode) || (m.PostalCode?.Contains(_filterPostcode, StringComparison.OrdinalIgnoreCase) ?? false))
        //    ).ToList();

        //    _totalPages = _filteredMerchants.Count > 0 ? (int)Math.Ceiling(_filteredMerchants.Count / (double)_pageSize) : 1;

        //    // Ensure currentPage is valid
        //    if (_currentPage > _totalPages && _totalPages > 0)
        //    {
        //        _currentPage = _totalPages;
        //    }

        //    UpdatePagedMerchants();
        //}

        private void UpdatePagedMerchants()
        {
            _pagedMerchants = this.allMerchants.Skip((_currentPage - 1) * _pageSize).Take(_pageSize).ToList();
        }

        private async Task ClearFilters()
        {
            _filterName = "";
            _filterReference = "";
            _filterSettlementSchedule = "-1";
            _filterRegion = "";
            _filterPostcode = "";
            _currentPage = 1;
            await this.LoadMerchants();
        }

        private void FirstPage()
        {
            if (_currentPage != 1)
            {
                _currentPage = 1;
                //UpdatePagedMerchants();
            }
        }

        private void PreviousPage()
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                //UpdatePagedMerchants();
            }
        }

        private void NextPage()
        {
            if (_currentPage < _totalPages)
            {
                _currentPage++;
               // UpdatePagedMerchants();
            }
        }

        private void LastPage()
        {
            var lastPage = _totalPages > 0 ? _totalPages : 1;
            if (_currentPage != lastPage)
            {
                _currentPage = lastPage;
               // UpdatePagedMerchants();
            }
        }

        private void ViewMerchant(Guid merchantId)
        {
            NavigationManager.NavigateTo($"/merchants/{merchantId}");
        }

        private void EditMerchant(Guid merchantId)
        {
            NavigationManager.NavigateTo($"/merchants/{merchantId}/edit");
        }

        private void MakeDeposit(Guid merchantId)
        {
            NavigationManager.NavigateTo($"/merchants/{merchantId}/deposit");
        }

        private void NavigateToNewMerchant()
        {
            NavigationManager.NavigateTo("/merchants/new");
        }
    }
}
