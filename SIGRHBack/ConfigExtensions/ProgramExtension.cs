using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SIGRHBack.Database;
using SIGRHBack.Database.Shared;
using SIGRHBack.Helpers;
using SIGRHBack.Models;

namespace SIGRHBack.ConfigExtensions
{
    public static class ProgramExtension
    {
        public static async void SeedUserRoles(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            ServiceMetier.Service1Client service = new ServiceMetier.Service1Client();


        // update migration automaticcaly on database
        var db = scope.ServiceProvider.GetRequiredService<SIGRHBackDbContext>().Database;
            db.Migrate();
            if (!db.EnsureCreated())
            {
                string defaultPassword = "P@ssword1234";
                //seed roles
                var roles = new List<IdentityRole>()
                    {
                    new IdentityRole
                    {
                            Name = UserRoleNames.Admin,
                            ConcurrencyStamp = "1",
                            NormalizedName = UserRoleNames.Admin.ToUpper()
                    },
                    new IdentityRole
                    {
                            Name = UserRoleNames.DirecteurServicesJudiciaires,
                            ConcurrencyStamp = "2",
                            NormalizedName = UserRoleNames.DirecteurServicesJudiciaires.ToUpper()
                    },
                    new IdentityRole
                    {
                            Name = UserRoleNames.DirecteurAssistantServicesJudiciaires,
                            ConcurrencyStamp = "3",
                            NormalizedName = UserRoleNames.DirecteurAssistantServicesJudiciaires.ToUpper()
                    },
                    new IdentityRole
                    {
                            Name = UserRoleNames.SecretaireGeneralMinistereJustice,
                            ConcurrencyStamp = "4",
                            NormalizedName = UserRoleNames.SecretaireGeneralMinistereJustice.ToUpper()
                    },
                    new IdentityRole
                    {
                            Name = UserRoleNames.OperateurSaisie,
                            ConcurrencyStamp = "5",
                            NormalizedName = UserRoleNames.OperateurSaisie.ToUpper()
                    },
                    new IdentityRole
                    {
                            Name = UserRoleNames.Revoquer,
                            ConcurrencyStamp = "6",
                            NormalizedName = UserRoleNames.Revoquer.ToUpper()
                    },
                    new IdentityRole
                    {
                            Name = UserRoleNames.ChefPersonnel,
                            ConcurrencyStamp = "7",
                            NormalizedName = UserRoleNames.ChefPersonnel.ToUpper()
                    },
                    };
                foreach (var item in roles)
                {
                    var roleExist = await roleManager.FindByNameAsync(item.Name);
                    if (roleExist == null)
                    {
                        await roleManager.CreateAsync(item);
                        Console.WriteLine("Created Role", ConsoleColor.Green);
                    }
                }
                var userAdminExist = await userManager.FindByEmailAsync(AppConsts.EmailAdmin);
                if (userAdminExist == null)
                {
                    // admin user
                    AppUser adminUser = new AppUser
                    {
                        UserName = AppConsts.UsernameAdmin,
                        NormalizedUserName = AppConsts.UsernameAdmin.ToUpper(),
                        Email = AppConsts.EmailAdmin,
                        NormalizedEmail = AppConsts.EmailAdmin.ToUpper(),
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        SecurityStamp = Guid.NewGuid().ToString("D"),
                    };
                    await userManager.CreateAsync(adminUser, defaultPassword);
                    await userManager.AddToRoleAsync(adminUser, UserRoleNames.Admin);
                };
                //insert user in td_utilisateur
                var userTdExist = await service.GetOneUserByLoginAsync(AppConsts.UsernameAdmin);
                if(userTdExist == null)
                {
                    ServiceMetier.TD_Utilisateur tdUser = new ServiceMetier.TD_Utilisateur();
                    tdUser.Uti_Email = AppConsts.EmailAdmin;
                    tdUser.Uti_Login = AppConsts.UsernameAdmin;
                    tdUser.Uti_Poste = "Admin";
                    tdUser.Uti_Adresse = "SIGRH";
                    tdUser.Uti_Telephone = "778976540";
                    tdUser.Uti_Nom = "Admin";
                    tdUser.Uti_Prenom= "SIGRH";
                    tdUser.Uti_DateCreation = DateTime.Now;
                    tdUser.Uti_idUser = userAdminExist.Id;
                    tdUser.Uti_Pro_Code = UserRoleNames.Admin;
                    tdUser.Uti_ActifOuiNon = "1";
                    service.AddUser(tdUser);
                }
            }

        }
    }
}
