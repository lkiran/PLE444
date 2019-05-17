using AutoMapper;
using PLE.Contract.DTOs;

namespace PLE.Service.Models.Mapper
{
	public class PleMappingProfile : Profile
	{
		public PleMappingProfile() {
			/* Reverseble mappings */
			CreateMap<ApplicationUser, UserDto>().ReverseMap();
			CreateMap<Course, CourseDto>().ReverseMap();
			CreateMap<Course, CourseDetailDto>().ReverseMap();
			CreateMap<Chapter, ChapterDto>().ReverseMap();
			CreateMap<Material, MaterialDto>().ReverseMap();
			CreateMap<Assignment, AssignmentDto>().ReverseMap();
			CreateMap<Discussion, DiscussionDto>().ReverseMap();
			CreateMap<Message, MessageDto>().ReverseMap();
			CreateMap<Document, DocumentDto>().ReverseMap();
			CreateMap<Community, CommunityDto>().ReverseMap();
			CreateMap<Contract.Enums.RoleType, ApplicationUser.RoleType>().ReverseMap();

			/* Non-reverseble mappings */
			CreateMap<Discussion.Reading, ReadingDto>();
			CreateMap<TimelineEntry, TimelineEntryDto>();
		}
	}
}