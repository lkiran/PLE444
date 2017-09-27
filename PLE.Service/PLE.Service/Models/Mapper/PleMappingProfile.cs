using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using PLE.Contract.DTOs;

namespace PLE.Service.Models.Mapper
{
	public class PleMappingProfile : Profile
	{
		public PleMappingProfile() {
			CreateMap<ApplicationUser, UserDto>();
			CreateMap<Space, SpaceDto>();
			CreateMap<Course, CourseDto>();
			CreateMap<Chapter, ChapterDto>();
			CreateMap<Material, MaterialDto>();
			CreateMap<Assignment, AssignmentDto>();
			CreateMap<Discussion, DiscussionDto>();
			CreateMap<Message, MessageDto>();
			CreateMap<Document, DocumentDto>();
			CreateMap<Discussion.Reading, ReadingDto>();
			CreateMap<TimelineEntry, TimeLineEntryDto>();
		}
	}
}