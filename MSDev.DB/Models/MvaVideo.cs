using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MSDev.DB.Models
{
	public class MvaVideo : Model
	{
		public int? MvaId { get; set; }

		[MaxLength(512)]
		public string SourceUrl { get; set; }

		[MaxLength(128)]
		public string CourseNumber { get; set; }
		/// <summary>
		/// Beginner,Intermediate,Advanced
		/// </summary>
		[MaxLength(32)]
		public string CourseLevel { get; set; }
		/// <summary>
		/// zh-cn
		/// </summary>
		[MaxLength(16)]
		public string LanguageCode { get; set; }
		[MaxLength(256)]
		public string Title { get; set; }
		[MaxLength(4000)]
		public string Description { get; set; }

		[MaxLength(32)]
		public string CourseDuration { get; set; }
		[MaxLength(512)]
		public string CourseImage { get; set; }

		/// <summary>
		/// published
		/// </summary>
		[MaxLength(32)]
		public string CourseStatus { get; set; }

		public int? ProductPackageVersionId { get; set; }

		[MaxLength(256 + 128)]
		public string Tags { get; set; }

		[MaxLength(256 + 128)]
		public string Technologies { get; set; }
		[MaxLength(512 + 256)]
		public string Author { get; set; }

		[MaxLength(256 + 128)]
		public string AuthorCompany { get; set; }
		[MaxLength(1024)]
		public string AuthorJobTitle { get; set; }
	}


}
