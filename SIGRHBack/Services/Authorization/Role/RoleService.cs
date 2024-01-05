using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SIGRHBack.Dtos.Authorization.Role;
using SIGRHBack.Dtos.Shared;

namespace SIGRHBack.Services.Authorization.Role
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public RoleService(RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }
        /// <summary>
        /// Cette méthode nous permet d'obtenir la liste des roles.
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseDto<IEnumerable<RoleDto>>> GetAll()
        {
            var result = await _roleManager.Roles.OrderBy(r => r.Id).ToListAsync();
            var resultDto = _mapper.Map<IEnumerable<RoleDto>>(result);
            return new ResponseDto<IEnumerable<RoleDto>>
            {
                CountTotal = result.Count,
                IsSuccess = true,
                Result = resultDto,
                Message = "Récuperation des rôles avec succès !"
            };
        }
        /// <summary>
        /// Cette méthode crée un nouveau role.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<ResponseDto> CreateRole(string name)
        {
            var roleExist = await _roleManager.RoleExistsAsync(name);
            if (roleExist == false)
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                {
                    return new ResponseDto
                    {
                        IsSuccess = true,
                        Message = "Création du role avec succès.",
                    };
                }
                return new ResponseDto
                {
                    Errors = result.Errors,
                    IsSuccess = false,
                    Message = "Erreur de création du rôle."

                };
            }
            return new ResponseDto
            {
                IsSuccess = false,
                Message = "Ce rôle existe déjà dans le système."

            };
        }
        /// <summary>
        /// Cette méthode nous permet de supprimer une role.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseDto> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role is not null)
            {
                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return new ResponseDto
                    {
                        IsSuccess = true,
                        Message = "Suppression de l'utilisateur avec succès."
                    };
                }
                return new ResponseDto
                {
                    IsSuccess = false,
                    Message = "Erreur suppression de role.",
                    Errors = result.Errors
                };
            }
            return new ResponseDto
            {
                IsSuccess = false,
                Message = "Erreur suppression : Role introuvable"
            };
        }
        /// <summary>
        /// Cette méthode nous permet de mettre à jour un role.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<ResponseDto> UpdateRole(string id, string name)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role is not null)
            {
                role.Name = name;
                role.NormalizedName = name.ToUpper();
                var result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return new ResponseDto
                    {
                        IsSuccess = false,
                        Message = "Suppression de l'utilisateur avec succès."
                    };
                }
            }
            return new ResponseDto
            {
                IsSuccess = false,
                Message = "Erreur de modfication, Ce role est introuvable."
            };
        }
        /// <summary>
        /// Cette méthode nous permet de récuperer un role par son Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseDto<RoleDto>>? GetOne(string id)
        {
            var result = await _roleManager.FindByIdAsync(id);
            if (result is not null)
            {
                return new ResponseDto<RoleDto>
                {
                    IsSuccess = true,
                    Result = _mapper.Map<RoleDto>(result),
                };
            }
            return null;
        }
    }
}
