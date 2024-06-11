namespace duka;

public interface IJwt
{
    string GenerateToken(User appUser, IEnumerable<string> Roles);
}
