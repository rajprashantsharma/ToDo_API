namespace ToDo_API.Responses
{
    public record SuccessResponse(bool Success = true);

    public record ErrorResponse(string Message, bool Success = false);
}
