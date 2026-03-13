using System;

namespace EWTSS_DESKTOP.Core.Models
{
	public abstract class BaseEntity
	{
		public int Id { get; set; }

		public DateTime CreatedOn { get; set; } = DateTime.Now;

		public DateTime UpdatedOn { get; set; } = DateTime.Now;

		public bool IsActive { get; set; } = true;
	}
}