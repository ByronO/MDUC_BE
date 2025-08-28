namespace MDUC_BE.Common.Models;

public record ErrorResponse(string TraceId, string ErrorCode, string Message, object? Details);
