public interface IAuthProcessor
{
    Task<LoginResponse> ProcessLoginAsync(LoginRequest request);
}