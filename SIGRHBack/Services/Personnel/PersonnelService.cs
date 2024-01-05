using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SIGRHBack.Dtos.Authorization.User;
using SIGRHBack.Dtos.Personnel;
using SIGRHBack.Dtos.Shared;
using SIGRHBack.Models;
using SIGRHBack.Ressources;
using System.Xml.Linq;

namespace SIGRHBack.Services.Personnel
{
	public class PersonnelService : IPersonnelService
	{
		private readonly IMapper mapper;

        public PersonnelService(IMapper mapper)
        {
            this.mapper = mapper;
        }

        private ServiceMetier.Service1Client service = new ServiceMetier.Service1Client();

		public async Task<ResponseDto<IEnumerable<ServiceMetier.RegistreViewModel>>> GetRegistrePersonnel(ServiceMetier.GetAllRegistreFilter filter)
		{
			var liste = await service.RegistrePersonnelAsync(filter);
			return new ResponseDto<IEnumerable<ServiceMetier.RegistreViewModel>>
			{
				Errors = null,
				IsSuccess = true,
				Message = Messages.GetDatasSuccess.ToString(),
				CountTotal = liste.Count(),
				Result = liste
			};
			
		}

		public async Task<ResponseDto>? AddMagistrat(ServiceMetier.PersonnelViewModel magistrat)
		{
		
			try
			{
				var result = await service.AddFicheMagistratAsync(magistrat);
				if (result)
				{
					return new ResponseDto
					{
						IsSuccess = true,
						Message = "Fiche créée avec succès.",
					};
				}
				return new ResponseDto
				{
					//Errors = result.Errors,
					IsSuccess = false,
					Message = "Erreur de création ."

				};
			}
			catch (Exception ex)
			{
				
				return null;
			}
		}

        public async Task<ResponseDto>? AddFonctionnaire(InputFonctionnaireDto fonctionnaire)
        {
            try
            {
				var personnel = mapper.Map<ServiceMetier.TD_FichePersonnelJudiciaire>(fonctionnaire);
                var result = await service.AddFicheFonctionnaireAsync(personnel);
                if (result)
                {
                    return new ResponseDto
                    {
                        IsSuccess = true,
                        Message = "Fiche personnelle créée avec succès.",
                    };
                }
                return new ResponseDto
                {
                    IsSuccess = false,
                    Message = "Erreur de création de la fiche personnelle ."
                };
            }
            catch (Exception ex)
            {

                return null;
            }
        }
    }
}
