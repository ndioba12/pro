using SIGRHMetier.Application.Filters;
using System;

namespace SIGRHMetier
{
    public class GetAllIndiceFilter
    {
        public int IdIndice {  get; set; } = default(int);
        public Nullable<long> Valeur {  get; set; } = default(Nullable<long>);
    }
}