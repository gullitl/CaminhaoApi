using CaminhaoApi.Domain.CaminhaoAggregate;
using FluentAssertions;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace CaminhaoApi.Test.Controllers
{
    public class CaminhaoControllerTest : TestStartup
    {
        private readonly string route = "caminhao/";

        [Fact]
        public async Task PostNewUserShouldReturnHttpStatusCodeOK()
        {
            Caminhao caminhao = new()
            {
                Marca = "DAF",
                Modelo = Modelo.FH,
                AnoModelo = 2021
            };

            HttpResponseMessage response = await _testClient.PostAsJsonAsync($"{route}create", caminhao);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            Caminhao caminhao1 = await response.Content.ReadAsAsync<Caminhao>();

            caminhao.Id = caminhao1.Id;
            caminhao1.Equals(caminhao).Should().BeTrue();
        }

        [Fact]
        public async Task PostRequestWithBodyNullToCreateUserShouldReturnHttpStatusCodeBadRequest()
        {
            HttpResponseMessage response2 = await _testClient.PostAsJsonAsync<Caminhao>($"{route}create", null);
            response2.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetRequestToGetAllUsersShouldReturnHttpStatusCodeOK()
        {
            Caminhao user = new()
            {
                Marca = "Iveco",
                Modelo = Modelo.FM,
                AnoModelo = 2020
            };

            await _testClient.PostAsJsonAsync($"{route}create", user);

            HttpResponseMessage response = await _testClient.GetAsync($"{route}getall");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetRequestToGetAllUsersShouldReturnListOfUsers()
        {
            Caminhao user = new()
            {
                Marca = "Mercedes-Benz",
                Modelo = Modelo.FH,
                AnoModelo = 2019
            };

            await _testClient.PostAsJsonAsync($"{route}create", user);

            HttpResponseMessage response = await _testClient.GetAsync($"{route}getall");
            IList<Caminhao> users = await response.Content.ReadAsAsync<List<Caminhao>>();
            users.Should().NotBeEmpty();
        }

        [Fact]
        public async Task TestGetById()
        {
            Caminhao user = new()
            {
                Marca = "MAN",
                Modelo = Modelo.FH,
                AnoModelo = 2019
            };

            HttpResponseMessage response = await _testClient.PostAsJsonAsync($"{route}create", user);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            Caminhao user1 = await response.Content.ReadAsAsync<Caminhao>();

            HttpResponseMessage response1 = await _testClient.GetAsync($"{route}getbyid?id=" + user1.Id);
            response1.StatusCode.Should().Be(HttpStatusCode.OK);
            Caminhao u = await response1.Content.ReadAsAsync<Caminhao>();
            u.Equals(user1).Should().BeTrue();

            HttpResponseMessage response2 = await _testClient.DeleteAsync($"{route}delete?id=" + user1.Id);
            response2.StatusCode.Should().Be(HttpStatusCode.OK);

            HttpResponseMessage response3 = await _testClient.GetAsync($"{route}getbyid?id=" + user1.Id);
            response3.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task TestDelete()
        {
            Caminhao user = new()
            {
                Marca = "Scania",
                Modelo = Modelo.FH,
                AnoModelo = 2019
            };

            HttpResponseMessage response = await _testClient.PostAsJsonAsync($"{route}create", user);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            Caminhao user1 = await response.Content.ReadAsAsync<Caminhao>();

            HttpResponseMessage response1 = await _testClient.DeleteAsync($"{route}delete?id=" + user1.Id);
            response1.StatusCode.Should().Be(HttpStatusCode.OK);

            HttpResponseMessage response2 = await _testClient.GetAsync($"{route}getbyid?id=" + user1.Id);
            response2.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        //Not implementated yet
        [Fact]
        public async Task TestUpdate()
        {
            Caminhao caminhao = new()
            {
                Marca = "DAF",
                Modelo = Modelo.FH,
                AnoModelo = 2019
            };

            var response = await _testClient.PostAsJsonAsync($"{route}create", caminhao);
            //response.StatusCode.Should().Be(HttpStatusCode.OK);
            Caminhao caminhaoSvd = await response.Content.ReadAsAsync<Caminhao>();

            var response1 = await _testClient.GetAsync($"{route}getbyid?id=" + caminhaoSvd.Id);
            //response1.StatusCode.Should().Be(HttpStatusCode.OK);
            //Caminhao u = await response1.Content.ReadAsAsync<Caminhao>();
            //u.Equals(caminhao1).Should().BeTrue();

            Caminhao caminhaoUpdt = new()
            {
                Id = caminhaoSvd.Id,
                Marca = "DAF Caminhões",
                Modelo = Modelo.FM,
                AnoModelo = 2020
            };

            var response2 = await _testClient.PutAsJsonAsync($"{route}update", caminhaoUpdt);

            response2.StatusCode.Should().Be(HttpStatusCode.OK);

            Caminhao caminhaoUpdtd = await response2.Content.ReadAsAsync<Caminhao>();

            caminhaoUpdtd.Equals(caminhaoUpdt).Should().BeTrue();
            caminhaoUpdtd.Equals(caminhaoSvd).Should().BeFalse();
        }
    }
}
