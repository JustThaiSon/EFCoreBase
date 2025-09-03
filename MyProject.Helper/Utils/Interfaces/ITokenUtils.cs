namespace MyProject.Helper.Utils.Interfaces
{
    public interface ITokenUtils
    {
        //string GenerateJwt(User user, IList<string> roles);

        string GenerateToken(long id);
        string GenerateRefreshToken(long id);
        string? GenerateTokenFromRefreshToken(string refreshToken);
        long? ValidateToken(string token);
        bool IsAccessTokenExpired(string accessToken);
    }
}
