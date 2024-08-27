namespace PedagioPayPortalUsuario.Data.Dtos.Email
{
    public class SmtpDto
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string AuthUser { get; set; }
        public string AuthPass { get; set; }
        public string? SSL { get; set; }
        public bool RequerAutenticacao { get; set; }
    }
}
