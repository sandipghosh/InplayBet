//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace InplayBet.Web.Data.Context
{
    using System;
    using System.Collections.Generic;
    
    public partial class User : BaseData
    {
        public User()
        {
            this.Challenges = new HashSet<Challenge>();
            this.Reports = new HashSet<Report>();
            this.Reports1 = new HashSet<Report>();
        }
    
        public int UserKey { get; set; }
        public string UserId { get; set; }
        public int CurrencyId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public string Password { get; set; }
        public string Sex { get; set; }
        public System.DateTime DateOfBirth { get; set; }
        public int BookMakerId { get; set; }
        public string AvatarPath { get; set; }
        public bool IsAdmin { get; set; }
        public int StatusId { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
    
        public virtual BookMaker BookMaker { get; set; }
        public virtual ICollection<Challenge> Challenges { get; set; }
        public virtual Currency Currency { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
        public virtual ICollection<Report> Reports1 { get; set; }
        public virtual Status Status { get; set; }
    }
}
