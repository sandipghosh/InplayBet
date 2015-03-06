
namespace InplayBet.Web.MapperConfig
{
    using AutoMapper;
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
                //Mapper.CreateMap<NJFairground.Web.Data.Context.Page, PageModel>()
                //    .IgnoreAllNonExisting().MapBothWays().IgnoreAllNonExisting();

                //Mapper.CreateMap<PageItem, PageItemModel>()
                //   .IgnoreAllNonExisting().MapBothWays().IgnoreAllNonExisting();

                //Mapper.CreateMap<Event, EventModel>()
                //   .IgnoreAllNonExisting().MapBothWays().IgnoreAllNonExisting();

                //Mapper.CreateMap<SchedularSchema, EventModel>()
                //    .ForMember(dest => dest.EventId, opt => opt.MapFrom(src => src.id))
                //    .ForMember(dest => dest.EventTitle, opt => opt.MapFrom(src => src.title))
                //    .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.start))
                //    .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.end))
                //    .ForMember(dest => dest.EventDesc, opt => opt.MapFrom(src => src.description))
                //    .IgnoreAllNonExisting();

                //Mapper.CreateMap<EventModel, SchedularSchema>()
                //    .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.EventId))
                //    .ForMember(dest => dest.title, opt => opt.MapFrom(src => src.EventTitle))
                //    .ForMember(dest => dest.start, opt => opt.MapFrom(src => src.StartDate))
                //    .ForMember(dest => dest.end, opt => opt.MapFrom(src => src.EndDate))
                //    .ForMember(dest => dest.description, opt => opt.MapFrom(src => src.EventDesc))
                //    .IgnoreAllNonExisting();
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker();
            }
        }
    }
}