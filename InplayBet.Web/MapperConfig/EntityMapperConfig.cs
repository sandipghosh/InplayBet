
namespace InplayBet.Web.MapperConfig
{
    using AutoMapper;
    using InplayBet.Web.Data.Context;
    using InplayBet.Web.MapperConfig.Converter;
    using InplayBet.Web.Models;
    using InplayBet.Web.Utilities;
    using System;

    public class EntityMapperConfig : Profile
    {
        /// <summary>
        /// Gets the name of the profile.
        /// </summary>
        /// <value>
        /// The name of the profile.
        /// </value>
        public override string ProfileName
        {
            get
            {
                return "EntityMapperConfig";
            }
        }

        /// <summary>
        /// Override this method in a derived class and call the CreateMap method to associate that map with this profile.
        /// Avoid calling the <see cref="T:AutoMapper.Mapper" /> class from this method.
        /// </summary>
        protected override void Configure()
        {
            try
            {
                Mapper.CreateMap<DateTime?, DateTime>().ConvertUsing<DateTimeConverter>();
                Mapper.CreateMap<DateTime?, DateTime?>().ConvertUsing<NullableDateTimeConverter>();

                Mapper.CreateMap<int?, int>().ConvertUsing<IntConverter>();
                Mapper.CreateMap<int?, int?>().ConvertUsing<NullableIntConverter>();

                Mapper.CreateMap<long?, long>().ConvertUsing<LongConverter>();
                Mapper.CreateMap<long?, long?>().ConvertUsing<NullableLongConverter>();

                Mapper.CreateMap<decimal?, decimal>().ConvertUsing<DecimalConverter>();
                Mapper.CreateMap<decimal?, decimal?>().ConvertUsing<NullableDecimalConverter>();

                Mapper.CreateMap<Status, StatusModel>().IgnoreAllNonExisting();
                Mapper.CreateMap<StatusModel, Status>().IgnoreAllNonExisting();

                Mapper.CreateMap<BookMaker, BookMakerModel>().IgnoreAllNonExisting();
                Mapper.CreateMap<BookMakerModel, BookMaker>().IgnoreAllNonExisting();

                Mapper.CreateMap<User, UserModel>().IgnoreAllNonExisting();
                Mapper.CreateMap<UserModel, User>().IgnoreAllNonExisting();

                Mapper.CreateMap<Contact, ContactModel>().IgnoreAllNonExisting();
                Mapper.CreateMap<ContactModel, Contact>().IgnoreAllNonExisting();

                Mapper.CreateMap<Follow, FollowModel>().IgnoreAllNonExisting();
                Mapper.CreateMap<FollowModel, Follow>().IgnoreAllNonExisting();

                Mapper.CreateMap<Bet, BetModel>()
                    .ForMember(dest => dest.TeamA, opt => opt.MapFrom(src => src.Team))
                    .ForMember(dest => dest.TeamB, opt => opt.MapFrom(src => src.Team1))
                    .IgnoreAllNonExisting();
                Mapper.CreateMap<BetModel, Bet>().IgnoreAllNonExisting();

                Mapper.CreateMap<Challenge, ChallengeModel>().IgnoreAllNonExisting();
                Mapper.CreateMap<ChallengeModel, Challenge>().IgnoreAllNonExisting();

                Mapper.CreateMap<Legue, LegueModel>().IgnoreAllNonExisting();
                Mapper.CreateMap<LegueModel, Legue>().IgnoreAllNonExisting();

                Mapper.CreateMap<Team, TeamModel>().IgnoreAllNonExisting();
                Mapper.CreateMap<TeamModel, Team>().IgnoreAllNonExisting();

                Mapper.CreateMap<Currency, CurrencyModel>().IgnoreAllNonExisting();
                Mapper.CreateMap<CurrencyModel, Currency>().IgnoreAllNonExisting();

                Mapper.CreateMap<Report, ReportModel>().IgnoreAllNonExisting();
                Mapper.CreateMap<ReportModel, Report>().IgnoreAllNonExisting();

                Mapper.CreateMap<UserRank, UserRankViewModel>()
                    //.ForMember(dest => dest.Won, opt => opt.MapFrom(src => src.Won.Value))
                    //.ForMember(dest => dest.Placed, opt => opt.MapFrom(src => src.Placed.Value))
                    //.ForMember(dest => dest.Profit, opt => opt.MapFrom(src => src.Profit.Value))
                    //.ForMember(dest => dest.Rank, opt => opt.MapFrom(src => src.Rank.Value))
                    .IgnoreAllNonExisting();
                Mapper.CreateMap<UserRankViewModel, UserRank>().IgnoreAllNonExisting();

                Mapper.AssertConfigurationIsValid();
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker();
            }
        }
    }
}