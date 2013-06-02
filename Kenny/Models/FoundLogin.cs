using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kenny.Models
{
    public class FoundLogin
    {
        public int Id { get; set; }

		[Required]
        public Site Site { get; set; }

		[Required]
        public string Username { get; set; }

		[Required]
        public string Password { get; set; }

		[Display(Name = "Valid?")]
		public bool? IsValid { get; set; }

		[Display(Name = "Source")]
		public string SourceUrl { get; set; }

		[Display(Name = "Date Collected")]
		public DateTime? DateCollected { get; set; }
    }
}