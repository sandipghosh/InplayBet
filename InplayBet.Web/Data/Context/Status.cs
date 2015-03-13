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
    
    public partial class Status : BaseData
    {
        public Status()
        {
            this.Bets = new HashSet<Bet>();
            this.BookMakers = new HashSet<BookMaker>();
            this.Challenges = new HashSet<Challenge>();
            this.Currencies = new HashSet<Currency>();
            this.Legues = new HashSet<Legue>();
            this.Reports = new HashSet<Report>();
            this.Teams = new HashSet<Team>();
            this.Users = new HashSet<User>();
        }
    
        public int StatusId { get; set; }
        public string StatusName { get; set; }
    
        public virtual ICollection<Bet> Bets { get; set; }
        public virtual ICollection<BookMaker> BookMakers { get; set; }
        public virtual ICollection<Challenge> Challenges { get; set; }
        public virtual ICollection<Currency> Currencies { get; set; }
        public virtual ICollection<Legue> Legues { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
        public virtual ICollection<Team> Teams { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
