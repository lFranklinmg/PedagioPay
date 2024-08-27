using PedagioPayPortalUsuario.Data.Dtos.Email;

namespace PedagioPayPortalUsuario.Service.Interfaces
{
    public interface IEmailService
    {
        public Task SendEmail(EmailDto emailDetails, SmtpDto smtpDetails);
        public string CriaCodigoConfirmacao();
        public bool ValidaCodigoConfirmacao(string codigo);
        public Task NewEmail(string to, string body, string subject);
        public string ConfirmRegisterBody(string tokenConcessao, string nome, string codigoValidacao);
        public string NewPassWordBody(string tokenConcessao, string link);

    }
}
