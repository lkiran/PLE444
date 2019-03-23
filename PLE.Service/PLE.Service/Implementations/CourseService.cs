using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Claims;
using System.Web;
using AutoMapper;
using Microsoft.AspNet.Identity;
using PLE.Contract.DTOs;
using PLE.Contract.DTOs.Requests;
using PLE.Contract.Enums;
using PLE.Service.Models;

namespace PLE.Service.Implementations
{
	public class CourseService
	{
		private readonly PleDbContext _db = new PleDbContext();

		public CourseDto Get(Guid id) {
			var course = _db.Courses.FirstOrDefault(c => c.Id == id);
			if (course == null)
				throw new Exception($"Course with id={id} cannot be found");
			var result = Mapper.Map<CourseDto>(course);

			return result;
		}

		public CourseDetailDto Detail(Guid id) {
			var course = _db.Courses
				.Include("Creator")
				.Include("Timeline")
				.Include("Timeline.Creator")
				.FirstOrDefault(c => c.Id == id);
			if (course == null)
				throw new Exception($"Course with id={id} cannot be found");
			var result = Mapper.Map<CourseDetailDto>(course);
			var terms = _db.Courses.Where(c => c.CopiedFromId == course.CopiedFromId && c.Id != id).ToList();
			result.Terms = Mapper.Map<List<CourseDto>>(terms);
			result.MemberCount = _db.UserCourses.Count(uc => uc.CourseId == course.Id && uc.IsActive && uc.DateJoin != null);

			return result;
		}

		public Guid Create(CourseDto request) {
			var course = Mapper.Map<Course>(request);
			course.Id = Guid.NewGuid();
			course.DateCreated = DateTime.Now;
			course.Timeline = new List<TimelineEntry>
			{
				new TimelineEntry
				{
					Heading = "Ders oluşturuldu",
					CreatorId = course.CreatorId,
					DateCreated = DateTime.Now,
					IconClass = "ti ti-plus"
				}
			};

			course = _db.Courses.Add(course);
			_db.SaveChanges();

			course.CopiedFromId = course.Id;
			_db.Entry(course).State = EntityState.Modified;
			_db.SaveChanges();

			return course.Id;
		}

		public Guid Duplicate(DuplicateCourseRequestDto request) {
			var userId = HttpContext.Current.User.Identity.GetUserId();
			var courseToDuplicate = _db.Courses
				.Include("Chapters")
				.Include("Assignments")
				.Include("Chapters.Materials")
				.FirstOrDefault(c => c.Id == request.Id && c.IsCourseActive);
			if (courseToDuplicate == null)
				throw new Exception($"An active Course term with id={request.Id} cannot be found");

			#region Duplicate Course
			var newCourse = new Course {
				CopiedFromId = courseToDuplicate.CopiedFromId,
				Code = request.NewCode,
				Name = request.NewName,
				Description = courseToDuplicate.Description,
				CreatorId = userId,
				DateCreated = DateTime.Now,
				IsCourseActive = false,
				Assignments = new List<Assignment>(),
				Chapters = new List<Chapter>(),
				Timeline = new List<TimelineEntry> {
					new TimelineEntry {
						Heading = "Ders oluşturuldu",
						CreatorId =  userId,
						DateCreated = DateTime.Now,
						IconClass = TimelineEntry.Icon.Plus
					}
				}
			};
			newCourse = _db.Courses.Add(newCourse);
			_db.SaveChanges();
			#endregion

			#region Duplicate Assignments
			foreach (var baseAssignment in courseToDuplicate.Assignments) {
				try {
					var newAssignment = new Assignment {
						CourseId = newCourse.Id,
						Title = baseAssignment.Title,
						Description = baseAssignment.Description,
						DateAdded = DateTime.Now,
						Deadline = DateTime.Now,
						IsActive = true,
						IsFeedbackPublished = false,
						Uploads = new List<Document>()
					};

					newCourse.Assignments.Add(newAssignment);
				} catch (Exception ex) {
					Debug.WriteLine(ex.Message);
				}
			}
			#endregion

			#region Duplicate Chapters
			foreach (var baseChapter in courseToDuplicate.Chapters) {
				try {
					var newChapter = new Chapter {
						CourseId = newCourse.Id,
						Description = baseChapter.Description,
						Title = baseChapter.Title,
						DateAdded = DateTime.Now,
						OrderBy = baseChapter.OrderBy,
						IsActive = true,
						Materials = baseChapter.Materials.ToList()
					};

					newCourse.Chapters.Add(newChapter);
				} catch (Exception ex) {
					Debug.WriteLine(ex.Message);
				}
			}
			#endregion

			_db.Entry(newCourse).State = EntityState.Modified;
			_db.SaveChanges();

			#region Deactivate other terms
			var terms = _db.Courses.Where(c => c.CopiedFromId == newCourse.CopiedFromId && c.Id != newCourse.Id).ToList();
			terms.ForEach(t => t.IsCourseActive = false);
			_db.SaveChanges();
			#endregion

			return newCourse.Id;
		}

		public List<CourseDto> GetCourseListByUser(string userId) {
			var joinedCourses = _db.UserCourses
				.Where(uc => uc.UserId == userId && uc.IsActive && uc.Course.IsCourseActive)
				.Select(c => c.Course);
			var createdCourses = _db.Courses.Where(c => c.CreatorId == userId);
			var data = joinedCourses.Union(createdCourses);

			var result = Mapper.Map<List<CourseDto>>(data.ToList());

			return result;
		}

		public IEnumerable<ClaimDto> GetWaitingClaims(string userId) {
			var userCourses = _db.UserCourses.Where(uc => uc.UserId == userId && uc.IsActive && uc.DateJoin == null);
			return userCourses.Select(t => new ClaimDto { Key = PleClaimType.Waiting, Value = t.CourseId.ToString() });
		}

		public IEnumerable<ClaimDto> GetMemberClaims(string userId) {
			var courses = _db.UserCourses.Where(uc => uc.UserId == userId && uc.IsActive && uc.DateJoin != null).Select(uc => uc.Course).ToList();
			foreach (var userCourse in courses) // Include terms of courses
				courses.AddRange(_db.Courses.Where(c => c.CopiedFromId == userCourse.CopiedFromId));

			return courses.Distinct().Select(t => new ClaimDto { Key = PleClaimType.Member, Value = t.Id.ToString() });
		}

		public IEnumerable<ClaimDto> GetCreatorClaims(string userId) {
			var courses = _db.Courses.Where(c => c.CreatorId == userId);
			return courses.Select(t => new ClaimDto { Key = PleClaimType.Creator, Value = t.Id.ToString() });
		}

		public IEnumerable<ClaimDto> GetAllClaims(string userId) {
			return new List<ClaimDto>()
				.Concat(GetWaitingClaims(userId))
				.Concat(GetCreatorClaims(userId))
				.Concat(GetMemberClaims(userId));
		}
	}
}