namespace duka;

public interface IUser
{
    Task<string> SignUpUser(User NewUser);

        Task<List<User>> GetUsers();
        Task<User> GetOneUser(Guid UserId);

        Task<User > GetUserByEmail(string Email);
        Task<User> GetUser(Guid userId, string token);
        Task<string> UpdateUser(Guid Id,RegisterUserDto updateUser);
        Task<string> DeleteUser(User user);
        Task<LoginResponseDto> SignInUser(LoginRequestDto loginRequest);

}
