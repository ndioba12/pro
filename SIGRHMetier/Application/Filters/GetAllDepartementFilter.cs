using SIGRHMetier.Application.Filters;

namespace SIGRHMetier
{
    public class GetAllDepartementFilter : FilterCommun
    {
        public string CodeRegion { get; set; } = string.Empty;
    }
}