using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using UserCrudWAPI.Models;
using System.Net.Mail;
using System.ComponentModel.DataAnnotations;

namespace UserCrudWAPI.Controllers
{
    public class UsersController : ApiController
    {
		public static List<User> Users = new List<User>()
		{
			new User(){Id=1, FirstName="Jacob", LastName="Stayn", DoB="01-31-1986", Email1="jacob@mail.com", Email2="jacob2@mail.com", Email3="jacob3@gmail.com", Contact1="1523463"},
			new User(){Id=2, FirstName="Michael", LastName="Johnson", DoB="01-31-1986", Email1="michael@mail.com", Email2="michael2@mail.com", Email3="michael3@gmail.com", Contact1="1523463"},
		};

		[HttpGet]
		public IEnumerable<User> GetUsers()
		{
			return Users;
		}

		[HttpGet]
		public IHttpActionResult GetUser(int id)
		{
			User requestedUser = Users.FirstOrDefault(u => u.Id == id);
			if (requestedUser == null)
				return NotFound();
			return Ok(requestedUser);
		}

		[HttpGet]
		public IHttpActionResult GetUsers(string searchtext)
		{
			var users = from c in Users
						  where c.FirstName.ToLower().Contains(searchtext.ToLower())
						  select c;
			//var users = Users.Where(u => u.FirstName.ToLower().Contains(searchtext)).FirstOrDefault();
			if (users.Count() ==0 || users == null)
				return NotFound();
			return Ok(users);
		}

		[HttpPost]
		[ResponseType(typeof(User))]
		public IHttpActionResult Post([FromBody]User user)
		{
			if (user.FirstName == null || user.LastName == null)
			{
				return BadRequest("Invalid passed data");
			}

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			user.Id = Users.Count;
			Users.Add(user);

			return CreatedAtRoute("DefaultApi", new { id = Users.Count}, user);
		}

		[HttpPut]
		public IHttpActionResult Put([FromBody]User user)
		{
			if (!ModelState.IsValid || !IsNullOrValidEmail(user.Email1)|| !IsNullOrValidEmail(user.Email2)|| !IsNullOrValidEmail(user.Email3))
				return BadRequest("Not a valid model");

			try{ 
				var existingUser = Users.FirstOrDefault(u=>u.Id == user.Id);
				if (existingUser != null)
				{
					existingUser.FirstName = user.FirstName;
					existingUser.LastName = user.LastName;
					existingUser.DoB = user.DoB;
					existingUser.Email1 = user.Email1;
					existingUser.Email2 = user.Email2;
					existingUser.Email3 = user.Email3;
					existingUser.Contact1 = user.Contact1;
					existingUser.Contact2 = user.Contact2;
					existingUser.Contact3 = user.Contact3;
				}
				else
					return NotFound();

				//return Ok(); 
				return Content(HttpStatusCode.Accepted, user);
			}
			catch
			{
				return NotFound();
			}
		}

		private bool IsNullOrValidEmail(string email)
		{
			if (string.IsNullOrEmpty(email))
				return true;
			else
			{
				try
				{
					var mail = new MailAddress(email);
					return mail.Host.Contains(".");
				}
				catch (Exception)
				{
					return false;
				}
			}
		}
	}
}
