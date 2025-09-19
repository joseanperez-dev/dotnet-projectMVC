namespace projectMVC.Models;

/*
*   ViewModel used for error views to track request identifier for diagnostics.
*/
public class ErrorViewModel
{
    /*
    *   The request identifier associated with the error context.
    */
    public string? RequestId { get; set; }

    /*
    *   Indicates whether the RequestId property has a value.
    */
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}
