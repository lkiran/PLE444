﻿using System;
using PLE.Contract.DTOs;
using System.Collections.Generic;

namespace PLE.Website.Service
{
	public class QuizService : BaseService
	{
		public QuizDto Detail(Guid id) {
			var url = $"Api/Quiz/Detail/{id}";
			var response = Client.Get<QuizDto>(url);
			return response;
		}

		public List<QuizListDto> ListByCourse(Guid courseId) {
			var url = $"Api/Quiz/ListByCourse/{courseId}";
			var response = Client.Get<List<QuizListDto>>(url);
			return response;
		}

		public List<QuizListDto> ListForCreator(Guid courseId) {
			var url = $"Api/Quiz/ListForCreator/{courseId}";
			var response = Client.Get<List<QuizListDto>>(url);
			return response;
		}

		public Guid Create(QuizDto request) {
			var url = $"Api/Quiz/Create";
			var response = Client.Post<Guid>(url, request);
			return response;
		}

		public QuizDto Update(QuizDto request) {
			var url = $"Api/Quiz/Update";
			var response = Client.Post<QuizDto>(url, request);
			return response;
		}

		public void DeleteQuiz(Guid id) {
			var url = $"Api/Quiz/{id}";
			Client.Delete(url);
		}

		public Guid AddQuestion(QuestionDto request, Guid quizId) {
			var url = $"Api/Quiz/AddQuestion/{quizId}";
			var response = Client.Post<Guid>(url, request);
			return response;
		}

		public QuestionDto Update(QuestionDto request) {
			var url = $"Api/Quiz/UpdateQuestion";
			var response = Client.Post<QuestionDto>(url, request);
			return response;
		}

		public void DeleteQuestion(Guid questionId) {
			var url = $"Api/Quiz/Question/{questionId}";
			Client.Delete(url);
		}

		public List<UserAnswerDto> SaveUserAnswer(Guid questionId, List<AnswerDto> request) {
			var url = $"Api/Quiz/Answer/{questionId}";
			var response = Client.Post<List<UserAnswerDto>>(url, request);
			return response;
		}
	}
}