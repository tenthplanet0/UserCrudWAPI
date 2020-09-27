using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserCrudWAPI.Models;
using UserCrudWAPI.Controllers;
using System.Web.Http.Results;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace UserCrudWAPI.Tests
{
	[TestClass]
	public class UsersUnitTest
	{
		[TestMethod]
		public void GetUsersFromSearchTestMethod()
		{
			var searchText = "m";
			//Arrange
			var testUsers = UsersController.Users.Where(u => u.FirstName.ToLower().Contains(searchText));
			var controller = new UsersController();

			//Act
			var actionResult = controller.GetUsers(searchText);
			var contentResult = actionResult as OkNegotiatedContentResult<IEnumerable<User>>;
			int expRes = testUsers.Count(), reqRes = contentResult.Content.Count();
			//Assert
			Assert.AreEqual(testUsers.Count(), contentResult.Content.Count());
		}
		[TestMethod]
		public void GetUsersFromSearchReturnsNotFound()
		{
			// Arrange
			UsersController controller = new UsersController();
			// Act
			IHttpActionResult actionResult = controller.GetUsers("k");
			// Assert
			Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
		}
		[TestMethod]
		public void GetUserReturnsUserWithSameID()
		{
			// Arrange
			UsersController controller = new UsersController();

			// Act
			IHttpActionResult actionResult = controller.GetUser(2);
			var contentResult = actionResult as OkNegotiatedContentResult<User>;

			// Assert
			Assert.IsNotNull(contentResult);
			Assert.IsNotNull(contentResult.Content);
			Assert.AreEqual(2, contentResult.Content.Id);
		}
		[TestMethod]
		public void GetUserReturnsNotFound()
		{
			// Arrange
			UsersController controller = new UsersController();
			// Act
			IHttpActionResult actionResult = controller.GetUser(10);
			// Assert
			Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
		}
		[TestMethod]
		public void GetUsersReturnsListOfUsers()
		{
			//Arrange
			var testUsers = UsersController.Users;
			var controller = new UsersController();

			//Act
			var actionResult = controller.GetUsers() as List<User>;

			//Assert
			Assert.AreEqual(actionResult.Count, testUsers.Count);
		}
		[TestMethod]
		public void PostMethodSetsLocationHeader()
		{
			//Arrange
			var testUsers = UsersController.Users;
			UsersController controller = new UsersController();

			//Act
			var actionResult = controller.Post(new User() { FirstName = "Jeff", LastName = "Bezos", DoB = "27-09-1950", Email1 = "jeffbezos@amazon.inc", Contact1 = "+17036113055" });
			var createdResult = actionResult as CreatedAtRouteNegotiatedContentResult<User>;

			//Assert
			Assert.IsNotNull(createdResult);
			Assert.AreEqual("DefaultApi", createdResult.RouteName);
			Assert.IsNotNull(createdResult .Content);
			Assert.AreEqual(testUsers.Count, createdResult.RouteValues["id"]);
		}
		[TestMethod]
		public void PostReturnsBadRequest()
		{
			//Arrange
			var testUsers = UsersController.Users;
			UsersController controller = new UsersController();

			//Act
			var actionResult = controller.Post(new User() {DoB="06-04-1986", Email1="abcdefcom"});

			//Assert
			Assert.IsInstanceOfType(actionResult, typeof(BadRequestErrorMessageResult));
		}

		[TestMethod]
		public void PutReturnsContentResult()
		{
			//Arrange
			UsersController controller = new UsersController();

			//Act
			var actionResult = controller.Put(new User() {Id=2, FirstName = "Steve", LastName = "Jobs", DoB = "27-09-1950", Email1 = "stevejobs@apple.inc", Contact1="+17036113055" });
			var contentResult = actionResult as NegotiatedContentResult<User>;

			//Assert
			Assert.IsNotNull(contentResult);
			Assert.AreEqual(HttpStatusCode.Accepted, contentResult.StatusCode);
			Assert.IsNotNull(contentResult.Content);
			//int reqId = 2, conRes = contentResult.Content.Id;
			Assert.AreEqual(2, contentResult.Content.Id);

		}
		[TestMethod]
		public void PutReturnsBadRequest()
		{
			//Arrange
			var testUsers = UsersController.Users;
			UsersController controller = new UsersController();

			//Act
			var actionResult = controller.Put(new User() { DoB = "06-04-1986", Email1 = "abcdefcom" });

			//Assert
			Assert.IsInstanceOfType(actionResult, typeof(BadRequestErrorMessageResult));
		}
		[TestMethod]
		public void PutReturnsNotFound()
		{
			//Arrange
			var testUsers = UsersController.Users;
			UsersController controller = new UsersController();

			//Act
			var actionResult = controller.Put(new User() {Id=10, DoB = "06-04-1986", Email1 = "abc@def.com" });

			//Assert
			Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
		}
	}
}
