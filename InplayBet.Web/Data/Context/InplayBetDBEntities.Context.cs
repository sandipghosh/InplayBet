﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class InplayBetDBEntities : DbContext
    {
        public InplayBetDBEntities()
            : base("name=InplayBetDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Bet> Bets { get; set; }
        public DbSet<BookMaker> BookMakers { get; set; }
        public DbSet<Challenge> Challenges { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Legue> Legues { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRank> UserRanks { get; set; }
    }
}
