using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;
using System.Net.Mail;
using PedagioPayPortalUsuario.Service.Interfaces;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;
using PedagioPayPortalUsuario.Data.Interfaces;
using PedagioPayPortalUsuario.Data.Dtos.Email;
using Serilog;
using System.Net.Mail;
using Elasticsearch.Net;

namespace PedagioPayPortalUsuario.Service
{
    public class EmailService : IEmailService
    {
        private readonly IRepositoryUsuario _repositoryUsuario;
        private readonly IConfiguration _configuration;
        public EmailService(IRepositoryUsuario repositoryUsuario, IConfiguration configuration)
        {
            _repositoryUsuario = repositoryUsuario;
            _configuration = configuration;
        }
        public async Task SendEmail(EmailDto emailDetalhes, SmtpDto smtpDetalhes)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(emailDetalhes.From));
                email.To.Add(MailboxAddress.Parse(emailDetalhes.To));
                email.Subject = emailDetalhes.Subject;
                email.Body = new TextPart(TextFormat.Html) { Text = emailDetalhes.Body };

                using var smtp = new MailKit.Net.Smtp.SmtpClient();
                smtp.Connect(smtpDetalhes.Host, smtpDetalhes.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(smtpDetalhes.AuthUser, smtpDetalhes.AuthPass);
                smtp.Send(email);
                smtp.Disconnect(true);
            }
            catch (Exception ex)
            {
                Log.Information("Erro ao acessar serviço de email", ex);
                throw;
            };
        }
        public async Task NewEmail(string to, string body, string subject)
        {
            try
            {
                SmtpDto smtp = new SmtpDto()
                {
                    Host = _configuration.GetSection("DevAppSettings:DevDeafaultSmtp:Host").Value,
                    Port = int.Parse(_configuration.GetSection("DevAppSettings:DevDeafaultSmtp:Port").Value),
                    AuthUser = _configuration.GetSection("DevAppSettings:DevDeafaultSmtp:Auth:User").Value,
                    AuthPass = _configuration.GetSection("DevAppSettings:DevDeafaultSmtp:Auth:Password").Value
                };
                EmailDto email = new EmailDto()
                {
                    To = to,
                    From = _configuration.GetSection("DevAppSettings:DefaultEmail").Value,
                    Body = body,
                    Subject = $"{subject} | PedagioPay"
                };
                await SendEmail(email, smtp);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao enviar email.");
            }
        }

        public string CriaCodigoConfirmacao()
        {
            List<int> list = new List<int>();
            var rand = new Random();
            for (int i = 0; i < 2; i++)
            {
                list.Add(rand.Next(10, 99));
                var codigo = list;
                Console.WriteLine(codigo);
            }
            list.ToString();
            var result = string.Join("", list);
            return result;
        }

        public bool ValidaCodigoConfirmacao(string codigo)
        {
            if (_repositoryUsuario.BuscaCodConfirmacao(codigo))
            {
                return true;
            }
            return false;

        }

        public string ConfirmRegisterBody(string tokenConcessao, string nome, string codigoValidacao)
        {
            string link = "";
            string base64String = "";
            if (tokenConcessao == "1")
            {
                 link = "https://pedagiopay.com/";
                base64String = "https://pedagiopay.com/img/logo-pedagiopay.png";
                Console.WriteLine(base64String);
            }
            if (tokenConcessao == "2")
            {
                 link = "https://www.ecoponte.com.br/";
                base64String = "https://pedagiopay.com/img/ecoponte-logo.png";
                Console.WriteLine(base64String);
            }
            if (tokenConcessao == "3")
            {
                 link = "https://www.econoroeste.com.br/";
                base64String = "https://pedagiopay.com/img/logo-econoroeste-home.png";
                Console.WriteLine(base64String);
            }

            string corpoEmail = $@"<!DOCTYPE html>
<html lang='pt-br'>
<head>
<meta charset='UTF-8'>
<meta name='viewport' content='width=device-width, initial-scale=1.0'>
<title>Concluir cadastro</title>

    <link rel='stylesheet' href='./css/style.css' />
</head>
<body>
 
    <center class='wrapper' style='width: 100%;
    table-layout: fixed;
    background-color: #cccccc;
    padding-bottom: 60px;'>
 
        <table class='main' width='100%' style='            background-color: #ffffff;
            margin: 0 auto;
            width: 100%;
            max-width: 600px;
            border-spacing: 0;
            font-family: Arial, ""Helvetica Neue"", Helvetica, sans-serif;
            color: #171a1b;'>
 
 
        <!-- TOP BORDER -->
<tr>
<td height='8' style='background-color: #F7A707;'></td>
</tr>
 
        <!-- LOGO -->
<tr>
<td style='padding: 14px 0 15px;'>
<table width='100%'>
<tr>
<td class='two-columns' style='display:flex'>
<table class='column' style = 'width: 100%;
            max-width: 300px;
            display: inline-block;
            vertical-align: middle;
            text-align: center;'>
<tr>
<td style='padding: 34px 40px 30px;'>
<a href='http://fadami.com.br/pedagiopay' target='_blank'>
<img src='https://pedagiopay.com/img/logo-pedagiopay.png' width='210' alt='PEDAGIOPAY'>
</a>
</td>
</tr>
</table>
<table class='column'>
<tr>
<td style='padding: 10px 75px 15px;'>
<a href='" +link+@"'target='_blank'>
<img src='" + base64String + @"' width='160' alt='LOGO'>
</a>
</td>
</tr>
</table>
</td>
</tr>
</table>
</td>
</tr>
 
        <!-- BANNER -->
<tr>
 
            <td style='background: linear-gradient(340deg, rgb(217, 222, 224) 0%, rgb(248, 248, 248) 100%);color: #03436e;'>
<table width='100%'>
<tr>
<td class='two-columns last' style=' padding: 15px 0;'>
 
                            <table class='column' style='            width: 100%;
            max-width: 300px;
            display: inline-block;
            vertical-align: middle;
            text-align: center;'>
<tr>
<td class='padding' style='padding: 0 40px;'>
 
                                        <table class='content' style='            line-height: 20px;
            text-align: left;'>
<tr>
<td>
<p style='font-size:34px; line-height:44px; font-weight: 700; padding-left: 10px;'>Conclua seu cadastro</p>
</td>
</tr>
</table>
 
                                    </td>
</tr>
</table>
 
                            <table class='column' style='            width: 100%;
            max-width: 300px;
            display: inline-block;
            vertical-align: middle;
            text-align: center;'>
<tr>
<td class='padding' style='padding: 0 40px;'>
 
<table class='content' style='            line-height: 20px;
            text-align: left;'>
<tr>
<td>
<a href='#'><img src='https://pedagiopay.com/img/cadastro.png' alt='' width='260' style='max-width:260px;'></a>
</td>
</tr>
</table>
 
                                    </td>
</tr>
</table
 
 
                        </td>
</tr>
</table>
</td>
</tr>
 
        <!-- CONTEÚDO -->
 
        <tr>
<td>
<table width='100%'>
 
                    <tr>
<td style='text-align: center; padding: 35px 15px'>
 
                                <p style='font-size:24px; font-weight:600; color:#F7A707;'>Olá, " + nome + @"</p>
 
                            <p style='font-size: 18px; font-weight: 400;'>
                                Utilize o código abaixo para prosseguir com seu cadastro:
</p>
<p class='codigo' style='    border-radius: 5px; padding: 12px 20px; border: solid 2px #F7A707;
    font-size: 24px; display: inline-block;
    letter-spacing: 2px;'>" + codigoValidacao + @"</p> 
 
                        </td>
</tr>
 
                </table>
</td>
</tr>
 
        <!-- BORDER -->
<tr>
<td height='2' style='background-color: #F7A707;'></td>
</tr>
 
        <!-- FOOTER -->
 
        <tr>
<td style='background-color: #03436e;'>
<table width='100%'>
<tr>
<td style='text-align:center; padding: 45px 20px; color: #ffffff'>
<a href='http://fadami.com.br/pedagiopay' target='_blank'>
<a href='https://pedagiopay.com/img/logo-pedagiopay.png' target='_blank'></a>
 
                            <p style='padding: 10px 0 0 0; font-weight: 400; color: #0b7eb3;'>Simples, rápido e PAY.</p>
 
                        </td>
</tr>
</table>
</td>
</tr>
 
        </table>
 
    </center>
</body>
</html>";

            return corpoEmail;
        }

        public string NewPassWordBody(string tokenConcessao, string link)
        {
            string linkSite = "";
            string base64String = "";
            if (tokenConcessao == "1")
            {
                linkSite = "https://pedagiopay.com/";
                base64String = "https://pedagiopay.com/img/logo-pedagiopay.png";
                Console.WriteLine(base64String);
            }
            if (tokenConcessao == "2")
            {
                linkSite = "https://www.ecoponte.com.br/";
                base64String = "https://pedagiopay.com/img/ecoponte-logo.png";
                Console.WriteLine(base64String);
            }
            if (tokenConcessao == "3")
            {
                linkSite = "https://www.econoroeste.com.br/";
                base64String = "https://pedagiopay.com/img/logo-econoroeste-home.png";
                Console.WriteLine(base64String);
            }
            string corpoEmail = $@"<!DOCTYPE html>
<html lang='pt-br'>
<head>
<meta charset='UTF-8'>
<meta name='viewport' content='width=device-width, initial-scale=1.0'>
<title>Recupere a sua senha</title>

    <link rel='stylesheet' href='./css/style.css' />
</head>
<body>
 
    <center class='wrapper' style='width: 100%;
    table-layout: fixed;
    background-color: #cccccc;
    padding-bottom: 60px;'>
 
        <table class='main' width='100%' style='            background-color: #ffffff;
            margin: 0 auto;
            width: 100%;
            max-width: 600px;
            border-spacing: 0;
            font-family: Arial, ""Helvetica Neue"", Helvetica, sans-serif;
            color: #171a1b;'>
 
 
        <!-- TOP BORDER -->
<tr>
<td height='8' style='background-color: #F7A707;'></td>
</tr>
 
        <!-- LOGO -->
<tr>
<td style='padding: 14px 0 15px;'>
<table width='100%'>
<tr>
<td class='two-columns' style='display:flex'>
<table class='column' style = 'width: 100%;
            max-width: 300px;
            display: inline-block;
            vertical-align: middle;
            text-align: center;'>
<tr>
<td style='padding: 34px 40px 30px;'>
<a href='http://fadami.com.br/pedagiopay' target='_blank'>
<img src='https://pedagiopay.com/img/logo-pedagiopay.png' width='210' alt='PEDAGIOPAY'>
</a>
</td>
</tr>
</table>
<table class='column'>
<tr>
<td style='padding: 10px 75px 15px;'>
<a href='" + link + @"'target='_blank'>
<img src='" + base64String + @"' width='160' alt='LOGO'>
</a>
</td>
</tr>
</table>
</td>
</tr>
</table>
</td>
</tr>
 
        <!-- BANNER -->
<tr>
 
            <td style='background: linear-gradient(340deg, rgb(217, 222, 224) 0%, rgb(248, 248, 248) 100%);color: #03436e;'>
<table width='100%'>
<tr>
<td class='two-columns last' style=' padding: 15px 0;'>
 
                            <table class='column' style='            width: 100%;
            max-width: 300px;
            display: inline-block;
            vertical-align: middle;
            text-align: center;'>
<tr>
<td class='padding' style='padding: 0 40px;'>
 
                                        <table class='content' style='            line-height: 20px;
            text-align: left;'>
<tr>
<td>
<p style='font-size:34px; line-height:44px; font-weight: 700; padding-left: 10px;'>Recupere a sua senha</p>
</td>
</tr>
</table>
 
                                    </td>
</tr>
</table>
 
                            <table class='column' style='            width: 100%;
            max-width: 300px;
            display: inline-block;
            vertical-align: middle;
            text-align: center;'>
<tr>
<td class='padding' style='padding: 0 40px;'>
 
<table class='content' style='            line-height: 20px;
            text-align: left;'>
<tr>
<td>
<a href='#'><img src='https://pedagiopay.com/img/senha.png' alt='' width='260' style='max-width:260px;'></a>
</td>
</tr>
</table>
 
                                    </td>
</tr>
</table
 
 
                        </td>
</tr>
</table>
</td>
</tr>
 
        <!-- CONTEÚDO -->
 
        <tr>
<td>
<table width='100%'>
 
                    <tr>
<td style='text-align: center; padding: 35px 15px'>
 
                                <p style='font-size:24px; font-weight:600; color:#F7A707;'>Olá!</p>
 
                            <p style='font-size: 18px; font-weight: 400;'>
                                Utilize o link abaixo para redefinir sua senha:
</p>
<p class='codigo' style='    border-radius: 5px; padding: 12px 20px; border: solid 2px #F7A707;
    font-size: 24px; display: inline-block;
    letter-spacing: 2px;'>" + link + @"</p> 
 
                        </td>
</tr>
 
                </table>
</td>
</tr>
 
        <!-- BORDER -->
<tr>
<td height='2' style='background-color: #F7A707;'></td>
</tr>
 
        <!-- FOOTER -->
 
        <tr>
<td style='background-color: #03436e;'>
<table width='100%'>
<tr>
<td style='text-align:center; padding: 45px 20px; color: #ffffff'>
<a href='http://fadami.com.br/pedagiopay' target='_blank'>
<a href='https://pedagiopay.com/img/logo-pedagiopay.png' target='_blank'></a>
 
                            <p style='padding: 10px 0 0 0; font-weight: 400; color: #0b7eb3;'>Simples, rápido e PAY.</p>
 
                        </td>
</tr>
</table>
</td>
</tr>
 
        </table>
 
    </center>
</body>
</html>";

            return corpoEmail;
        }

        static string ImageToBase64(string imagePath)
        {
            // Verifica se o arquivo existe
            if (File.Exists(imagePath))
            {
                // Ler o arquivo de imagem em um array de bytes
                byte[] imageBytes = File.ReadAllBytes(imagePath);

                // Obter o MIME type com base na extensão do arquivo
                string mimeType = "image/png";

                // Converter o array de bytes em uma string base64 e formatá-la para uso em HTML
                string base64String = $"data:{mimeType};base64,{Convert.ToBase64String(imageBytes)}";

                return base64String;
            }
            else
            {
                return "Arquivo não encontrado.";
            }
        }
    }
}
