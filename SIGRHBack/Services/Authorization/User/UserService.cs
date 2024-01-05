using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SIGRHBack.Database;
using SIGRHBack.Database.Shared;
using SIGRHBack.Dtos.Authorization.Role;
using SIGRHBack.Dtos.Authorization.User;
using SIGRHBack.Dtos.Shared;
using SIGRHBack.Models;
using SIGRHBack.Ressources;
using SIGRHBack.Services.Messagerie.Mail;

namespace SIGRHBack.Services.Authorization.User
{
    public class UserService : IUserService
    {
        private readonly SIGRHBackDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMailService _mailService;
        private readonly IMapper _mapper;
        private ServiceMetier.Service1Client service = new ServiceMetier.Service1Client();


        //constructor
        public UserService(UserManager<AppUser> userManager, IMapper mapper, RoleManager<IdentityRole> roleManager, SIGRHBackDbContext context)
        {
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
            _context = context;
        }
        public async Task<ResponseDto?> ActiverOuDesactiverUser(string userId)
        {
            var result = service.ActiverOuDesactiverUser(userId);
            if(result != null)
            {
                return new ResponseDto
                {
                    IsSuccess = true,
                    Message = result.Uti_ActifOuiNon == "0" ? Messages.DesactiverUserSuccess : Messages.ActiverUserSuccess
                };
            };
            return new ResponseDto
            {
                IsSuccess = false,
                Message = Messages.OperationError
            };
        }
        public async Task<ResponseDto> CreateUser(InputUserDto item)
        {
            try
            {
                string defaultPassword = "P@ssword1234";

                // verif doublons profils/utilisateurs
                    // admin
                    if (item.Profil.Equals(UserRoleNames.Admin, StringComparison.OrdinalIgnoreCase))
                        return new ResponseDto { Message = String.Format(Messages.UserRoleNot, item.Profil), IsSuccess = false };
                // chef personnel
                if (
                item.Profil.Equals(UserRoleNames.ChefPersonnel, StringComparison.OrdinalIgnoreCase)
                &&
                await service.VerifUserByProfilAsync(UserRoleNames.ChefPersonnel) == true
                )
                    return new ResponseDto { Message = String.Format(Messages.UserRoleNot, item.Profil), IsSuccess = false };

                // directeur services judiciaires
                if (
                    item.Profil.Equals(UserRoleNames.DirecteurServicesJudiciaires, StringComparison.OrdinalIgnoreCase)
                    &&
                    await service.VerifUserByProfilAsync(UserRoleNames.DirecteurServicesJudiciaires) == true
                )
                    return new ResponseDto { Message = String.Format(Messages.UserRoleNot, item.Profil), IsSuccess = false };

                // directeur assistant services judicaires
                if (
                item.Profil.Equals(UserRoleNames.DirecteurAssistantServicesJudiciaires, StringComparison.OrdinalIgnoreCase)
                &&
                await service.VerifUserByProfilAsync(UserRoleNames.DirecteurAssistantServicesJudiciaires) == true
                )
                    return new ResponseDto { Message = String.Format(Messages.UserRoleNot, item.Profil), IsSuccess = false };

                // secretaire generale ministere de la justice
                if (
                item.Profil.Equals(UserRoleNames.SecretaireGeneralMinistereJustice, StringComparison.OrdinalIgnoreCase)
                &&
                await service.VerifUserByProfilAsync(UserRoleNames.SecretaireGeneralMinistereJustice) == true
                )
                return new ResponseDto { Message = String.Format(Messages.UserRoleNot, item.Profil), IsSuccess = false };

                var user = _mapper.Map<AppUser>(item);
                var result = await _userManager.CreateAsync(user, defaultPassword);
                if (result.Succeeded)
                {
                    var profil = await service.GetProfilByCodeAsync(item.Profil);
                    // update other table complement user (TD_Utilisateur)
                    var tdUser = _mapper.Map<ServiceMetier.TD_Utilisateur>(item);
                    tdUser.Uti_Pro_Code = item.Profil;
                    tdUser.Uti_idUser = user.Id;
                    var resultTduser = service.AddUser(tdUser);
                    if (resultTduser == false)
                    {
                        await _userManager.DeleteAsync(user);
                        return new ResponseDto
                        {
                            Result = resultTduser,
                            IsSuccess = false,
                            Message = Messages.ErrorCreationUser
                        };
                    }
                    await _userManager.AddToRoleAsync(user, item.Profil);
                    return new ResponseDto
                    {
                        Result = result,
                        IsSuccess = true,
                        Message = Messages.CreatedUserSuccess.ToString(),
                    };
                }
                else
                {
                    var response = new ResponseDto();
                    response.IsSuccess = false;
                    response.Errors = result.Errors.Select(e => e.Description);
                    if (result.Errors.Any(e => e.Code == "DuplicateUserName"))
                    {
                        response.Message = string.Format(Messages.ErrorDuplicateUserName.ToString(), user.UserName);
                    }
                    else if (result.Errors.Any(e => e.Code == "InvalidUserName"))
                    {
                        response.Message = string.Format(Messages.InvalidUserName.ToString(), user.UserName);
                    }
                    else if (result.Errors.Any(e => e.Code == "DuplicateEmail"))
                    {
                        response.Message = string.Format(Messages.DuplicateEmail.ToString(), user.Email);
                    }
                    else
                    {
                        response.Message = Messages.ErrorCreationUser.ToString();
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                // save erreur
                //var erreur = new InputTdErreurDto()
                //{
                //    Titre = "CreateUser",
                //    NomClasse = "AppUser",
                //    DescriptionErreur = ex.Message,
                //};
                //tdErreurService.AddOne(erreur);
                return null;
            }
        }
        /// <summary>
        /// Supprimer un utilisateur.
        /// </summary>
        /// <param name="userId">identifiant utilisateur</param>
        /// <returns></returns>
        public async Task<bool> DeleteUser(string userId)
        {
            var user = await GetById(userId);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                return result.Succeeded;
            }
            return false;
        }
        public async Task<ResponseDto<IEnumerable<ServiceMetier.UserViewModel>>> GetAllUser(ServiceMetier.GetAllUserFilter filter)
        {
            var allUsers = service.GetAllUsers(filter);
            return new ResponseDto<IEnumerable<ServiceMetier.UserViewModel>>
            {
                Errors = null,
                IsSuccess = true,
                Message = Messages.GetDatasSuccess.ToString(),
                CountTotal = allUsers.Count(),
                Result = allUsers
            };
        }
        public async Task<ResponseDto<ServiceMetier.UserViewModel>> GetUserByEmail(string email)
        {
            var user = await service.GetUserByEmailAsync(email);
            if(user != null)
            {
                return new ResponseDto<ServiceMetier.UserViewModel>
                {
                    IsSuccess = true,
                    Message = Messages.GetOneUserSuccess,
                    Result = user
                };
            }
            return new ResponseDto<ServiceMetier.UserViewModel>
            {
                IsSuccess = false,
                Message = Messages.ErrorGetOneUser,
                Result = user
            };
        }
        public async Task<ResponseDto<OutputUserDto>> GetUserById(string userId)
        {
            var user = await service.GetOneUserViewByParentAsync(userId);
            if (user != null)
            {
                return new ResponseDto<OutputUserDto>
                {
                    IsSuccess = true,
                    Message = Messages.GetOneUserSuccess,
                    Result = _mapper.Map<OutputUserDto>(user)
                };
            }
            return new ResponseDto<OutputUserDto>
            {
                IsSuccess = false,
                Message = Messages.ErrorGetOneUser,
                Result = _mapper.Map<OutputUserDto>(user)
            };
        }

        public async Task<ResponseDto>? UpdateUser<T>(string userId, T user) where T : InputUpdatedAccountDto
        {
            ServiceMetier.TD_Utilisateur tdUser = new();
            _mapper.Map(user, tdUser);
            tdUser.Uti_idUser = userId;
            var result = await service.UpdateUserByParentAsync(tdUser.Uti_idUser, tdUser);
            if (result == true)
            {
                return new ResponseDto
                {
                    IsSuccess = true,
                    Message = Messages.EditedUserSuccess.ToString(),
                    Result = _mapper.Map<OutputUserDto>(tdUser)
                };
            }
            return new ResponseDto
            {
                IsSuccess = false,
                Message = Messages.ErrorEditedUser.ToString()
            };
        }

        public async Task<ResponseDto> AssignToRole(string id, RoleDto role)
        {
            var user = await GetById(id);
            if (user is null)
            {
                return new ResponseDto
                {
                    IsSuccess = false,
                    Message = Messages.ErrorAssignRoleUserNotFound
                };
            }
            // assign role to user
            var result = await _userManager.AddToRoleAsync(user, role.Name);
            if (result.Succeeded == true)
                return new ResponseDto
                {
                    IsSuccess = true,
                    Message = String.Format(Messages.AssignRoleSuccess, role.Name)
                };
            return new ResponseDto
            {
                IsSuccess = false,
                Message = String.Format(Messages.ErrorAssignRole, role.Name),
                Errors = result.Errors
            };
        }
        public async Task<ResponseDto> RemoveToRole(string id, RoleDto role)
        {
            var user = await GetById(id);
            if (user is null)
            {
                return new ResponseDto
                {
                    IsSuccess = false,
                    Message = Messages.ErrorAssignRoleUserNotFound.ToString()
                };
            }
            var result = await _userManager.RemoveFromRoleAsync(user, role.Name);
            if (result.Succeeded)
                return new ResponseDto
                {
                    IsSuccess = true,
                    Message = string.Format(Messages.DeleteRoleSuccess, user.UserName)
                };
            return new ResponseDto
            {
                IsSuccess = true,
                Message = string.Format(Messages.ErrorDeleteRoleUser, user.UserName),
                Errors = result.Errors
            };
        }
        protected async Task<AppUser> GetById(string userId) => await _userManager.FindByIdAsync(userId);

        public async Task<ResponseDto> ResetPassword(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null && user.Email != "admin@sigrh.sn")
            {
                //bool sendEmail = true;
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                // send email
                var url = "";
                var urlToken = string.Format(url, user.Email, token);
                var resultSendMail = _mailService.SendMailResetPassword(user.Email, urlToken);
                //await _sendmailService.AddOne(resultSendMail);
                return new ResponseDto
                {
                    IsSuccess = true,
                    Message = Messages.ForgotPasswordSuccess.ToString(),
                    Result = token,
                };
            }
            else
            {
                return new ResponseDto
                {
                    IsSuccess = false,
                    Message = string.Format(Messages.ErrorForgotPassword.ToString(), user.Email),
                    Result = null,
                };
            }
        }
        protected async Task<bool> VerifDoublon(string identifiant, string email, string userId)
        {
            var userInit = await GetById(userId);
            // verif username is modified and email
            if ((userInit.UserName != identifiant))
            {
                var user = await _userManager.FindByNameAsync(identifiant);
                if (user != null)
                    return true;
            }
            else
            if ((userInit.Email != email))
            {
                var userExist = await _userManager.FindByEmailAsync(email);
                if (userExist != null)
                    return true;
            }
            return false;
        }

