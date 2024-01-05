using SIGRHBack.Models;

namespace SIGRHBack.Services.SendMails
{
    public interface ISendMailService
    {
        /// <summary>
        /// Ajouter un mail de d'audition et de surveillance.
        /// </summary>
        /// <param name="item">Audition mail</param>
        /// <returns></returns>
        Task<SendMail> AddOne(SendMail item);
    }
}