namespace SIGRHBack.Helpers
{
    public static class AppConsts
    {
        // admin user
        public const string EmailAdmin = "admin@sigrh.sn";
        public const string UsernameAdmin = "adminsigrh";
        // app
        public const string AppUrl = "http://localhost:4200/";
        // user
        public const string UrlResetPassword = AppUrl + "auth/reset-password?email={0}&token={1}";
    }
}
