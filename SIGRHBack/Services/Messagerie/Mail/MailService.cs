using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using SIGRHBack.Helpers;
using SIGRHBack.Models;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace SIGRHBack.Services.Messagerie.Mail
{
    public class MailService : IMailService
    { 
    public SmtpClient client { get; set; }
    private readonly IConfiguration _configuration;
    public string Key { get; set; }
    public string EmailFrom { get; set; }
    public string SmtpHost { get; set; }
    public string SmtpPort { get; set; }
    public string SmtpUser { get; set; }
    public string SmtpPass { get; set; }
    public bool EnableSsl { get; set; }
    public bool UseDefaultCredentials { get; set; }

    public MailService(IConfiguration configuration)
    {
        _configuration = configuration;
        getInfos();
        configureSmtp();
    }
    public void configureSmtp()
    {
        this.client = new SmtpClient(SmtpHost);

        client.Port = int.Parse(SmtpPort);
        client.EnableSsl = EnableSsl;
        client.Credentials = new NetworkCredential(SmtpUser, SmtpPass);
        client.UseDefaultCredentials = UseDefaultCredentials;
    }

    public void getInfos()
    {
        Key = this._configuration["AppSettings:Key"];
        EmailFrom = this._configuration["AppSettings:EmailFrom"];
        SmtpHost = this._configuration["AppSettings:SmtpSettings:SmtpHost"];
        SmtpPort = this._configuration["AppSettings:SmtpSettings:SmtpPort"];
        SmtpUser = this._configuration["AppSettings:SmtpSettings:SmtpUser"];
        SmtpPass = this._configuration["AppSettings:SmtpSettings:SmtpPass"];
        EnableSsl = this._configuration["AppSettings:SmtpSettings:EnableSsl"] == "true" ? true : false;
        UseDefaultCredentials = this._configuration["AppSettings:SmtpSettings:UseDefaultCredentials"] == "true" ? true : false;
    }

    public SendMail? SendMailResetPassword([EmailAddress] string to, string token)
    {
        var response = new SendMail();

        string urlToResetPassword = CreateUrlResetPassword(to, token);
        StringBuilder messageBody = new StringBuilder();
        messageBody.Append("<h1>Reinitialisation de mot de passe</h1>");
        messageBody.AppendLine("<p>Bonjour, </p>");
        messageBody.AppendLine("<p>Vous avez demandé la réinitialisation de votre mot de passe.</p>");
        messageBody.AppendLine("<p>Si vous n'êtes pas à l'origine de cette demande, vous pouvez ignorer cet e-mail.</p>");
        messageBody.AppendLine("<p>Si vous êtes à l'origine de cette demande, vous pouvez réinitialiser votre mot de passe en cliquant sur le lien ci-dessous.</p>");
        messageBody.AppendLine($"<p>Merci de cliquez sur le lien suivant : <strong><a href='{urlToResetPassword}'>ici</a> </strong></p>");

        var mailMessage = new MailMessage(EmailFrom, to)
        {
            Subject = "Reinitialisation de mot de passe.",
            Body = messageBody.ToString(),
            IsBodyHtml = true,
            From = new MailAddress(EmailFrom, "SIGRH Justice"),
            Priority = MailPriority.High
        };
        try
        {
            client.SendMailAsync(mailMessage);
            response.ObjetEmail = "Reinitialisation de mot de passe";
            response.StatusEmail = true;
            response.AdresseDestinataire = to;
            response.TextEmail = messageBody.ToString();

        }
        catch (Exception ex)
        {
            //throw;
            response.ObjetEmail = "Reinitialisation de mot de passe";
            response.StatusEmail = false;
            response.AdresseDestinataire = to;
            response.TextEmail = messageBody.ToString();
        }
        return response;
    }

    public string CreateUrlResetPassword(string to, string token)
    {
        string urlToResetPassword = string.Format(AppConsts.UrlResetPassword, to, token);
        return urlToResetPassword;
    }
    }
}
