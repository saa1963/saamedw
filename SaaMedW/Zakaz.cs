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
    
    public partial class Zakaz
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Zakaz()
        {
            this.Zakaz1 = new HashSet<Zakaz1>();
        }
    
        public int Id { get; set; }
        public System.DateTime Dt { get; set; }
        public int Num { get; set; }
        public int PersonId { get; set; }
        public bool Dms { get; set; }
        public string Polis { get; set; }
        public Nullable<int> DmsCompanyId { get; set; }
        public Nullable<System.DateTime> Vozvrat { get; set; }
        public string Email { get; set; }
        public bool Card { get; set; }
        public Nullable<int> CheckNum { get; set; }
    
        public virtual DmsCompany DmsCompany { get; set; }
        public virtual Person Person { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Zakaz1> Zakaz1 { get; set; }
    }
}
