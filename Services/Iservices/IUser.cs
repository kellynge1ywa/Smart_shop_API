namespace duka;

public interface IUser
{
    Task<string> SignUpUser(RegisterUserDto NewUser);

        Task<List<UserDto>> GetUsers();
        Task<User> GetOneUser(Guid UserId);
        Task<User> GetUser(Guid userId, string token);
        Task<string> UpdateUser(Guid Id,RegisterUserDto updateUser);
        Task<string> DeleteUser(User user);
        Task<LoginResponseDto> SignInUser(LoginRequestDto loginRequest);

}
