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
    
    public partial class TR_HistoriqueCarriere1
    {
        public int His_Id { get; set; }
        public int His_Niv_Id { get; set; }
        public int His_FPJ_Id { get; set; }
        public System.DateTime His_DateDebutPassage { get; set; }
        public System.DateTime His_DateFinPrevuePassage { get; set; }
        public Nullable<System.DateTime> His_DateFinPassage { get; set; }
        public int His_DoJ_Id { get; set; }
    }
}
