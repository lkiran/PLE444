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
using PLE.Contract.DTOs.Responses;
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

		public bool Join(string userId, Guid courseId) {
			var course = GetActiveCourseById(courseId);
			var uc = _db.UserCourses.FirstOrDefault(i => i.UserId == userId.ToString() && i.Course.CopiedFromId == course.CopiedFromId);

			if (uc == null) {
				uc = new UserCourse {
					UserId = userId,
					Course = course
				};

				_db.UserCourses.Add(uc);
			} else {
				uc.IsActive = true;
				_db.Entry(uc).State = EntityState.Modified;
			}

			_db.SaveChanges();

			return true;
		}

		public bool Leave(string userId, Guid courseId) {
			var uc = _db.UserCourses.FirstOrDefault(i => i.UserId == userId.ToString() && i.Course.Id == courseId)
					 ?? throw new Exception($"The user membership is not found for the user with id={userId} and the course with id={courseId}");

			uc.IsActive = false;
			uc.DateJoin = null;

			_db.Entry(uc).State = EntityState.Modified;
			_db.SaveChanges();

			return true;
		}

		public bool Eject(string userId, Guid courseId) {
			var uc = _db.UserCourses.FirstOrDefault(c => c.CourseId == courseId && c.UserId == userId.ToString())
					 ?? throw new Exception($"The user membership is not found for the user with id={userId} and the course with id={courseId}");

			uc.IsActive = false;
			uc.DateJoin = null;

			_db.Entry(uc).State = EntityState.Modified;
			_db.SaveChanges();

			return true;
		}

		public bool Approve(int membershipId) {
			var userCourse = _db.UserCourses.FirstOrDefault(uc => uc.Id == membershipId)
				?? throw new Exception($"The user membership with id={membershipId} is not found");

			userCourse.DateJoin = DateTime.Now;

			_db.Entry(userCourse).State = EntityState.Modified;
			_db.SaveChanges();

			return true;
		}

		public bool Approve(List<int> membershipIds) {
			var memberships = _db.UserCourses.Where(uc => membershipIds.Contains(uc.Id));
			foreach (var membership in memberships) {
				membership.DateJoin = DateTime.Now;
				_db.Entry(membership).State = EntityState.Modified;
			}

			_db.SaveChanges();

			return true;
		}


		public Guid Create(CourseDto course) {
			var mappedCourse = Mapper.Map<Course>(course);
			mappedCourse.Id = Guid.NewGuid();
			mappedCourse.DateCreated = DateTime.Now;
			mappedCourse.Timeline = new List<TimelineEntry>
			{
				new TimelineEntry
				{
					Heading = "Ders oluşturuldu",
					CreatorId = mappedCourse.CreatorId,
					DateCreated = DateTime.Now,
					IconClass = "ti ti-plus"
				}
			};

			mappedCourse = _db.Courses.Add(mappedCourse);
			_db.SaveChanges();

			mappedCourse.CopiedFromId = mappedCourse.Id;
			_db.Entry(mappedCourse).State = EntityState.Modified;
			_db.SaveChanges();

			return mappedCourse.Id;
		}

		public Guid Duplicate(DuplicateCourseRequestDto request) {
			var userId = HttpContext.Current.User.Identity.GetUserId();
			var courseToDuplicate = _db.Courses
				.Include("Chapters")
				.Include("Assignments")
				.Include("Chapters.Materials")
				.FirstOrDefault(c => c.Id == request.Id);
			if (courseToDuplicate == null)
				throw new Exception($"A course with id={request.Id} cannot be found");

			#region Duplicate Course
			var newCourse = new Course {
				CopiedFromId = courseToDuplicate.CopiedFromId,
				Description = courseToDuplicate.Description,
				CanEveryoneJoin = courseToDuplicate.CanEveryoneJoin,
				Code = request.NewCode,
				Name = request.NewName,
				CreatorId = userId,
				DateCreated = DateTime.Now,
				IsCourseActive = false,
				Assignments = new List<Assignment>(),
				Chapters = new List<Chapter>(),
				Timeline = new List<TimelineEntry> {
					new TimelineEntry {
						Heading = $"<b>{courseToDuplicate.Name}</b> dersi için yeni dönem oluşturuldu",
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

			return courses.Distinct().Select(t => new ClaimDto { Key = PleClaimType.Member, Value = t.Id.ToString() });
		}
		
		public IEnumerable<ClaimDto> GetViewerClaims(string userId) {
			var courses = _db.UserCourses.Where(uc => uc.UserId == userId && uc.IsActive && uc.DateJoin != null).Select(uc => uc.Course).ToList();
			foreach (var userCourse in courses.ToList()) // Include terms of courses
				courses.AddRange(_db.Courses.Where(c => c.CopiedFromId == userCourse.CopiedFromId));

			return courses.Distinct().Select(t => new ClaimDto { Key = PleClaimType.Viewer, Value = t.Id.ToString() });
		}

		public IEnumerable<ClaimDto> GetCreatorClaims(string userId) {
			var courses = _db.Courses.Where(c => c.CreatorId == userId);
			return courses.Select(t => new ClaimDto { Key = PleClaimType.Creator, Value = t.Id.ToString() });
		}

		public IEnumerable<ClaimDto> GetAllClaims(string userId) {
			return new List<ClaimDto>()
				.Concat(GetWaitingClaims(userId))
				.Concat(GetCreatorClaims(userId))
				.Concat(GetViewerClaims(userId))
				.Concat(GetMemberClaims(userId));
		}

		#region Private Methods

		private Course GetActiveCourseById(Guid courseId) {
			return _db.Courses.FirstOrDefault(i => i.Id == courseId && i.IsCourseActive)
				   ?? throw new Exception($"Active course wit id={courseId} does not exist");
		}


		#endregion
	}
}