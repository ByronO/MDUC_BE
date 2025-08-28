namespace MDUC_BE.Common.Errors;

public static class ApiErrorCode
{
    public const string Validation = "validation_error";
    public const string Unauthorized = "unauthorized";
    public const string UpstreamError = "upstream_error";
    public const string Unexpected = "unexpected_error";
}
