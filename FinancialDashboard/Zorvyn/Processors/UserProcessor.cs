using Microsoft.EntityFrameworkCore;

public class UserProcessor : IUserProcessor
{
    private readonly AppDbContext _context;

    public UserProcessor(AppDbContext context)
    {
        _context = context;
    }

    public async Task CreateUser(CreateUserRequest request, string createdBy)
    {
        var user = new User
        {
            Email = request.Email,
            PasswordHash = request.Password,
            Role = request.Role,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = createdBy
        };

        await _context.Users.AddAsync(user);

        await _context.SaveChangesAsync();
    }

    public async Task<List<User>> GetUsers()
    {
        return await _context.Users
            .Where(x => !x.IsDeleted)
            .ToListAsync();
    }

    public async Task UpdateUser(UpdateUserRequest request, string updatedBy)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Id == request.Id);

        if (user == null)
            throw new Exception("User not found");

        user.Role = request.Role;
        user.IsActive = request.IsActive;
        user.UpdatedAt = DateTime.UtcNow;
        user.UpdatedBy = updatedBy;

        await _context.SaveChangesAsync();
    }

    public async Task DeactivateUser(Guid id)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Id == id);

        user.IsActive = false;

        await _context.SaveChangesAsync();
    }
}