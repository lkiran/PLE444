using AutoMapper;
using PLE.Contract.DTOs;

namespace PLE.Service.Models.Mapper
{
	public class PleMappingProfile : Profile
	{
		public PleMappingProfile() {
			CreateMap<ApplicationUser, UserDto>().ReverseMap();
			CreateMap<Course, CourseDto>().ReverseMap();
			CreateMap<Course, CourseDetailDto>();
			CreateMap<Chapter, ChapterDto>();
			CreateMap<Material, MaterialDto>();
			CreateMap<Assignment, AssignmentDto>();
			CreateMap<Discussion, DiscussionDto>();
			CreateMap<Message, MessageDto>();
			CreateMap<Document, DocumentDto>();
			CreateMap<Discussion.Reading, ReadingDto>();
			CreateMap<TimelineEntry, TimelineEntryDto>();
			CreateMap<Community, CommunityDto>();
		}
	}
}