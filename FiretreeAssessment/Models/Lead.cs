//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FiretreeAssessment.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Lead
    {
        [Key]
        public int LeadId { get; set; }
        
        [Required, Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required, Display(Name = "Last Name")]
        public string LastName { get; set; }

        [EmailAddress]
        [Required, Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required, Display(Name = "Message")]
        public string Message { get; set; }
        
        [Display(Name = "Agent Email")]
        public string AgentEmail { get; set; }
    }
}
