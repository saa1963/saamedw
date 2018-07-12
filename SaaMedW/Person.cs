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
    
    public partial class Person
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Person()
        {
            this.Invoice = new HashSet<Invoice>();
            this.Visit = new HashSet<Visit>();
            this.Pays = new HashSet<Pays>();
            this.Person_Person2 = new HashSet<Person>();
            this.Person2_Person = new HashSet<Person>();
        }
    
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public Nullable<System.DateTime> BirthDate { get; set; }
        public string Phone { get; set; }
        public Nullable<int> Sex { get; set; }
        public string Inn { get; set; }
        public string Snils { get; set; }
        public Nullable<int> DocumentTypeId { get; set; }
        public string DocSeria { get; set; }
        public string DocNumber { get; set; }
        public string AddressSubject { get; set; }
        public string AddressRaion { get; set; }
        public string AddressCity { get; set; }
        public string AddressPunkt { get; set; }
        public string AddressStreet { get; set; }
        public string AddressHouse { get; set; }
        public string AddressFlat { get; set; }
        public Nullable<int> Mestnost { get; set; }
        public System.DateTime CreateDate { get; set; }
        public Nullable<int> RepresentativeId { get; set; }
    
        public virtual DocumentType DocumentType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Invoice> Invoice { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Visit> Visit { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pays> Pays { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Person> Person_Person2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Person> Person2_Person { get; set; }
        public virtual Person Representative { get; set; }
    }
}
