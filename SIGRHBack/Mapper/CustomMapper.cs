using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SIGRHBack.Database.Shared;
using SIGRHBack.Dtos.ActeAdministration;
using SIGRHBack.Dtos.ActeGestion;
using SIGRHBack.Dtos.Authorization.Role;
using SIGRHBack.Dtos.Authorization.User;
using SIGRHBack.Dtos.Personnel;
using SIGRHBack.Models;

namespace SIGRHBack.Mapper
{
    public class CustomMapper : Profile
    {
        public CustomMapper()
        {

            //--------------User Mapper---------------------

            CreateMap<RoleDto, IdentityRole>().ReverseMap();
            CreateMap<IdentityUser, AppUser>().ReverseMap();
            CreateMap<AppUser, OutputUserDto>().ReverseMap();
            CreateMap<OutputUserDto, IdentityUser>().ReverseMap();
            CreateMap<InputUserDto, AppUser>()
                .ForMember(destinationMember => destinationMember.UserName, src => src.MapFrom(s => s.Identifiant))
                .ForMember(destinationMember => destinationMember.Email, src => src.MapFrom(s => s.Email))
                .ReverseMap();
            CreateMap<InputUserDto, ServiceMetier.TD_Utilisateur>()
                .ForMember(destinationMember => destinationMember.Uti_Login, src => src.MapFrom(s => s.Identifiant))
                .ForMember(destinationMember => destinationMember.Uti_Email, src => src.MapFrom(s => s.Email))
                .ForMember(destinationMember => destinationMember.Uti_Prenom, src => src.MapFrom(s => s.Email))
                .ForMember(destinationMember => destinationMember.Uti_Nom, src => src.MapFrom(s => s.Nom))
                .ForMember(destinationMember => destinationMember.Uti_Adresse, src => src.MapFrom(s => s.Adresse))
                .ForMember(destinationMember => destinationMember.Uti_Poste, src => src.MapFrom(s => s.Poste))
                .ForMember(destinationMember => destinationMember.Uti_Telephone, src => src.MapFrom(s => s.Telephone))
                .ForMember(destinationMember => destinationMember.Uti_ActifOuiNon, src => src.MapFrom(s => s.ActifOuiNon))
                .ForMember(destinationMember => destinationMember.Uti_DateCreation, src => src.MapFrom(s => s.DateCreation))
                .ReverseMap();
            CreateMap<InputUpdatedUserDto, ServiceMetier.TD_Utilisateur>()
                .ForMember(destinationMember => destinationMember.Uti_Prenom, src => src.MapFrom(s => s.PrenomUtilisateur))
                .ForMember(destinationMember => destinationMember.Uti_Nom, src => src.MapFrom(s => s.NomUtilisateur))
                .ForMember(destinationMember => destinationMember.Uti_Adresse, src => src.MapFrom(s => s.Adresse))
                .ForMember(destinationMember => destinationMember.Uti_Poste, src => src.MapFrom(s => s.Poste))
                .ForMember(destinationMember => destinationMember.Uti_Telephone, src => src.MapFrom(s => s.Telephone))
                .ForMember(destinationMember => destinationMember.Uti_Pro_Code, src => src.MapFrom(s => s.Profil))
                .ReverseMap();
            CreateMap<InputUpdatedAccountDto, ServiceMetier.TD_Utilisateur>()
                .ForMember(destinationMember => destinationMember.Uti_Prenom, src => src.MapFrom(s => s.PrenomUtilisateur))
                .ForMember(destinationMember => destinationMember.Uti_Nom, src => src.MapFrom(s => s.NomUtilisateur))
                .ForMember(destinationMember => destinationMember.Uti_Adresse, src => src.MapFrom(s => s.Adresse))
                .ForMember(destinationMember => destinationMember.Uti_Poste, src => src.MapFrom(s => s.Poste))
                .ForMember(destinationMember => destinationMember.Uti_Telephone, src => src.MapFrom(s => s.Telephone))
                .ReverseMap();
            CreateMap<OutputUserDto, ServiceMetier.TD_Utilisateur>()
                .ForMember(destinationMember => destinationMember.Uti_Prenom, src => src.MapFrom(s => s.PrenomUtilisateur))
                .ForMember(destinationMember => destinationMember.Uti_Nom, src => src.MapFrom(s => s.NomUtilisateur))
                .ForMember(destinationMember => destinationMember.Uti_Adresse, src => src.MapFrom(s => s.Adresse))
                .ForMember(destinationMember => destinationMember.Uti_Poste, src => src.MapFrom(s => s.Poste))
                .ForMember(destinationMember => destinationMember.Uti_Telephone, src => src.MapFrom(s => s.Telephone))
                .ForMember(destinationMember => destinationMember.Uti_idUser, src => src.MapFrom(s => s.IdUtilisateur))
                .ForMember(destinationMember => destinationMember.Uti_Email, src => src.MapFrom(s => s.EmailUtilisateur))
                .ForMember(destinationMember => destinationMember.Uti_Login, src => src.MapFrom(s => s.Identifiant))
                .ForMember(destinationMember => destinationMember.Uti_Pro_Code, src => src.MapFrom(s => s.CodeProfil))
                .ForMember(destinationMember => destinationMember.Uti_ActifOuiNon, src => src.MapFrom(s => s.ActifOuiNon))
                .ForMember(destinationMember => destinationMember.Uti_DateCreation, src => src.MapFrom(s => s.DateCreation))
                .ReverseMap();
            CreateMap<InputFonctionnaireDto, ServiceMetier.TD_FichePersonnelJudiciaire>()
                .ForMember(destinationMember => destinationMember.FPJ_Nom, src => src.MapFrom(s => s.Nom))
                .ForMember(destinationMember => destinationMember.FPJ_Prenom, src => src.MapFrom(s => s.Prenom))
                .ForMember(destinationMember => destinationMember.FPJ_Email, src => src.MapFrom(s => s.Email))
                .ForMember(destinationMember => destinationMember.FPJ_DateDeNaissance, src => src.MapFrom(s => s.DateNaissance))
                .ForMember(destinationMember => destinationMember.FPJ_LieuDeNaissance, src => src.MapFrom(s => s.LieuNaissance))
                .ForMember(destinationMember => destinationMember.FPJ_Sexe, src => src.MapFrom(s => s.Sexe))
                .ForMember(destinationMember => destinationMember.FPJ_SituationMatrimoniale, src => src.MapFrom(s => s.SituationMatrimoniale))
                .ForMember(destinationMember => destinationMember.FPJ_NumeroCNI, src => src.MapFrom(s => s.Cni))
                .ForMember(destinationMember => destinationMember.FPJ_SIP_Id, src => src.MapFrom(s => s.IdPhoto))
                .ForMember(destinationMember => destinationMember.FPJ_Promotion, src => src.MapFrom(s => s.Promotion))
                .ForMember(destinationMember => destinationMember.FPJ_Com_Code, src => src.MapFrom(s => s.commune))
                .ForMember(destinationMember => destinationMember.FPJ_Qua_Code, src => src.MapFrom(s => s.Quartier))
                .ForMember(destinationMember => destinationMember.FPJ_Dep_Code, src => src.MapFrom(s => s.Departement))
                .ForMember(destinationMember => destinationMember.FPJ_Telephone, src => src.MapFrom(s => s.Telephone))
                .ForMember(destinationMember => destinationMember.FPJ_Matricule, src => src.MapFrom(s => s.MatriculeSolde))
                .ForMember(destinationMember => destinationMember.FPJ_Jur_Fon_Code, src => src.MapFrom(s => s.JuridictionF))
                .ForMember(destinationMember => destinationMember.FPJ_TyP_Code, src => src.MapFrom(s => s.TypePersonnel))
                .ForMember(destinationMember => destinationMember.FPJ_Fon_Code, src => src.MapFrom(s => s.Fonction))
                .ForMember(destinationMember => destinationMember.FPJ_Ech_Code, src => src.MapFrom(s => s.Echelon))
                .ForMember(destinationMember => destinationMember.FPJ_Grf_Code, src => src.MapFrom(s => s.Grade))
                .ForMember(destinationMember => destinationMember.FPJ_Cju_Code, src => src.MapFrom(s => s.Corps))
                .ForMember(destinationMember => destinationMember.FPJ_Reg_Code, src => src.MapFrom(s => s.Region))
                .ForMember(destinationMember => destinationMember.FPJ_Jur__Reg_Code, src => src.MapFrom(s => s.RegionJ))
                .ForMember(destinationMember => destinationMember.FPJ_ConjointMagistratOuiNon, src => src.MapFrom(s => s.ConjointMagistratOuiNON))
                .ForMember(destinationMember => destinationMember.FPJ_MatriculeConjoint, src => src.MapFrom(s => s.MatriculeConjoint))
                .ForMember(destinationMember => destinationMember.FPJ_MatriculeConjoint, src => src.MapFrom(s => s.MatriculeConjoint))
                .ForMember(destinationMember => destinationMember.FPJ_Uti_Creation_Id, src => src.MapFrom(s => s.UserCreation))
                .ForMember(destinationMember => destinationMember.FPJ_Positon, src => src.MapFrom(s => s.Position))
                .ReverseMap();
            CreateMap<OutputUserDto, ServiceMetier.UserViewModel>()
               .ForMember(destinationMember => destinationMember.Prenom, src => src.MapFrom(s => s.PrenomUtilisateur))
               .ForMember(destinationMember => destinationMember.Nom, src => src.MapFrom(s => s.NomUtilisateur))
               .ForMember(destinationMember => destinationMember.IdUser, src => src.MapFrom(s => s.IdUtilisateur))
               .ForMember(destinationMember => destinationMember.Email, src => src.MapFrom(s => s.EmailUtilisateur))
               .ForMember(destinationMember => destinationMember.Login, src => src.MapFrom(s => s.Identifiant))
               .ForMember(destinationMember => destinationMember.CodeProfil, src => src.MapFrom(s => s.CodeProfil))
               .ForMember(destinationMember => destinationMember.LibelleProfil, src => src.MapFrom(s => s.LibelleProfil))
               .ForMember(destinationMember => destinationMember.DateCreation, src => src.MapFrom(s => s.DateCreation))
               .ReverseMap();

            //--------------Acte Gestion Mapper---------------------
            CreateMap<ActeGestionConsultationDto, ServiceMetier.ActeGestionConsultationViewModel>().ReverseMap();
            CreateMap<ActeGestionModificationDto, ServiceMetier.ActeGestionModificationViewModel>().ReverseMap();
            CreateMap<ActeGestionCreationDto, ServiceMetier.ActeGestionCreationViewModel>().ReverseMap();
            CreateMap<AddOrUpdateActeDto, ServiceMetier.ActeAdminViewModel>().ReverseMap();

            //Mapper for AccteAdministration
            /*     CreateMap<AddOrUpdateActeDto, ServiceMetier.ActeAdminViewModel>()
                   .ForMember(destinationMember => destinationMember.Residence, src => src.MapFrom(s => s.Residence))
                   .ForMember(destinationMember => destinationMember.CodeEchellon, src => src.MapFrom(s => s.CodeEchelon))
                   .ForMember(destinationMember => destinationMember.CodeEmploi, src => src.MapFrom(s => s.CodeEmploiJudiciaire))
                   .ForMember(destinationMember => destinationMember.CodeFonction, src => src.MapFrom(s => s.CodeFonctionnaireJustice))
                   .ForMember(destinationMember => destinationMember.CodeJuridictionEmploi, src => src.MapFrom(s => s.CodeJurEmploi))
                   .ForMember(destinationMember => destinationMember.CodeJuridictionFonction, src => src.MapFrom(s => s.CodeJurFonction))
                   .ForMember(destinationMember => destinationMember.NumeroDecision, src => src.MapFrom(s => s.NumeroDecision))
                   .ForMember(destinationMember => destinationMember.DateCreation, src => src.MapFrom(s => s.DateCreation))
                   .ForMember(destinationMember => destinationMember.CodeNatureDecision, src => src.MapFrom(s => s.CodeNaturedecision))
                   .ForMember(destinationMember => destinationMember.CodeTypeDocument, src => src.MapFrom(s => s.CodeTypeDocument))
                   .ForMember(destinationMember => destinationMember.IdFichier, src => src.MapFrom(s => s.IdFichier))
                   .ForMember(destinationMember => destinationMember.IdFichePersonnel, src => src.MapFrom(s => s.IdFichePersonnelJudiciaire))
                   .ForMember(destinationMember => destinationMember.IdUtilisateurCreation, src => src.MapFrom(s => s.IdUtiCreation))
                   .ForMember(destinationMember => destinationMember.CodeGroupe, src => src.MapFrom(s => s.CodeGroupe))
                   .ForMember(destinationMember => destinationMember.CodeGradeM, src => src.MapFrom(s => s.CodeGradeMagistrat))
                   .ForMember(destinationMember => destinationMember.CodeGradeF, src => src.MapFrom(s => s.CodeGradeFonctionnaire))
                   .ForMember(destinationMember => destinationMember.CodeEchellon, src => src.MapFrom(s => s.CodeEchelon))
                   .ForMember(destinationMember => destinationMember.ValeurIndice, src => src.MapFrom(s => s.ValeurIndice))
                   .ForMember(destinationMember => destinationMember.DateSoumission, src => src.MapFrom(s => s.DateSoumission))
                   .ReverseMap();*/
        }
    }
}