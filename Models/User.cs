using Microsoft.AspNetCore.Identity;

namespace duka;

public class User:IdentityUser
{
     public string Fullname { get; set; } = "";
        public DateTime DOB { get; set; }

}
