﻿using System;
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
				quizzes = quizzes.Where(q => q.AvailableOn >= DateTime.Now);

			var result = Mapper.Map<List<QuizListDto>>(quizzes);

			return result;
		}

		public Quiz GetQuiz(Guid quizId) {
			var quiz = _db.Quizzes.FirstOrDefault(q => q.Id == quizId && !q.IsDeleted)
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
			var question = _db.Questions.FirstOrDefault(q => q.Id == questionId && !q.IsDeleted)
					   ?? throw new Exception($"Question with id={questionId} cannot be found");

			return question;
		}

		public Guid AddQuestion(Question question, Guid quizId) {
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

		public List<UserAnswer> SaveUserAnswer(Guid questionId, string userId, List<Answer> answers) {
			var question = GetQuestion(questionId);
			if (question.Answering != Question.AnswerType.ShortAnswer)
				throw new Exception($"Question with id={questionId} is not set for AnswerType.ShortAnswer");

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
		#endregion

		#region Private Methods
		private UserAnswer AddUserAnswer(Question question, Answer answer, string userId) {
			var ua = new UserAnswer {
				Answer = answer,
				AnsweredOn = DateTime.Now,
				UserId = userId
			};
			ua = _db.UserAnswers.Add(ua);

			question.UserAnswers.Add(ua);
			_db.Entry(question).State = EntityState.Modified;

			_db.SaveChanges();

			return ua;
		}

		private Answer CreateAnswer(Answer answer) {
			answer = _db.Answers.Add(answer);
			_db.SaveChanges();

			return answer;
		}

		private Quiz GetQuizOfQuestion(Question model) {
			return _db.Quizzes.FirstOrDefault(q => q.Questions.Contains(model))
				   ?? throw new Exception($"Related Quiz of question with id={model.Id} cannot be found");
		}
		#endregion
	}
}