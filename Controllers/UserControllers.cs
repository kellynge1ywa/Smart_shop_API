using System.Security.Claims;
using AutoMapper;
using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;

namespace duka;
[Route("api/[controller]")]
[ApiController]

public class UserController : ControllerBase
{
    private readonly IUser _userServices;
    private readonly ResponseDto _response;
    private readonly IMapper _mapper;
     private readonly IJwt _jwt;
    public UserController(IUser user,IMapper mapper, IJwt jwt)
    {
        _userServices = user;
        _mapper=mapper;
        _jwt=jwt;
        _response = new ResponseDto();
    }

    [HttpGet]
    public async Task<ActionResult<ResponseDto>> GetUsers()
    {
        try
        {
            var users = await _userServices.GetUsers();
            if (users != null)
            {
                _response.Result = users;
                return Ok(_response);
            }

            _response.Error = "Users not found";
            return NotFound(_response);

        }
        catch (Exception ex)
        {
            _response.Error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            return StatusCode(500, _response);
        }
    }

    [HttpGet("{Id}")]
    public async Task<ActionResult<ResponseDto>> GetUser(Guid Id)
    {
        try
        {
            var user = await _userServices.GetOneUser(Id);
            if (user != null)
            {
                _response.Result = user;
                return Ok(_response);
            }

            _response.Error = "Users not found";
            return NotFound(_response);

        }
        catch (Exception ex)
        {
            _response.Error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            return StatusCode(500, _response);
        }
    }

    [HttpGet("loggedIn User")]
    public async Task<ActionResult<ResponseDto>> GetLoggedInUser()
    {
        try
        {
            var Token = HttpContext.Request.Headers["Authorization"].ToString().Split(" ")[1];
            var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                _response.Error = "Please log in";
                return BadRequest(_response);
            }

            var UserId = Guid.Parse(userId);
            var user = await _userServices.GetUser(UserId,Token);

            if (user != null)
            {
                _response.Result = user;
                return Ok(_response);
            }

            _response.Error = "Users not found";
            return NotFound(_response);

        }
        catch (Exception ex)
        {
            _response.Error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            return StatusCode(500, _response);
        }
    }

    [HttpPost("register")]
    public async Task<ActionResult<ResponseDto>> RegisterUser(RegisterUserDto registerUser)
    {
        try
        {
            var newuser= _mapper.Map<User>(registerUser);
            newuser.Password=BCrypt.Net.BCrypt.HashPassword(registerUser.Password);
            var checkMail= await _userServices.GetUserByEmail(registerUser.Email);
            if( checkMail != null)
            {
                _response.Error="Email exists";
                return BadRequest(_response);
            }
             var result= await _userServices.SignUpUser(newuser);
            _response.Result=result;
            
            return Created("",_response);

        }
        catch (Exception ex)
        {
            _response.Error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            return StatusCode(500, _response);
        }

    }

     [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> LoginUser(LoginRequestDto loginRequest)
    {
        try
        {
            var checkUser= await _userServices.GetUserByEmail(loginRequest.Email);
            if(checkUser == null){
                _response.Error="Email not found";
                return BadRequest(_response);
            }
            var checkPassword= BCrypt.Net.BCrypt.Verify(loginRequest.Password,checkUser.Password);
            if(!checkPassword)
            {
                _response.Error="Wrong Password";
                return BadRequest(_response);
            }
            var token= _jwt.GenerateToken(checkUser);

            var loggedUser= new LoginResponseDto()
            {
                Token=token,
                User=checkUser
            };

            _response.Result=loggedUser;
            return Ok(_response);
            

        }
        catch (Exception ex)
        {
            _response.Error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            return StatusCode(500, _response);
        }

    }

    [HttpPut("{Id}")]
    public async Task<ActionResult<ResponseDto>> UpdateUser(Guid Id,RegisterUserDto updateUser)
    {
        try
        {
            var user= await _userServices.GetOneUser(Id);
            if (user ==null)
            {
                _response.Error="User not found";
                return NotFound(_response);
            }
            var userId= user.Id;

            var updateduser= new User ()
            {
                Id=userId,
                Fullname=updateUser.Fullname,
                Email=updateUser.Email,
                PhoneNumber=updateUser.PhoneNumber,
                Password=BCrypt.Net.BCrypt.HashPassword(updateUser.Password),
                Residence=updateUser.Residence,
                Role=user.Role
            };
         
            // var checkMail= await _userServices.GetUserByEmail(updateUser.Email);
            // if( checkMail == null)
            // {
            //     _response.Error="Email exists";
            //     return BadRequest(_response);
            // }
             var result= await _userServices.UpdateUser(user.Id,updateUser);
            _response.Result=result;
            
            return Created("",_response);

        }
        catch (Exception ex)
        {
              _response.Error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            return StatusCode(500, _response);
        }
    }

    [HttpPatch("{Id}")]
    public async Task<ActionResult<ResponseDto>> UpdateUserDetails(Guid Id,RegisterUserDto updateUser)
    {
        try
        {
            var user= await _userServices.GetOneUser(Id);
            if (user ==null)
            {
                _response.Error="User not found";
                return NotFound(_response);
            }
            var userId= user.Id;

            var response= await _userServices.UpdateUser(userId,updateUser);
            if (!string.IsNullOrWhiteSpace(response))
            {
                _response.Result="Update successful";
                return Created("", _response);
            }
            _response.Error=response;
            _response.Success=false;
            return BadRequest(_response);

        }
        catch (Exception ex)
        {
              _response.Error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            return StatusCode(500, _response);
        }
    }

     [HttpDelete("{Id}")]
    public async Task<ActionResult<ResponseDto>> DeleteUser(Guid Id)
    {
        try
        {
            var user = await _userServices.GetOneUser(Id);
            if (user == null)
            {
                _response.Error = "Category not found";
                return NotFound(_response);
            }

            var deleted = await _userServices.DeleteUser(user);
            _response.Result = deleted;
            return Ok(_response);

        }
        catch (Exception ex)
        {
            _response.Error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            return StatusCode(500, _response);
        }
    }






}
