using SIGRHMetier.Application.Filters;

namespace SIGRHMetier
{
    public class GetAllQuartierFilter : FilterCommun
    {
        public string CodeCommune { get; set; } = string.Empty;
    }
}