﻿using AutoMapper;
using PLE.Contract.DTOs;
using PLE444.ViewModels;

namespace PLE444.Models
{
	public class PleWebsiteMappingProfile : Profile
	{
		public PleWebsiteMappingProfile() {
			CreateMap<CreateCourseViewModel, CourseDto>().ReverseMap();
		}
	}
}