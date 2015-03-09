﻿
namespace InplayBet.Web.MapperConfig
{
    using AutoMapper;
    using InplayBet.Web.MapperConfig.Converter;
    using InplayBet.Web.Utilities;
    using InplayBet.Web.Models;
    using InplayBet.Web.Data.Context;
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

                Mapper.CreateMap<Status, StatusModel>().MapBothWays().IgnoreAllNonExisting();
                Mapper.CreateMap<BookMaker, BookMakerModel>().MapBothWays().IgnoreAllNonExisting();
                Mapper.CreateMap<User, UserModel>().MapBothWays().IgnoreAllNonExisting();

                Mapper.CreateMap<Bet, BetModel>()
                    .ForMember(dest => dest.TeamA, opt => opt.MapFrom(src => src.Team))
                    .ForMember(dest => dest.TeamB, opt => opt.MapFrom(src => src.Team1))
                    .MapBothWays().IgnoreAllNonExisting();
                Mapper.CreateMap<Challenge, ChallengeModel>().MapBothWays().IgnoreAllNonExisting();
                Mapper.CreateMap<Legue, LegueModel>().MapBothWays().IgnoreAllNonExisting();
                Mapper.CreateMap<Team, TeamModel>().MapBothWays().IgnoreAllNonExisting();

                Mapper.AssertConfigurationIsValid();
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker();
            }
        }
    }
}