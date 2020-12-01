namespace BlazorBattles.Shared.Entities
{
    public record ServiceResponse<T>(T Data, string Message, bool Success = true);
}