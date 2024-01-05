using SIGRHBack.Models;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace SIGRHBack.Services.Messagerie.Mail
{
    public interface IMailService
    {
        public SmtpClient client { get; set; }
        public string Key { get; set; }
        public string EmailFrom { get; set; }
        public string SmtpHost { get; set; }
        public string SmtpPort { get; set; }
        public string SmtpUser { get; set; }
        public string SmtpPass { get; set; }
        public bool EnableSsl { get; set; }
        public bool UseDefaultCredentials { get; set; }
        /// <summary>
        /// Envoyer un mail de réinitialisation de mot de passe à un utilisateur.
        /// </summary>
        /// <param name="to">l'adresse électronique du destinataire</param>
        /// <param name="token">le token de réinitialisation de mot de passe</param>
        SendMail? SendMailResetPassword([EmailAddress] string to, string token);
    }
}