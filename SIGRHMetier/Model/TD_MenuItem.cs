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
    
    public partial class TD_MenuItem
    {
        public int Men_Id { get; set; }
        public string Men_Libelle { get; set; }
        public string Men_Route { get; set; }
        public string Men_Icon { get; set; }
        public Nullable<int> Men_Priorite { get; set; }
        public string Men_CodeProfil { get; set; }
        public Nullable<int> Men_HaveSubMenu { get; set; }
    }
}