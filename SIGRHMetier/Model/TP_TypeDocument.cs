//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré à partir d'un modèle.
//
//     Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//     Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SIGRHMetier.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class TP_TypeDocument
    {
        public int TyD_Id { get; set; }
        public string TyD_Libelle { get; set; }
        public string TyD_MagistratOuiNon { get; set; }
        public string TyD_FonctionnaireJudiciaireOuiNon { get; set; }
        public string TyD_TyA_Code { get; set; }
        public int TyD_Echeance { get; set; }
        public Nullable<int> TyD_AlerteRouge { get; set; }
        public Nullable<int> TyD_AlerteJaune { get; set; }
        public string TyD_Mouvement { get; set; }
        public string TyD_Code { get; set; }
    }
}
