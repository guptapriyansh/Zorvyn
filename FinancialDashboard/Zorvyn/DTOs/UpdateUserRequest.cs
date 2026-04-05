public class UpdateUserRequest
{
    public Guid Id { get; set; }
    public string Role { get; set; }
    public bool IsActive { get; set; }
}