using FluentAssertions;
using Caminhao.Api.Domain.UserAggregate;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Caminhao.Api.Test.Controllers
{
    public class UserControllerTest : TestStartup
    {
        private readonly string route = "user/";

        [Fact]
        public async Task PostNewUserShouldReturnHttpStatusCodeOK()
        {
            User user = new User
            {
                Firstname = "Luzolo",
                Lastname = "Lusembo",
                Middlename = "Plamedi",
                Sex = Sex.Male,
                Email = "plam.l@live.fr",
                Username = "pllusembo",
                Password = "12345"
            };

            HttpResponseMessage response = await _testClient.PostAsJsonAsync($"{route}create", user);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            User user1 = await response.Content.ReadAsAsync<User>();
            user.Id = user1.Id;
            Assert.True(user.Equals(user1));
        }

        [Fact]
        public async Task PostRequestToCreateNewUserShouldReturnUserCreatedEqualPostedOne()
        {
            User user = new User
            {
                Firstname = "Luzolo",
                Lastname = "Lusembo",
                Middlename = "Plamedi",
                Sex = Sex.Male,
                Email = "plam.l@live.com",
                Username = "pllusembo",
                Password = "12345"
            };

            HttpResponseMessage response = await _testClient.PostAsJsonAsync($"{route}create", user);
            User user1 = await response.Content.ReadAsAsync<User>();
            user.Id = user1.Id;
            user1 = null;
            user1.Equals(user).Should().BeTrue();
        }

        [Fact]
        public async Task PostRequestToCreateExistingUserShouldReturnHttpStatusCodeConfilict()
        {
            User user = new User
            {
                Firstname = "Luzolo",
                Lastname = "Lusembo",
                Middlename = "Plamedi",
                Sex = Sex.Male,
                Email = "plam.l@live.fr",
                Username = "pllusembo",
                Password = "12345"
            };

            await _testClient.PostAsJsonAsync($"{route}create", user);

            HttpResponseMessage response = await _testClient.PostAsJsonAsync($"{route}create", user);
            response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task PostRequestWithBodyNullToCreateUserShouldReturnHttpStatusCodeBadRequest()
        {
            HttpResponseMessage response2 = await _testClient.PostAsJsonAsync<User>($"{route}create", null);
            response2.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetRequestToGetAllUsersShouldReturnHttpStatusCodeOK()
        {
            User user = new User
            {
                Firstname = "Luzolo",
                Lastname = "Nzuzi",
                Middlename = "Zouzou",
                Sex = Sex.Female,
                Email = "elsa.l@live.fr",
                Username = "elsa",
                Password = "12345"
            };

            await _testClient.PostAsJsonAsync($"{route}create", user);

            HttpResponseMessage response = await _testClient.GetAsync($"{route}getall");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetRequestToGetAllUsersShouldReturnListOfUsers()
        {
            User user = new User
            {
                Firstname = "Luzolo",
                Lastname = "Nzuzi",
                Middlename = "Zouzou",
                Sex = Sex.Female,
                Email = "elsa.l@live.fr",
                Username = "elsa",
                Password = "12345"
            };

            await _testClient.PostAsJsonAsync($"{route}create", user);

            HttpResponseMessage response = await _testClient.GetAsync($"{route}getall");
            IList<User> users = await response.Content.ReadAsAsync<List<User>>();
            users.Should().NotBeEmpty();
        }

        [Fact]
        public async Task TestGetById()
        {
            User user = new User
            {
                Firstname = "Luzolo",
                Lastname = "Matanu",
                Middlename = "Herv?",
                Sex = Sex.Male,
                Email = "herve.l@live.fr",
                Username = "hlm",
                Password = "12345"
            };

            HttpResponseMessage response = await _testClient.PostAsJsonAsync($"{route}create", user);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            User user1 = await response.Content.ReadAsAsync<User>();

            HttpResponseMessage response1 = await _testClient.GetAsync($"{route}getbyid?id=" + user1.Id);
            response1.StatusCode.Should().Be(HttpStatusCode.OK);
            User u = await response1.Content.ReadAsAsync<User>();
            u.Equals(user1).Should().BeTrue();

            HttpResponseMessage response2 = await _testClient.DeleteAsync($"{route}delete?id=" + user1.Id);
            response2.StatusCode.Should().Be(HttpStatusCode.OK);

            HttpResponseMessage response3 = await _testClient.GetAsync($"{route}getbyid?id=" + user1.Id);
            response3.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task TestDelete()
        {
            User user = new User
            {
                Firstname = "Luzolo",
                Lastname = "Nsambu",
                Middlename = "Nadine",
                Sex = Sex.Female,
                Email = "nadine.l@live.fr",
                Username = "nana",
                Password = "12345"
            };

            HttpResponseMessage response = await _testClient.PostAsJsonAsync($"{route}create", user);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            User user1 = await response.Content.ReadAsAsync<User>();

            HttpResponseMessage response1 = await _testClient.DeleteAsync($"{route}delete?id=" + user1.Id);
            response1.StatusCode.Should().Be(HttpStatusCode.OK);

            HttpResponseMessage response2 = await _testClient.GetAsync($"{route}getbyid?id=" + user1.Id);
            response2.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task TestLogin()
        {
            User user = new User
            {
                Firstname = "Luzolo",
                Lastname = "Makiese",
                Middlename = "Patricia",
                Sex = Sex.Female,
                Email = "patou.l@live.fr",
                Username = "ptcia",
                Password = "12345"
            };

            HttpResponseMessage response = await _testClient.PostAsJsonAsync($"{route}create", user);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            User user1 = await response.Content.ReadAsAsync<User>();

            HttpResponseMessage response1 = await _testClient.GetAsync($"{route}getbyid?id=" + user1.Id);
            response1.StatusCode.Should().Be(HttpStatusCode.OK);
            User u = await response1.Content.ReadAsAsync<User>();
            u.Equals(user1).Should().BeTrue();

            HttpResponseMessage response2 = await _testClient.PostAsJsonAsync($"{route}login", user1);
            response2.StatusCode.Should().Be(HttpStatusCode.OK);
            User u1 = await response2.Content.ReadAsAsync<User>();
            u1.Equals(user1).Should().BeTrue();

            User user2 = new User
            {
                Email = "patou.l@live.fr",
                Password = "12345"
            };

            HttpResponseMessage response3 = await _testClient.PostAsJsonAsync($"{route}login", user2);
            response3.StatusCode.Should().Be(HttpStatusCode.OK);
            User u2 = await response3.Content.ReadAsAsync<User>();
            u2.Equals(user1).Should().BeTrue();

            User user3 = new User
            {
                Username = "ptcia",
                Password = "12345"
            };

            HttpResponseMessage response4 = await _testClient.PostAsJsonAsync($"{route}login", user3);
            response4.StatusCode.Should().Be(HttpStatusCode.OK);
            User u3 = await response4.Content.ReadAsAsync<User>();
            u3.Equals(user1).Should().BeTrue();

            user2.Email = "pato.l@live.fr";

            HttpResponseMessage response5 = await _testClient.PostAsJsonAsync($"{route}login", user2);
            response5.StatusCode.Should().Be(HttpStatusCode.NoContent);
            User u4 = await response4.Content.ReadAsAsync<User>();
            u4.Should().BeNull();

            user3.Username = "ptci";

            HttpResponseMessage response6 = await _testClient.PostAsJsonAsync($"{route}login", user3);
            response6.StatusCode.Should().Be(HttpStatusCode.NoContent);
            User u5 = await response6.Content.ReadAsAsync<User>();
            u5.Should().BeNull();

            user2.Email = "patou.l@live.fr";
            user2.Password = "1234";

            HttpResponseMessage response7 = await _testClient.PostAsJsonAsync($"{route}login", user2);
            response7.StatusCode.Should().Be(HttpStatusCode.NoContent);
            User u6 = await response7.Content.ReadAsAsync<User>();
            u6.Should().BeNull();

            HttpResponseMessage response8 = await _testClient.DeleteAsync($"{route}delete?id=" + user1.Id);
            response8.StatusCode.Should().Be(HttpStatusCode.OK);

            HttpResponseMessage response9 = await _testClient.PostAsJsonAsync($"{route}login", user1);
            response9.StatusCode.Should().Be(HttpStatusCode.NoContent);
            (await response9.Content.ReadAsAsync<User>()).Should().BeNull();
        }

        //Not implementated yet
        [Fact]
        public async Task TestUpdate()
        {
            User user = new User
            {
                Firstname = "Lusembo",
                Lastname = "Kekese",
                Middlename = "Antoinette",
                Sex = Sex.Male,
                Email = "kemi.l@live.fr",
                Username = "kemi",
                Password = "12345"
            };

            var response = await _testClient.PostAsJsonAsync($"{route}create", user);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            User user1 = await response.Content.ReadAsAsync<User>();

            var response1 = await _testClient.GetAsync($"{route}getbyid?id=" + user1.Id);
            response1.StatusCode.Should().Be(HttpStatusCode.OK);
            User u = await response1.Content.ReadAsAsync<User>();
            u.Equals(user1).Should().BeTrue();

            User user2 = new User
            {
                Id = user1.Id,
                Firstname = "Lusembo",
                Lastname = "Kekese Kemi",
                Middlename = "Antoinette",
                Sex = Sex.Female,
                Email = "anto.l@live.fr",
                Username = "nenette",
                Password = "12345"
            };

            var response2 = await _testClient.PutAsJsonAsync($"{route}update", user2);
            response2.StatusCode.Should().Be(HttpStatusCode.OK);
            User u1 = await response2.Content.ReadAsAsync<User>();
            u1.Equals(user2).Should().BeTrue();
            u1.Equals(user1).Should().BeFalse();

        }

        [Fact]
        public async Task TestChangeProfil()
        {
            User user = new User
            {
                Firstname = "Sangi",
                Lastname = "Mohumbu",
                Middlename = "Naomi",
                Sex = Sex.Male,
                Email = "nao.l@live.fr",
                Username = "nao",
                Password = "12345"
            };

            var response = await _testClient.PostAsJsonAsync($"{route}create", user);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            User user1 = await response.Content.ReadAsAsync<User>();

            var response1 = await _testClient.GetAsync($"{route}getbyid?id=" + user1.Id);
            response1.StatusCode.Should().Be(HttpStatusCode.OK);
            User u = await response1.Content.ReadAsAsync<User>();
            u.Equals(user1).Should().BeTrue();

            User user2 = new User
            {
                Id = user1.Id,
                Firstname = "Sangi",
                Lastname = "Mohumbu",
                Middlename = "Naomie",
                Sex = Sex.Female,
                Email = "naomi.l@live.fr",
                Username = "naomi"
            };

            var response2 = await _testClient.PutAsJsonAsync($"{route}changeprofil", user2);
            response2.StatusCode.Should().Be(HttpStatusCode.OK);
            User u1 = await response2.Content.ReadAsAsync<User>();
            u1.Equals(user2).Should().BeTrue();
            u1.Equals(user1).Should().BeFalse();
        }

        private string GetEncrypt(string username)
        {
            char separetor = '#';
            string changePasswordToken = username;
            double timeout = 60;
            string datestr = DateTime.Now.ToString();

            string plain = $"{changePasswordToken}{separetor}{timeout}{separetor}{datestr}";
            return DesCryptography.Encrypt(plain);
        }

        [Fact]
        public async Task TestChangePassword()
        {
            User user = new User
            {
                Firstname = "Kahuma",
                Lastname = "Luzolo",
                Middlename = "Precieu",
                Sex = Sex.Male,
                Email = "precieu.l@live.fr",
                Username = "precieu",
                Password = "12345"
            };

            var response = await _testClient.PostAsJsonAsync($"{route}create", user);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            User user1 = await response.Content.ReadAsAsync<User>();

            var response1 = await _testClient.GetAsync($"{route}getbyid?id=" + user1.Id);
            response1.StatusCode.Should().Be(HttpStatusCode.OK);
            User u = await response1.Content.ReadAsAsync<User>();
            u.Equals(user1).Should().BeTrue();

            string token = GetEncrypt(u.Username);

            NewPassword userNewPassword = new NewPassword
            {
                Token = token,
                Username = u.Username,
                Password = "UtlzNewPsw123"
            };

            var response2 = await _testClient.PutAsJsonAsync($"{route}changepassword", userNewPassword);
            response2.StatusCode.Should().Be(HttpStatusCode.OK);
            User u1 = await response2.Content.ReadAsAsync<User>();
            u1.Equals(u).Should().BeFalse();
            u1.Equals(user1).Should().BeFalse();
            u1.Password.Equals(u.Password).Should().BeFalse();
            u1.Password.Equals(user1.Password).Should().BeFalse();

            var response3 = await _testClient.GetAsync($"{route}getbyid?id=" + user1.Id);
            response3.StatusCode.Should().Be(HttpStatusCode.OK);
            User u2 = await response3.Content.ReadAsAsync<User>();
            u2.Equals(u).Should().BeFalse();
            u2.Equals(user1).Should().BeFalse();
            u2.Password.Equals(u.Password).Should().BeFalse();
            u2.Password.Equals(user1.Password).Should().BeFalse();

        }

        [Fact]
        public async Task TestChangePasswordWithToken()
        {
            User user = new User
            {
                Firstname = "Nsimba",
                Lastname = "Nsimba",
                Middlename = "Dina",
                Sex = Sex.Male,
                Email = "dina.l@live.fr",
                Username = "dina",
                Password = "dina123"
            };

            var response = await _testClient.PostAsJsonAsync($"{route}create", user);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            User user1 = await response.Content.ReadAsAsync<User>();

            var response1 = await _testClient.GetAsync($"{route}getbyid?id=" + user1.Id);
            response1.StatusCode.Should().Be(HttpStatusCode.OK);
            User u = await response1.Content.ReadAsAsync<User>();
            u.Equals(user1).Should().BeTrue();

            NewPassword userNewPassword = new NewPassword
            {
                Username = u.Username,
                Password = "UtlzNewPsw123"
            };

            var response2 = await _testClient.PutAsJsonAsync($"{route}changepassword", userNewPassword);
            response2.StatusCode.Should().Be(HttpStatusCode.OK);
            User u1 = await response2.Content.ReadAsAsync<User>();
            u1.Equals(u).Should().BeFalse();
            u1.Equals(user1).Should().BeFalse();
            u1.Password.Equals(u.Password).Should().BeFalse();
            u1.Password.Equals(user1.Password).Should().BeFalse();

            var response3 = await _testClient.GetAsync($"{route}getbyid?id=" + user1.Id);
            response3.StatusCode.Should().Be(HttpStatusCode.OK);
            User u2 = await response3.Content.ReadAsAsync<User>();
            u2.Equals(u).Should().BeFalse();
            u2.Equals(user1).Should().BeFalse();
            u2.Password.Equals(u.Password).Should().BeFalse();
            u2.Password.Equals(user1.Password).Should().BeFalse();
        }

    }
}
