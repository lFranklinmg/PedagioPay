using Fadami.Helper.Emails.Entity;

namespace PedagioPayPortalUsuario.Models.Util
{
    public class Util
    {
        public static Email Email(string aliasRemetente, string assunto, string smtp, string username, string password, string porta)
        {
            var email = new Email
            {
                SMTP =
            {
                Servidor = smtp,
                Login = username,
                Senha = password,
                UtilizaSSL = false,
                RequerAutenticacao = false,
                Porta = int.Parse(porta)
            },
                AliasRemetente = aliasRemetente,
                Remetente = username,
                Assunto = assunto
            };

            return email;
        }
    }
}
