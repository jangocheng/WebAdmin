using System.ComponentModel.DataAnnotations;

namespace WebAdmin.FormModels.Config
{
	public class ConfigForm
	{

		/// <summary>
		/// 名称
		/// </summary>
		[Required]
		[MaxLength(32)]
		public string Name { get; set; }

		/// <summary>
		/// 类型
		/// </summary>
		[Required]
		[MaxLength(32)]
		public string Type { get; set; }

		/// <summary>
		/// 值
		/// </summary>
		[Required]
		[MaxLength(4000)]
		public string Value { get; set; }
	}
}