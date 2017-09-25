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
		public PleMappingProfile()
		{
			CreateMap<ApplicationUser, UserDto>();
			CreateMap<Course, CourseDto>();
			
		}
	}
}