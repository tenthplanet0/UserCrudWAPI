using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace UserCrudWAPI.Models
{
	public class User
	{
		public int Id { get; set; }
		[Required]
		public string FirstName { get; set; }
		[Required]
		public string LastName { get; set; }
		public string DoB { get; set; }

		[DataType(DataType.EmailAddress)]
		public string Email1 { get; set; }
		[DataType(DataType.EmailAddress)]
		public string Email2 { get; set; }
		[DataType(DataType.EmailAddress)]
		public string Email3 { get; set; }

		public string Contact1 { get; set; }
		public string Contact2 { get; set; }
		public string Contact3 { get; set; }
	}
}