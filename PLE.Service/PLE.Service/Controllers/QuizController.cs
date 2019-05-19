using System;
using System.Collections.Generic;
using System.Web.Http;
using AutoMapper;
using Microsoft.AspNet.Identity;
using PLE.Contract.DTOs;
using PLE.Service.Implementations;
using PLE.Service.Models;

namespace PLE.Service.Controllers
{
	[RoutePrefix("Api/Quiz")]
	public class QuizController : BaseApiController
	{
		#region Fields
		private readonly QuizService _quizService;
		#endregion

		#region Ctor
		public QuizController() {
			_quizService = new QuizService();
		}
		#endregion

		[HttpGet]
		[Route("Detail/{id}")]
		public IHttpActionResult Detail(Guid id) {
			try {
				var quiz = _quizService.GetQuiz(id);
				var result = Mapper.Map<QuizDto>(quiz);

				return Ok(result);
			} catch (Exception e) {
				return InternalServerError(e);
			}
		}

		[HttpGet]
		[Route("ListByCourse/{courseId}")]
		public IHttpActionResult ListByCourse(Guid courseId) {
			try {
				var quizzes = _quizService.List(courseId);
				var result = Mapper.Map<List<QuizListDto>>(quizzes);

				return Ok(result);
			} catch (Exception e) {
				return InternalServerError(e);
			}
		}

		[HttpGet]
		[Route("ListForCreator/{courseId}")]
		public IHttpActionResult ListForCreator(Guid courseId) {
			try {
				var quizzes = _quizService.List(courseId, true);
				var result = Mapper.Map<List<QuizListDto>>(quizzes);

				return Ok(result);
			} catch (Exception e) {
				return InternalServerError(e);
			}
		}

		[HttpPost]
		[Authorize]
		[Route("Create")]
		public IHttpActionResult Create(QuizDto request) {
			try {
				var quiz = Mapper.Map<Quiz>(request);
				var result = _quizService.Create(quiz);
				return Ok(result);
			} catch (Exception e) {
				return InternalServerError(e);
			}
		}

		[HttpPost]
		[Authorize]
		[Route("Update")]
		public IHttpActionResult Update(QuizDto request) {
			try {
				var quiz = Mapper.Map<Quiz>(request);
				quiz = _quizService.Update(quiz);
				var result = Mapper.Map<QuizDto>(quiz);
				return Ok(result);
			} catch (Exception e) {
				return InternalServerError(e);
			}
		}

		[HttpDelete]
		[Authorize]
		[Route("{id}")]
		public IHttpActionResult DeleteQuiz(Guid id) {
			try {
				_quizService.DeleteQuiz(id);

				return Ok();
			} catch (Exception e) {
				return InternalServerError(e);
			}
		}

		[HttpPost]
		[Authorize]
		[Route("AddQuestion/{quizId}")]
		public IHttpActionResult AddQuestion([FromBody]QuestionDto request, [FromUri]Guid quizId) {
			try {
				var question = Mapper.Map<Question>(request);
				Guid result = _quizService.AddQuestion(question, quizId);

				return Ok(result);
			} catch (Exception e) {
				return InternalServerError(e);
			}
		}

		[HttpPost]
		[Authorize]
		[Route("UpdateQuestion")]
		public IHttpActionResult Update(QuestionDto request) {
			try {
				var question = Mapper.Map<Question>(request);
				question = _quizService.UpdateQuestion(question);
				var result = Mapper.Map<QuestionDto>(question);

				return Ok(result);
			} catch (Exception e) {
				return InternalServerError(e);
			}
		}

		[HttpDelete]
		[Authorize]
		[Route("Question/{questionId}")]
		public IHttpActionResult DeleteQuestion(Guid questionId) {
			try {
				_quizService.DeleteQuestion(questionId);

				return Ok();
			} catch (Exception e) {
				return InternalServerError(e);
			}
		}

		[HttpPost]
		[Authorize]
		[Route("Answer/{questionId}")]
		public IHttpActionResult SaveUserAnswer([FromUri]Guid questionId, [FromBody]List<AnswerDto> request) {
			try {
				var userId = User.Identity.GetUserId();

				var answers = Mapper.Map<List<Answer>>(request);
				var userAnswers = _quizService.SaveUserAnswer(questionId, userId, answers);
				var result = Mapper.Map<List<UserAnswerDto>>(userAnswers);

				return Ok(result);
			} catch (Exception e) {
				return InternalServerError(e);
			}
		}
	}
}