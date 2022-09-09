using ApiCatalogo.Models;

namespace ApiCatalogo.Services;

public interface ITokensService
{
    string GerarToken(string key, string issue, string audience, UserModel user);
}
