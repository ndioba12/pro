using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGRHMetier.Application.ViewModel
{
    public class JuridictionViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Siege { get; set; }
        public string TypeJuridictionCode { get; set; }
        public string TypeJuridictionLibelle { get; set; }
        public string ClasseJuridictionCode { get; set; }
        public string ClasseJuridictionLibelle { get; set; }
        public string ClasseJuridictionAncienLibelle { get; set; }
    }
}