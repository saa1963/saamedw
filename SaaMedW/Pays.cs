//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SaaMedW
{
    using System;
    using System.Collections.Generic;
    
    public partial class Pays
    {
        public int Id { get; set; }
        public System.DateTime Dt { get; set; }
        public int PersonId { get; set; }
        public decimal Sm { get; set; }
        public enumPaymentType PaymentType { get; set; }
    
        public virtual Person Person { get; set; }
    }
}