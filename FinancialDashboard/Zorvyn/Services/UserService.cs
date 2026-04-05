public class UserService : IUserService
{
    private readonly IUserProcessor _processor;

    public UserService(IUserProcessor processor)
    {
        _processor = processor;
    }

    public Task CreateUser(CreateUserRequest request, string createdBy)
        => _processor.CreateUser(request, createdBy);

    public Task<List<User>> GetUsers()
        => _processor.GetUsers();

    public Task UpdateUser(UpdateUserRequest request, string updatedBy)
        => _processor.UpdateUser(request, updatedBy);

    public Task DeactivateUser(Guid id)
        => _processor.DeactivateUser(id);
}