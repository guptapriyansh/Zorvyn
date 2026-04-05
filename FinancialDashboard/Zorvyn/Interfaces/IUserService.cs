public interface IUserService
{
    Task CreateUser(CreateUserRequest request, string createdBy);
    Task<List<User>> GetUsers();
    Task UpdateUser(UpdateUserRequest request, string updatedBy);
    Task DeactivateUser(Guid id);
}