        public async Task<ResponseDto?> UpdatePassword(InputUserUpdatePassword input)
        {
            var user = await _userManager.FindByIdAsync(input?.IdUser);
            if (user != null && user.Email != "admin@sigrh.sn")
            {
                ResponseDto response = new();
                // update password
                var result = await _userManager.ChangePasswordAsync(user, input?.OldPassword, input?.Password);
                if (result.Succeeded)
                {
                    response.IsSuccess = true;
                    response.Message = Messages.UpdatePasswordSuccess.ToString();
                    return response;
                }
                else
                {
                    if (result.Errors.Any(e => e.Code == "PasswordRequiresDigit"))
                    {
                        response.Message = string.Format(Messages.PasswordRequireDigits.ToString(), user.UserName);
                    }
                    else if (result.Errors.Any(e => e.Code == "PasswordRequiresNonAlphanumeric"))
                    {
                        response.Message = string.Format(Messages.PasswordRequiresNonAlphanumeric.ToString(), user.UserName);
                    }
                    else if (result.Errors.Any(e => e.Code == "PasswordMismatch"))
                    {
                        response.Message = string.Format(Messages.PasswordMismatch.ToString(), user.UserName);
                    }
                    else if (result.Errors.Any(e => e.Code == "PasswordRequiresUpper"))
                    {
                        response.Message = string.Format(Messages.PasswordRequiresUpper.ToString(), user.Email);
                    }
                    else if (result.Errors.Any(e => e.Code == "PasswordRequiresLower"))
                    {
                        response.Message = string.Format(Messages.PasswordRequiresLower.ToString(), user.Email);
                    }
                    else
                    {
                        response.Message = Messages.ErrorResetPassword.ToString();
                    }
                    response.IsSuccess = false;
                    response.Result = result;
                    return response;
                }
            }
            return null;
        }

    }
}
