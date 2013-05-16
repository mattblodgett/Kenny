using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kenny.Models
{
	public class Site
	{
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }

		[Required]
		[Display(Name = "Base URL")]
		public string BaseUrl { get; set; }

		[Required]
		[Display(Name = "Authenticated URL")]
		public string AuthenticatedUrl { get; set; }

		public int OwnerId { get; set; }

		public virtual UserProfile Owner { get; set; }
	}
}