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
    
    public partial class officehour
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public officehour()
        {
            this.appointments = new HashSet<appointment>();
        }
    
        public int id { get; set; }
        public string professorID { get; set; }
        public System.DateTime startDTime { get; set; }
        public System.DateTime endDTime { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<appointment> appointments { get; set; }
        public virtual professor_info professor_info { get; set; }
    }
}
