namespace SIGRHBack.Dtos.Authorization.Auth
{
    public class OutputLoginDto
    {
        public string IdUser { get; set; }
        public string Name { get; set; }
        public string Profil { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public IEnumerable<object>? Errors { get; set; }
        public string? TypeToken { get; set; }
        public DateTime? ExpiresToken { get; set; }
        public string? EmailUser { get; set; }
        public string? NomUtilisateur { get; set; }
        public string? PrenomUtilisateur { get; set; }
        public string? avatar { get; set; }
        public IEnumerable<ServiceMetier.TD_MenuItem>? Menus { get; set; }
    }
}
