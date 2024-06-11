
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace duka;

public class UserServices : IUser

{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IJwt _jwt;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    public UserServices(AppDbContext dbContext,IMapper mapper,IJwt jwt, UserManager<User> userManager,RoleManager<IdentityRole> roleManager)
    {
        _dbContext=dbContext;
        _mapper=mapper;
        _jwt=jwt;
        _userManager=userManager;
        _roleManager=roleManager;
    }

    public async Task<string> DeleteUser(User user)
    {
        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
        return "User deleted";
    }

    

    public async Task<User> GetOneUser(Guid UserId)
    {
        return await _dbContext.AppUsers.Where(user=>user.Id == UserId.ToString()).FirstOrDefaultAsync();
    }

    public async Task<User> GetUser(Guid userId, string token)
    {
        return await _dbContext.AppUsers.Where(user=> user.Id == userId.ToString()).FirstOrDefaultAsync();
    }

    public async Task<List<UserDto>> GetUsers()
    {
        var users= await _dbContext.AppUsers.Select(user=> _mapper.Map<UserDto>(user)).ToListAsync();
        return users;

    }

    public async Task<LoginResponseDto> SignInUser(LoginRequestDto loginRequest)
    {
        var user=await _dbContext.AppUsers.Where(user=> user.UserName.ToLower() == loginRequest.Email.ToLower()).FirstOrDefaultAsync();

        var checkPassword=await  _userManager.CheckPasswordAsync(user,loginRequest.Password);
        if(!checkPassword || user == null)
        {
            return new LoginResponseDto();
        }

        var loggedUser= _mapper.Map<UserDto>(user);
        var roles=await _userManager.GetRolesAsync(user);
        var token=_jwt.GenerateToken(user,roles);

        var loggedInUser=new LoginResponseDto()
        {
            UserDto=loggedUser,
            Token=token
        };
        return loggedInUser;

    }

    public async Task<string> SignUpUser(RegisterUserDto NewUser)
    {
        var newUser= _mapper.Map<User>(NewUser);
        using (var transaction=await _dbContext.Database.BeginTransactionAsync())
        {
            var result=await _userManager.CreateAsync(newUser, NewUser.Password);

            if(result.Succeeded)
            {
                if(!_dbContext.AppUsers.Any())
                {
                    await _userManager.AddToRoleAsync(newUser,"Admin");
                }

                await _dbContext.SaveChangesAsync();
                transaction.Commit();
                return "User registered";
            }
            else
            {
                transaction.Rollback();
                return result.Errors.FirstOrDefault()!.Description;
            }
        }
    }

    public async Task<string> UpdateUser(Guid Id, RegisterUserDto updateUser)
    {
        using (var transaction= await _dbContext.Database.BeginTransactionAsync())
        {
            var userToUpdate=await _userManager.FindByIdAsync(Id.ToString());

            if (userToUpdate == null)
            {
                return "User not found";
            }

            _mapper.Map(updateUser, userToUpdate);

            if(!string.IsNullOrEmpty(updateUser.Password))
            {
                var passwordUpdate=await _userManager.ChangePasswordAsync(userToUpdate,updateUser.Password,updateUser.Password);
                if(!passwordUpdate.Succeeded)
                {
                    transaction.Rollback();
                    return passwordUpdate.Errors.FirstOrDefault()?.Description;
                }
            }

            await _userManager.UpdateAsync(userToUpdate);
            await _dbContext.SaveChangesAsync();
            transaction.Commit();
            return "User updated successfully!!";
        }

    }
}
