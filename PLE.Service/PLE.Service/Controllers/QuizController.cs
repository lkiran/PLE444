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
		[Route("GetUserAnswers/{quizId}")]
		public IHttpActionResult GetUserAnswers(Guid quizId) {
			try {
				var userId = User.Identity.GetUserId();
				var userAnswers = _quizService.GetUserAnswers(quizId, userId);
				var result = Mapper.Map<List<UserAnswerDto>>(userAnswers);

				return Ok(result);
			} catch (Exception e) {
				return InternalServerError(e);
			}
		}

		[HttpGet]
		[Route("GetUserAnswersForCreator/{quizId}")]
		public IHttpActionResult GetUserAnswersForCreator(Guid quizId) {
			try {
				var userAnswers = _quizService.GetUserAnswers(quizId);
				var result = Mapper.Map<List<UserAnswerDto>>(userAnswers);

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

		[HttpGet]
		[Route("Question/{id}")]
		public IHttpActionResult Question(Guid id) {
			try {
				var question = _quizService.GetQuestion(id);
				var result = Mapper.Map<QuestionDto>(question);
				var quiz = Mapper.Map<QuizDto>(_quizService.GetQuizOfQuestion(question));
				result.Quiz = quiz;

				return Ok(result);
			} catch (Exception e) {
				return InternalServerError(e);
			}
		}

		[HttpPost]
		[Authorize]
		[Route("Question/Add/{quizId}")]
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
		[Route("Question/Update")]
		public IHttpActionResult UpdateQuestion(QuestionDto request) {
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

		[HttpGet]
		[Route("Question/Answer/{id}")]
		public IHttpActionResult Answer(Guid id) {
			try {
				var answer = _quizService.GetAnswer(id);
				var result = Mapper.Map<AnswerDto>(answer);
				var question = Mapper.Map<QuestionDto>(_quizService.GetQuestionOfAnswerOption(answer));
				result.Question = question;

				return Ok(result);
			} catch (Exception e) {
				return InternalServerError(e);
			}
		}


		[HttpPost]
		[Authorize]
		[Route("Question/Answer/Add/{questionId}")]
		public IHttpActionResult AddAnswer([FromBody]AnswerDto request, [FromUri]Guid questionId) {
			try {
				var answer = Mapper.Map<Answer>(request);
				var result = _quizService.AddAnswer(answer, questionId);

				return Ok(result);
			} catch (Exception e) {
				return InternalServerError(e);
			}
		}

		[HttpPost]
		[Authorize]
		[Route("Question/Answer/Update")]
		public IHttpActionResult UpdateAnswer(AnswerDto request) {
			try {
				var answer = Mapper.Map<Answer>(request);
				answer = _quizService.UpdateAnswer(answer);
				var result = Mapper.Map<AnswerDto>(answer);

				return Ok(result);
			} catch (Exception e) {
				return InternalServerError(e);
			}
		}

		[HttpDelete]
		[Authorize]
		[Route("Question/Answer/{answerId}")]
		public IHttpActionResult DeleteAnswer(Guid answerId) {
			try {
				_quizService.RemoveAnswerOption(answerId);

				return Ok();
			} catch (Exception e) {
				return InternalServerError(e);
			}
		}

		[HttpPost]
		[Authorize]
		[Route("Question/Answer/{questionId}")]
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