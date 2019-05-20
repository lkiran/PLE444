using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using PLE.Contract.DTOs;
using PLE.Service.Models;

namespace PLE.Service.Implementations
{
	public class QuizService
	{
		#region Fields
		private readonly PleDbContext _db;
		#endregion

		#region Ctor
		public QuizService() {
			_db = new PleDbContext();
		}
		#endregion

		public List<QuizListDto> List(Guid courseId, bool forCreator = false) {
			var quizzes = _db.Quizzes.Where(quiz => quiz.CourseId == courseId && !quiz.IsDeleted);
			if (!forCreator)
				quizzes = quizzes.Where(q => q.IsPublished);

			var result = Mapper.Map<List<QuizListDto>>(quizzes);

			return result;
		}


		public List<UserAnswer> GetUserAnswers(Guid quizId, string userId = null) {
			var quiz = GetQuiz(quizId);
			var result = new List<UserAnswer>();
			foreach (var question in quiz.Questions) {
				var r = _db.UserAnswers
					.Include("Answer")
					.Include("User")
					.Where(a => a.QuestionId == question.Id);
				result.AddRange(r);
			}
			if (!string.IsNullOrEmpty(userId))
				result = result.Where(a => a.UserId == userId).ToList();

			return result;
		}

		public Quiz GetQuiz(Guid quizId) {
			var quiz = _db.Quizzes
						   .Include("Questions")
						   .Include("Questions.AnswerOptions")
						   .FirstOrDefault(q => q.Id == quizId && !q.IsDeleted)
				?? throw new Exception($"Quiz with id={quizId} cannot be found");

			return quiz;
		}

		public Guid Create(Quiz quiz) {
			quiz.Id = Guid.NewGuid();

			quiz = _db.Quizzes.Add(quiz);
			_db.SaveChanges();

			return quiz.Id;
		}

		public Quiz Update(Quiz quiz) {
			var model = GetQuiz(quiz.Id);
			model.IsPublished = quiz.IsPublished;
			model.AvailableOn = quiz.AvailableOn;
			model.AvailableTill = quiz.AvailableTill;
			model.Description = quiz.Description;
			model.Name = quiz.Name;
			model.TimeSpan = quiz.TimeSpan;

			_db.Entry(model).State = EntityState.Modified;
			_db.SaveChanges();
			return model;
		}

		public void DeleteQuiz(Guid quizId) {
			var model = GetQuiz(quizId);
			model.IsDeleted = true;

			_db.Entry(model).State = EntityState.Modified;
			_db.SaveChanges();
		}

		#region Questions
		public Question GetQuestion(Guid questionId) {
			var question = _db.Questions
							   .Include("AnswerOptions")
							   .FirstOrDefault(q => q.Id == questionId && !q.IsDeleted)
					   ?? throw new Exception($"Question with id={questionId} cannot be found");

			return question;
		}

		public Quiz GetQuizOfQuestion(Question model) {
			return _db.Quizzes.Include("Questions").FirstOrDefault(q => q.Questions.Any(a => a.Id == model.Id))
				   ?? throw new Exception($"Related Quiz of question with id={model.Id} cannot be found");
		}

		public Guid AddQuestion(Question question, Guid quizId) {
			question.Id = Guid.NewGuid();
			question = _db.Questions.Add(question);

			var quiz = GetQuiz(quizId);
			quiz.Questions.Add(question);

			_db.Entry(quiz).State = EntityState.Modified;
			_db.SaveChanges();

			return question.Id;
		}

		public Question UpdateQuestion(Question question) {
			var model = GetQuestion(question.Id);
			model.Title = question.Title;
			model.Description = question.Description;
			model.Answering = question.Answering;

			_db.Entry(model).State = EntityState.Modified;
			_db.SaveChanges();

			return model;
		}

		public void DeleteQuestion(Guid questionId) {
			var model = GetQuestion(questionId);
			model.IsDeleted = true;

			_db.Entry(model).State = EntityState.Modified;
			var quiz = GetQuizOfQuestion(model);
			quiz.Questions.Remove(model);
			_db.Entry(quiz).State = EntityState.Modified;

			_db.SaveChanges();
		}
		#endregion

		#region Answers
		public Answer GetAnswer(Guid answerId, bool raise = true) {
			var answer = _db.Answers.FirstOrDefault(a => a.Id == answerId);
			return answer == null && raise
				? throw new Exception($"Answer with id={answerId} could not be found")
				: answer;
		}

		public Question GetQuestionOfAnswerOption(Answer model) {
			return _db.Questions.FirstOrDefault(q => q.AnswerOptions.Any(a => a.Id == model.Id))
				   ?? throw new Exception($"Related Question of answer with id={model.Id} cannot be found");
		}

		private Answer CreateAnswer(Answer answer) {
			answer.Id = Guid.NewGuid();
			answer = _db.Answers.Add(answer);
			_db.SaveChanges();

			return answer;
		}

		public Guid AddAnswer(Answer answer, Guid questionId) {
			var question = GetQuestion(questionId);
			answer = CreateAnswer(answer);
			question.AnswerOptions.Add(answer);

			_db.Entry(question).State = EntityState.Modified;
			_db.SaveChanges();

			return answer.Id;
		}

		public Answer UpdateAnswer(Answer answer) {
			var model = GetAnswer(answer.Id);
			model.Content = answer.Content;

			_db.Entry(model).State = EntityState.Modified;
			_db.SaveChanges();

			return model;
		}

		public void RemoveAnswerOption(Guid answerId) {
			var answer = GetAnswer(answerId);
			var question = GetQuestionOfAnswerOption(answer);
			question.AnswerOptions.Remove(answer);

			_db.Entry(question).State = EntityState.Modified;
			_db.SaveChanges();
		}
		#endregion

		#region User Answers
		public List<UserAnswer> SaveUserAnswer(Guid questionId, string userId, List<Answer> answers) {
			var question = GetQuestion(questionId);

			var quiz = GetQuizOfQuestion(question);
			if (!quiz.CanAnswer)
				throw new Exception("Answer time for quiz is overdue");

			switch (question.Answering) {
				case Question.AnswerType.SingleSelection:
					return new List<UserAnswer> { SaveUserAnswer(question, userId, answers.First().Id) };
				case Question.AnswerType.MultipleSelection:
					return SaveUserAnswer(question, userId, answers.Select(a => a.Id).ToArray());
				case Question.AnswerType.ShortAnswer:
					return new List<UserAnswer> { SaveUserAnswer(question, userId, answers.First()) };
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private UserAnswer SaveUserAnswer(Question question, string userId, Answer answer) {
			answer = CreateAnswer(answer);
			var ua = AddUserAnswer(question, answer, userId);

			return ua;
		}

		private UserAnswer SaveUserAnswer(Question question, string userId, Guid answerId) {
			var answer = GetAnswer(answerId);
			var ua = AddUserAnswer(question, answer, userId);

			return ua;
		}

		private List<UserAnswer> SaveUserAnswer(Question question, string userId, Guid[] answerIds) {
			var answers = answerIds.Select(a => GetAnswer(a, false));
			var userAnswers = answers.Select(answer => AddUserAnswer(question, answer, userId)).ToList();

			return userAnswers;
		}

		private UserAnswer AddUserAnswer(Question question, Answer answer, string userId) {
			var ua = new UserAnswer {
				Id = Guid.NewGuid(),
				AnsweredOn = DateTime.Now,
				Question = question,
				Answer = answer,
				UserId = userId
			};
			ua = _db.UserAnswers.Add(ua);
			_db.SaveChanges();

			return ua;
		}
		#endregion
	}
}