using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using PLE.Contract.DTOs;
using PLE444.Models;
using PLE.Contract.Enums;
using PLE.Website.Service;
using PLE444.Helpers;
using PLE444.ViewModels;

namespace PLE444.Controllers
{
	[PleAuthorization]
	public class QuizController : Controller
	{
		#region Fields
		private PleDbContext db = new PleDbContext();
		private CourseService _courseService;
		private QuizService _quizService;
		private EmailService ms = new EmailService();
		#endregion

		#region Ctor
		public QuizController() {
			_courseService = new CourseService();
			_quizService = new QuizService();
		}
		#endregion

		public ActionResult Index(Guid id) {
			var model = new QuizListViewModel();
			model.Course = _courseService.Detail(id);
			model.IsCreator = isCourseCreator(model.Course);
			model.IsMember = isMember(model.Course);
			model.IsViewer = isViewer(model.Course);

			if (!model.IsViewer && !model.IsMember && !model.IsCreator)
				return RedirectToAction("Index", "Course", new { id = id });

			model.Quizzes = model.IsCreator
				? _quizService.ListForCreator(id)
				: _quizService.ListByCourse(id);

			return View(model);
		}


		public ActionResult Create(Guid id) {
			var model = new QuizViewModel {
				CourseId = id,
				AvailableOn = DateTime.Now,
				AvailableTill = DateTime.Now,
			};
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(QuizViewModel model) {
			if (!ModelState.IsValid)
				return View(model);

			var course = _courseService.Detail(model.CourseId);

			if (!course.IsCourseActive)
				return RedirectToAction("Index", "Home");

			if (!isCourseCreator(course))
				return RedirectToAction("Index", "Home");

			if (!ModelState.IsValid)
				return View(model);

			var quiz = new QuizDto {
				CourseId = model.CourseId,
				Name = model.Name,
				Description = model.Description,
				AvailableOn = model.AvailableOn,
				AvailableTill = model.AvailableTill,
			};

			quiz.Id = _quizService.Create(quiz);

			return RedirectToAction("Edit", "Quiz", new { id = quiz.Id });
		}


		public ActionResult Edit(Guid id) {
			var quiz = _quizService.Detail(id);
			if (quiz == null)
				return HttpNotFound();

			var course = _courseService.Detail(quiz.CourseId);
			if (!isCourseCreator(course))
				return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

			var model = Mapper.Map<QuizViewModel>(quiz);

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(QuizViewModel model) {
			var course = _courseService.Detail(model.CourseId);

			if (!isCourseCreator(course))
				return RedirectToAction("Index", "Home");

			if (!ModelState.IsValid) {
				model = Mapper.Map<QuizViewModel>(_quizService.Detail(model.Id));
				return View(model);
			}

			var quiz = new QuizDto {
				Id = model.Id,
				CourseId = model.CourseId,
				IsPublished = model.IsPublished,
				Name = model.Name,
				Description = model.Description,
				AvailableOn = model.AvailableOn,
				AvailableTill = model.AvailableTill,
			};

			quiz = _quizService.Update(quiz);

			return RedirectToAction("Edit", "Quiz", new { id = quiz.Id });
		}


		public ActionResult Show(Guid id) {
			var quiz = _quizService.Detail(id);
			if (quiz == null)
				return HttpNotFound();

			var course = _courseService.Detail(quiz.CourseId);
			if (!isCourseCreator(course))
				return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

			
			quiz.IsPublished = true;
			quiz = _quizService.Update(quiz);

			return RedirectToAction("Index", "Quiz", new { id = quiz.CourseId });
		}

		public ActionResult Hide(Guid id) {
			var quiz = _quizService.Detail(id);
			if (quiz == null)
				return HttpNotFound();

			var course = _courseService.Detail(quiz.CourseId);
			if (!isCourseCreator(course))
				return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

			quiz.IsPublished = false;
			quiz = _quizService.Update(quiz);

			return RedirectToAction("Index", "Quiz", new { id = quiz.CourseId });
		}


		public ActionResult Delete() {
			throw new NotImplementedException();
		}


		public ActionResult AddQuestion(Guid quizId) {
			var model = new QuestionViewModel {
				QuizId = quizId,
				Answering = QuestionDto.AnswerType.SingleSelection
			};
			return View("Question", model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult AddQuestion(QuestionViewModel model) {
			if (!ModelState.IsValid)
				return View("Question", model);

			var quiz = _quizService.Detail(model.QuizId);
			var course = _courseService.Detail(quiz.CourseId);

			if (!course.IsCourseActive)
				return RedirectToAction("Index", "Home");

			if (!isCourseCreator(course))
				return RedirectToAction("Index", "Home");

			var question = new QuestionDto {
				Title = model.Title,
				Description = model.Description,
				Answering = model.Answering,
			};

			question.Id = _quizService.AddQuestion(question, quiz.Id);

			return RedirectToAction("Edit", "Quiz", new { id = quiz.Id });
		}


		public ActionResult EditQuestion(Guid id) {
			var question = _quizService.GetQuestion(id);
			var model = Mapper.Map<QuestionViewModel>(question);

			return View("Question", model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult EditQuestion(QuestionViewModel model) {
			if (!ModelState.IsValid)
				return View("Question", model);

			var question = new QuestionDto {
				Id = model.Id,
				Title = model.Title,
				Description = model.Description,
				Answering = model.Answering
			};

			question = _quizService.UpdateQuestion(question);
			question = _quizService.GetQuestion(question.Id);

			return RedirectToAction("Edit", "Quiz", new { id = question.Quiz.Id });
		}


		public ActionResult DeleteQuestion(Guid id) {
			throw new NotImplementedException();
		}


		public ActionResult AddAnswerOption(Guid questionId) {
			var model = new AnswerViewModel() {
				QuestionId = questionId,
			};
			return View("Answer", model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult AddAnswerOption(AnswerViewModel model) {
			if (!ModelState.IsValid)
				return View("Answer", model);

			var question = _quizService.GetQuestion(model.QuestionId);
			var course = _courseService.Detail(question.Quiz.CourseId);

			if (!course.IsCourseActive)
				return RedirectToAction("Index", "Home");

			if (!isCourseCreator(course))
				return RedirectToAction("Index", "Home");

			var answer = new AnswerDto {
				Content = model.Content
			};

			question.Id = _quizService.AddAnswer(answer, question.Id);

			return RedirectToAction("EditQuestion", "Quiz", new { id = model.QuestionId });
		}


		public ActionResult EditAnswerOption(Guid id) {
			var answerOption = _quizService.GetAnswer(id);
			var model = Mapper.Map<AnswerViewModel>(answerOption);

			return View("Answer", model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult EditAnswerOption(AnswerViewModel model) {
			if (!ModelState.IsValid)
				return View("Answer", model);

			var answer = new AnswerDto {
				Id = model.Id,
				Content = model.Content
			};

			answer = _quizService.UpdateAnswer(answer);
			answer = _quizService.GetAnswer(answer.Id);

			return RedirectToAction("EditQuestion", "Quiz", new { id = answer.Question.Id });
		}


		public ActionResult DeleteAnswer() {
			throw new NotImplementedException();
		}

		#region Private Methods
		private bool isCourseCreator(Guid? courseId) {
			if (courseId == null)
				return false;
			var identity = User.GetPrincipal()?.Identity as PleClaimsIdentity;
			if (identity == null)
				return false;
			return identity.HasClaim(PleClaimType.Creator, courseId.ToString());
		}

		private bool isCourseCreator(CourseDto course) {
			return isCourseCreator(course.Id);
		}

		private bool isMember(Guid? courseId) {
			if (courseId == null)
				return false;
			if (!(User.GetPrincipal()?.Identity is PleClaimsIdentity identity))
				return false;
			return identity.HasClaim(PleClaimType.Member, courseId.ToString());
		}

		private bool isMember(CourseDto course) {
			return isMember(course.Id);
		}

		private bool isViewer(CourseDto course) {
			return isViewer(course.Id);
		}

		private bool isViewer(Guid? courseId) {
			if (courseId == null)
				return false;
			if (!(User.GetPrincipal()?.Identity is PleClaimsIdentity identity))
				return false;
			return identity.HasClaim(PleClaimType.Viewer, courseId.ToString());
		}

		private bool isWaiting(Guid? courseId) {
			if (courseId == null)
				return false;
			if (!(User.GetPrincipal()?.Identity is PleClaimsIdentity identity))
				return false;
			var waiting = identity.HasClaim(PleClaimType.Waiting, courseId.ToString());
			if (!waiting)
				return waiting;
			identity.AddClaims(_courseService.GetClaims());
			waiting = identity.HasClaim(PleClaimType.Waiting, courseId.ToString());
			return waiting;
		}

		protected override void Dispose(bool disposing) {
			if (disposing) {
				db.Dispose();
			}
			base.Dispose(disposing);
		}
		#endregion
	}
}