public class AuthService : IAuthService
{
    private readonly IAuthProcessor _authProcessor;
    private readonly ILogger<AuthService> _logger;

    public AuthService(IAuthProcessor authProcessor, ILogger<AuthService> logger)
    {
        _authProcessor = authProcessor;
        _logger = logger;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        _logger.LogInformation("Processing login in service layer");

        return await _authProcessor.ProcessLoginAsync(request);
    }
}