using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EasyAuth.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]    
    public class MemoryUserStoreTests
    {
        private IUserStore userStore;

        [TestInitialize]
        public void TestInitialize()
        {
            userStore = new MemoryUserStore();
        }

        [TestCleanup]
        public void TestCleanup()
        {            
            userStore.Dispose();
        }

        #region AddUser tests
        [TestMethod]
        public void AddUser_GivenNewUser_UserAdded()
        {
            string username = "testuser", password = "testpass";
            userStore.AddUser(username, password);

            UserData user = userStore.GetUserByUsername(username);
            var actual = user.Username;
            var expected = username;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(EasyAuth.UserAlreadyExistsException))]
        public void AddUser_GivenAlreadyExistingUsername_ThrowsException()
        {
            string username = "testuser", password = "testpass";
            userStore.AddUser(username, password);

            userStore.AddUser(username, password);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void AddUser_GivenEmptyFirstArgument_ThrowsException()
        {
            userStore.AddUser("", "password");
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void AddUser_GivenEmptySecondArgument_ThrowsException()
        {
            userStore.AddUser("username", "");
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void AddUser_GivenNullFirstArgument_ThrowsException()
        {
            userStore.AddUser(null, "password");
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void AddUser_GivenNullSecondArgument_ThrowsException()
        {
            userStore.AddUser("username", null);
        }
        #endregion

        #region DeleteUser tests
        [TestMethod]
        public void DeleteUser_GivenExistingUserId_UserDeleted()
        {
            string username = "testuser", password = "testpass";
            userStore.AddUser(username, password);

            userStore.DeleteUserById(userStore.GetUserByUsername(username).UserId);
            var actual = userStore.UserExistsByUsername(username);

            Assert.IsFalse(actual);
        }

        [TestMethod]
        [ExpectedException(typeof(EasyAuth.UserDoesNotExistException))]
        public void DeleteUser_GivenNonExistantUserId_ThrowsException()
        {
            userStore.DeleteUserById(395);
        }
        #endregion

        #region UpdateUser tests
        [TestMethod]
        public void UpdateUser_GivenExistingUser_UserUpdated()
        {
            string username = "testuser", password = "testpass", newUsername = "newusername";
            userStore.AddUser(username, password);
            UserData user = userStore.GetUserByUsername(username);
            
            user.Username = newUsername;
            userStore.UpdateUserById(user.UserId, user);
            var actual = userStore.UserExistsByUsername(newUsername);

            Assert.IsTrue(actual);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void UpdateUser_GivenNullSecondArgument_ThrowsException()
        {
            string username = "testuser", password = "testpass";
            userStore.AddUser(username, password);
            UserData user = userStore.GetUserByUsername(username);

            userStore.UpdateUserById(user.UserId, null);
        }

        [TestMethod]
        [ExpectedException(typeof(EasyAuth.UserDoesNotExistException))]
        public void UpdateUser_GivenNonExistantUserId_ThrowsException()
        {
            string username = "testuser", password = "testpass";
            userStore.AddUser(username, password);
            UserData user = userStore.GetUserByUsername(username);

            userStore.UpdateUserById(26, user);
        }

        [TestMethod]
        [ExpectedException(typeof(EasyAuth.UserIdDoesNotMatchUserObjectId))]
        public void UpdateUser_GivenUserIdThatDoesNotMatchUserObjectId_ThrowsException()
        {
            userStore.AddUser("user1", "password");
            userStore.AddUser("user2", "password");
            UserData user1 = userStore.GetUserByUsername("user1");
            UserData user2 = userStore.GetUserByUsername("user2");

            userStore.UpdateUserById(user1.UserId, user2);
        }
        #endregion

        #region UserExistsById tests
        [TestMethod]
        public void UserExistsById_GivenExistingUserId_ReturnsTrue()
        {
            userStore.AddUser("user1", "password");            
            UserData user1 = userStore.GetUserByUsername("user1");

            var actual = userStore.UserExistsById(user1.UserId);

            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void UserExistsById_GivenNonExistantUserId_ReturnsFalse()
        {
            var actual = userStore.UserExistsById(236);

            Assert.IsFalse(actual);
        }
        #endregion

        #region UserExistsByUsername tests
        [TestMethod]
        public void UserExistsByUsername_GivenExistingUsername_ReturnsTrue()
        {
            userStore.AddUser("user1", "password");

            var actual = userStore.UserExistsByUsername("user1");

            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void UserExistsByUsername_GivenNonExistantUserId_ReturnsFalse()
        {
            var actual = userStore.UserExistsByUsername("doesnotexist");

            Assert.IsFalse(actual);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void UserExistsByUsername_GivenNull_ThrowsException()
        {
            userStore.UserExistsByUsername(null);
        }
        #endregion

        #region GetUserById tests
        [TestMethod]
        public void GetUserById_GivenExistingUserId_ReturnsCorrectUser()
        {
            string username = "user1";
            userStore.AddUser(username, "password");
            UserData user1 = userStore.GetUserByUsername(username);

            UserData user = userStore.GetUserById(user1.UserId);
            var expected = username;
            var actual = user.Username;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void GetUserById_GivenNegativeId_ThrowsException()
        {
            userStore.GetUserById(-20);
        }

        [TestMethod]
        [ExpectedException(typeof(EasyAuth.UserDoesNotExistException))]
        public void GetUserById_GivenNonExistantUserId_ThrowsException()
        {
            userStore.GetUserById(12);
        }

        [TestMethod]
        public void GetUserById_UserDataChangedWithoutUpdate_StoredUserDataNotAffected()
        {
            string username = "user1";
            userStore.AddUser(username, "password");
            int userId = userStore.GetUserByUsername(username).UserId;

            UserData userNotUpdated = userStore.GetUserById(userId);
            userNotUpdated.Username = "bogususername";
            UserData userOriginal = userStore.GetUserById(userId);
            var expected = username;
            var actual = userOriginal.Username;

            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region GetUserByUsername tests
        [TestMethod]
        public void GetUserByUsername_GivenExistingUsername_ReturnsCorrectUser()
        {
            string username = "user1";
            userStore.AddUser(username, "password");
            
            UserData user = userStore.GetUserByUsername(username);
            var expected = username;
            var actual = user.Username;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void GetUserByUsername_GivenNullArgument_ThrowsException()
        {
            userStore.GetUserByUsername(null);
        }

        [TestMethod]
        [ExpectedException(typeof(EasyAuth.UserDoesNotExistException))]
        public void GetUserByUsername_GivenNonExistantUsername_ThrowsException()
        {
            userStore.GetUserByUsername("NotAValidUser");
        }

        [TestMethod]
        public void GetUserByUsername_UserDataChangedWithoutUpdate_StoredUserDataNotAffected()
        {
            string username = "user1";
            userStore.AddUser(username, "password");

            UserData userNotUpdated = userStore.GetUserByUsername(username);
            userNotUpdated.Username = "bogususername";
            UserData userOriginal = userStore.GetUserByUsername(username);
            var expected = username;
            var actual = userOriginal.Username;

            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}
