using TownHall.Core;

namespace TownHall.Tests
{
	public class UserServiceTests : TestBase
	{
		[Test]
		public void Test_Login_ReturnsTrue_WithValidCredentials()
		{
			// Arrange
			var users = CreateTestUsers();
			var testUser = users.First();
			MockValidateCredentials(testUser.Email, testUser.Password, testUser);

			// Act
			var result = userService.Login(testUser.Email, testUser.Password);

			// Assert
			Assert.True(result);
			Assert.That(GlobalCurrentUser.User, Is.EqualTo(testUser));
		}

		[Test]
		public void Test_Login_ReturnsFalse_WithInvalidCredentials()
		{
			// Arrange
			MockValidateCredentials("invalid@email.com", "wrongpassword", null);

			// Act
			var result = userService.Login("invalid@email.com", "wrongpassword");

			// Assert
			Assert.False(result);
			Assert.That(GlobalCurrentUser.User.FirstName, Is.Null);
		}

		[Test]
		public void Test_CreateUser_ReturnsNewUser_WithCorrectProperties()
		{
			// Arrange
			var email = "test@example.com";
			var password = "password123";
			var firstName = "John";
			var lastName = "Doe";
			var phone = "123-456-7890";
			var address = "123 Main St";

			var expectedUser = new User
			{
				Id = Guid.NewGuid(),
				Email = email,
				Password = password,
				FirstName = firstName,
				LastName = lastName,
				Phone = phone,
				Address = address
			};

			MockAddUser(expectedUser);

			// Act
			var result = userService.CreateUser(email, password, firstName, lastName, phone, address);

			// Assert
			Assert.That(result.Email, Is.EqualTo(email));
			Assert.That(result.Password, Is.EqualTo(password));
			Assert.That(result.FirstName, Is.EqualTo(firstName));
			Assert.That(result.LastName, Is.EqualTo(lastName));
			Assert.That(result.Phone, Is.EqualTo(phone));
			Assert.That(result.Address, Is.EqualTo(address));
			Assert.That(result.Id, Is.Not.EqualTo(Guid.Empty));
		}

		[Test]
		public void Test_GetUsers_ReturnsAllUsers()
		{
			// Arrange
			var users = CreateTestUsers();
			MockGetUsers(users);

			// Act
			var result = userService.GetUsers();

			// Assert
			Assert.That(result.Count, Is.EqualTo(users.Count));
			Assert.That(result, Is.EqualTo(users));
		}

		[Test]
		public void Test_GetUsers_ReturnsEmptyList_WhenNoUsers()
		{
			// Arrange
			var emptyUserList = new List<User>();
			MockGetUsers(emptyUserList);

			// Act
			var result = userService.GetUsers();

			// Assert
			Assert.That(result.Count, Is.EqualTo(0));
			Assert.That(result, Is.EqualTo(emptyUserList));
		}

		[Test]
		public void Test_GetUserById_ReturnsCorrectUser()
		{
			// Arrange
			var users = CreateTestUsers();
			var targetUser = users.First();
			MockGetUserById(targetUser.Id, targetUser);

			// Act
			var result = userService.GetUserById(targetUser.Id);

			// Assert
			Assert.That(result, Is.EqualTo(targetUser));
		}

		[Test]
		public void Test_GetUserById_ReturnsNull_WhenUserNotFound()
		{
			// Arrange
			var nonExistentId = Guid.NewGuid();
			MockGetUserById(nonExistentId, null);

			// Act
			var result = userService.GetUserById(nonExistentId);

			// Assert
			Assert.That(result, Is.Null);
		}
	}
}