using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.ViewModels;

[ExcludeFromCodeCoverage]
public class FileLine {
    public Int32 LineNumber { get; set; }
    public String Data { get; set; }
    public FileLineProcessingResult ProcessingResult { get; set; }
    public Guid TransactionId { get; set; }
    public String RejectionReason { get; set; }
}