//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ekaH_server.App_DBHandler
{
    using System;
    using System.Collections.Generic;
    
    public partial class student_info
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public student_info()
        {
            this.appointments = new HashSet<appointment>();
            this.submissions = new HashSet<submission>();
            this.studentcourses = new HashSet<studentcourse>();
        }
    
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string education { get; set; }
        public string concentration { get; set; }
        public Nullable<int> graduationYear { get; set; }
        public string streetAdd1 { get; set; }
        public string streetAdd2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string phone { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<appointment> appointments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<submission> submissions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<studentcourse> studentcourses { get; set; }
    }
}
