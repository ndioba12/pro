using SIGRHMetier.Application.Filters;

namespace SIGRHMetier
{
    public class GetAllCommuneFilter : FilterCommun
    {
        public string CodeDepartement { get; set; } = string.Empty;
    }
}