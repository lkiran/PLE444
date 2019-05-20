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
			CreateMap<Answer, AnswerDto>().ReverseMap();
			CreateMap<Quiz, QuizDto>().ReverseMap();
			CreateMap<Question, QuestionDto>().ReverseMap();
			CreateMap<Question.AnswerType, QuestionDto.AnswerType>().ReverseMap();
			CreateMap<Question.EvaluationType, QuestionDto.EvaluationType>().ReverseMap();
			CreateMap<UserAnswer, UserAnswerDto>().ReverseMap();

			/* Non-reverseble mappings */
			CreateMap<Discussion.Reading, ReadingDto>();
			CreateMap<TimelineEntry, TimelineEntryDto>();
			CreateMap<Quiz, QuizListDto>();
		}
	}
}