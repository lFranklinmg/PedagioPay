namespace PedagioPayPortalUsuario.Service.Interfaces
{
    public interface ITokenService
    {
        public string GenerateToken(string usuario, string secret, int expiration);
        public bool ValidateToken(string token, string secret);
    }
}
