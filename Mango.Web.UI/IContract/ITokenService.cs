namespace Mango.Web.UI.IContract
{
    public interface ITokenService
    {
        void SetToken(string token);
        string GetToken();
        void RemoveToken();
    }
}
