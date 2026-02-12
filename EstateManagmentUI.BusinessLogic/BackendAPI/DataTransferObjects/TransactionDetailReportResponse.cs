using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EstateManagementUI.BusinessLogic.BackendAPI.DataTransferObjects
{
    public class TransactionDetailReportResponse
    {
        [JsonProperty("transactions")]
        public List<TransactionDetail> Transactions { get; set; }
        [JsonProperty("summary")]
        public TransactionDetailSummary Summary { get; set; }
    }
}
