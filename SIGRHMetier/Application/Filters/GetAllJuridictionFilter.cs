using SIGRHMetier.Application.Filters;

namespace SIGRHMetier
{
    public class GetAllJuridictionFilter 
    {
        public string Siege { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string CodeClasseJuridiction { get; set; } = string.Empty;
        public string CodeTypeJuridiction { get; set; } = string.Empty;
    }
}