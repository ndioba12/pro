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
    
    public partial class TP_Echelon
    {
        public int Ech_Id { get; set; }
        public string Ech_Libelle { get; set; }
        public string Ech_Code { get; set; }
        public Nullable<int> Ech_Duree { get; set; }
        public string Ech_Typ_Code { get; set; }
        public string Ech_Gro_Code { get; set; }
        public string Ech_Grf_Code { get; set; }
        public string Ech_MoP_Code { get; set; }
        public Nullable<int> Ech_Ind_Id { get; set; }
    }
}
