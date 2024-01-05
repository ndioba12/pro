﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGRHMetier.Application.ViewModel
{
    public class FichePersonnelViewModel
    {
        public int FPJ_Id { get; set; }
        public string FPJ_Nom { get; set; }
        public string FPJ_PhotoContent { get; set; }
        public string FPJ_PhotoType { get; set; }
        public string FPJ_Prenom { get; set; }
        public Nullable<System.DateTime> FPJ_DateDeNaissance { get; set; }
        public string FPJ_LieuDeNaissance { get; set; }
        public string FPJ_Sexe { get; set; }
        public string FPJ_SituationMatrimoniale { get; set; }
        public string FPJ_NumeroCNI { get; set; }
        public string FPJ_Promotion { get; set; }
        public Nullable<System.DateTime> FPJ_DateEntreeEnFonction { get; set; }
        public Nullable<int> FPJ_RangExamen { get; set; }
        public string FPJ_Com_Code { get; set; }
        public string FPJ_Qua_Code { get; set; }
        public string FPJ_Telephone { get; set; }
        public string FPJ_Email { get; set; }
        public string FPJ_Matricule { get; set; }
        public string FPJ_Emp_Code { get; set; }
        public string FPJ_Jur_Fon_Code { get; set; }
        public Nullable<int> FPJ_SIP_Id { get; set; }
        public string FPJ_Fon_Code { get; set; }
        public string FPJ_Ech_Code { get; set; }
        public Nullable<int> FPJ_ConjointMagistratOuiNon { get; set; }
        public string FPJ_NomConjoint { get; set; }
        public string FPJ_PrenomConjoint { get; set; }
        public string FPJ_MatriculeConjoint { get; set; }
        public System.DateTime FPJ_DateCreationFiche { get; set; }
        public string FPJ_CarteIdentiteMagistrat { get; set; }
        public int FPJ_Uti_Creation_Id { get; set; }
        public string FPJ_TyP_Code { get; set; }
        public Nullable<System.DateTime> FPJ_DateValidation { get; set; }
        public Nullable<System.DateTime> FPJ_DateSoumission { get; set; }
        public string FPJ_Jur_Emp_Code { get; set; }
        public Nullable<int> FPJ_Uti_Validation_Id { get; set; }
        public string FPJ_Positon { get; set; }
        public int FPJ_EstValide { get; set; }
        public Nullable<long> FPJ_Reference { get; set; }
        public string FPJ_Motif { get; set; }
        public string FPJ_Gro_Code { get; set; }
        public string FPJ_Grm_Code { get; set; }
        public string FPJ_Grf_Code { get; set; }
        public string FPJ_Cju_Code { get; set; }
        public string FPJ_Dep_Code { get; set; }
        public string FPJ_Reg_Code { get; set; }
        public string FPJ_Jur__Reg_Code { get; set; }
    }
}