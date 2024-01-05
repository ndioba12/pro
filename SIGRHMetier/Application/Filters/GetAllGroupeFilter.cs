using SIGRHMetier.Application.Filters;

namespace SIGRHMetier
{

    public class GetAllGroupeFilter : FilterCommun
    {
        public string CodeGradeMagistrat {  get; set; } = string.Empty;
    }
}