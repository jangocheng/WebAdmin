using System;
using System.Collections.Generic;

namespace MSDev.Work.Entities
{
    public class LiveEvent
	{
		public object LiveEventId { get; set; }
		public object RegistrationUrl { get; set; }
		public object EventUrl { get; set; }
		public List<object> EventSchedule { get; set; }
	}

	public class AuthorInfo
	{
		public int AuthorId { get; set; }
		public string CreateDatetime { get; set; }
		public string ChangeDatetime { get; set; }
		public int CreatedByUserId { get; set; }
		public int UpdatedByUserId { get; set; }
		public object IsActive { get; set; }
		public object TwitterHandle { get; set; }
		public object PersonalBio { get; set; }
		public object ImageUrl { get; set; }
		public object PersonalWebsite { get; set; }
		public object EmailAddress { get; set; }
		public string DisplayName { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string JobTitle { get; set; }
		public string Company { get; set; }
	}

	public class ReplacementCourseDetails
	{
		public int CourseId { get; set; }
		public object CourseNumber { get; set; }
		public object LanguageCode { get; set; }
		public object LanguageId { get; set; }
		public object CourseName { get; set; }
		public object Version { get; set; }
		public int ProductPackageVersionId { get; set; }
	}

	public class CourseRating
	{
		public string RatingCount { get; set; }
		public string AverageRatingValue { get; set; }
		public int ChannelId { get; set; }
	}

	public class MvaEntity
	{
		public int Id { get; set; }
		public string CourseNumber { get; set; }
		public string CourseLevel { get; set; }
		public string LanguageCode { get; set; }
		public string LanguageId { get; set; }
		public string CourseName { get; set; }
		public string CourseShortDescription { get; set; }
		public string CourseDuration { get; set; }
		public string CourseImage { get; set; }
		public DateTime PublishedTime { get; set; }
		public object RetirementTime { get; set; }
		public string CourseStatus { get; set; }
		public int ProductPackageVersionId { get; set; }
		public int TotalModules { get; set; }
		public string Version { get; set; }
		public string Tags { get; set; }
		public int Points { get; set; }
		public LiveEvent LiveEvent { get; set; }
		public object ProductPackageVersionTypeName { get; set; }
		public DateTime LastUpdated { get; set; }
		public bool IsLiveEventExisting { get; set; }
		public object LiveEventStartDate { get; set; }
		public object CourseFormats { get; set; }
		public bool IsVersionModified { get; set; }
		public List<string> Technologies { get; set; }
		public List<string> TechnologiesCategory { get; set; }
		public List<string> Audiences { get; set; }
		public List<string> Topics { get; set; }
		public List<AuthorInfo> AuthorInfo { get; set; }
		public ReplacementCourseDetails ReplacementCourseDetails { get; set; }
		public List<CourseRating> CourseRatings { get; set; }
	}

	public class MvaApi
	{
		public List<MvaEntity> Results { get; set; }
		public int TotalResultCount { get; set; }

		public List<object> NarrowBySections { get; set; }
		public string SearchIndexUpdatedDateTime { get; set; }
	}
}
