//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SGF
{
    using System;
    using System.Collections.Generic;
    
    public partial class Item
    {
        public int id { get; set; }
        public Nullable<int> gasto_id { get; set; }
        public string nome { get; set; }
        public Nullable<double> valor { get; set; }
    
        public virtual Gasto Gasto { get; set; }
    }
}
