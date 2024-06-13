
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace duka;

public class UserServices : IUser

{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IJwt _jwt;

    public UserServices(AppDbContext dbContext, IMapper mapper, IJwt jwt)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _jwt = jwt;

    }

    public async Task<string> DeleteUser(User user)
    {
        _dbContext.AppUsers.Remove(user);
        await _dbContext.SaveChangesAsync();
        return "User deleted";
    }



    public async Task<User> GetOneUser(Guid UserId)
    {
        return await _dbContext.AppUsers.Where(user => user.Id == UserId).FirstOrDefaultAsync();
    }

    public async Task<User> GetUser(Guid userId, string token)
    {
        return await _dbContext.AppUsers.Where(user => user.Id == userId).FirstOrDefaultAsync();
    }

    public async Task<User> GetUserByEmail(string Email)
    {
        return await _dbContext.AppUsers.Where(user => user.Email.ToLower() == Email.ToLower()).FirstOrDefaultAsync();
    }

    public async Task<List<User>> GetUsers()
    {
        var users = await _dbContext.AppUsers.ToListAsync();
        return users;

    }

    public Task<LoginResponseDto> SignInUser(LoginRequestDto loginRequest)
    {
        throw new NotImplementedException();
    }

    // public async Task<LoginResponseDto> SignInUser(LoginRequestDto loginRequest)
    // {

    // }

    public async Task<string> SignUpUser(User NewUser)
    {
        var isFirstUser = _dbContext.AppUsers.Any();

        if (!isFirstUser)
        {
            NewUser.Role = "Admin";
        }
        _dbContext.AppUsers.Add(NewUser);
        await _dbContext.SaveChangesAsync();
        return "User registered";
    }

    public async Task<string> UpdateUser(Guid Id, RegisterUserDto updateUser)
    {
        var user = await _dbContext.AppUsers.Where(user => user.Id == Id).FirstOrDefaultAsync();
        if (user != null)
        {
            user.Fullname = updateUser.Fullname;
            user.Email = updateUser.Email;
            user.Password=updateUser.Password;
            user.PhoneNumber=updateUser.PhoneNumber;
            user.Residence=updateUser.Residence;

            await _dbContext.SaveChangesAsync();
            return "User updated!!";

        }
        return "User not found";
    }

    // public async Task<string> UpdateUser(Guid Id, RegisterUserDto updateUser)
    // {

    // }
}
