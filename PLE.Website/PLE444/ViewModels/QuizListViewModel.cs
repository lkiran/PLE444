using System.Collections.Generic;
using PLE.Contract.DTOs;

namespace PLE444.ViewModels
{
	public class QuizListViewModel
	{
		public List<QuizListDto> Quizzes { get; set; }
	
		public CourseDto Course { get; set; }

		public bool IsCreator { get; set; }
		
		public bool IsMember { get; internal set; }
		
		public bool IsViewer { get; internal set; }
	}
}