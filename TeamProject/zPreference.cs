//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TeamProject
{
    using System;
    using System.Collections.Generic;
    
    public partial class zPreference
    {
        public short UserId { get; set; }
        public short PlaceHolder { get; set; }
    
        public virtual zUser zUser { get; set; }
    }
}
