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
    
    public partial class Team : BaseData
    {
        public Team()
        {
            this.Bets = new HashSet<Bet>();
            this.Bets1 = new HashSet<Bet>();
        }
    
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public int StatusId { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
    
        public virtual Status Status { get; set; }
        public virtual ICollection<Bet> Bets { get; set; }
        public virtual ICollection<Bet> Bets1 { get; set; }
    }
}