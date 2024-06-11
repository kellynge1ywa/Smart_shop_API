using System.ComponentModel.DataAnnotations;

namespace duka;

public class RegisterUserDto
{
    [Required]
    public string Fullname { get; set; } = "";
    [Required]
    public string Email { get; set; } = "";
    [Required]
    public string Password { get; set; } = "";
    [Required]
    public string Residence { get; set; } = "";
    [Required]
    public string PhoneNumber { get; set; } = "";
    public DateTime DOB { get; set; }


    public string Role { get; set; } = "User";

}
