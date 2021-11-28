using CaminhaoApi.Domain.CaminhaoAggregate;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace CaminhaoApi.Test.Controllers
{
    public class CaminhaoControllerTest : TestStartup
    {
        private readonly string route = "Caminhao/";

        [Fact]
        public async Task PostParaCadastrarCaminhaoDeveRetornarHttpStatusCodeOK()
        {
            // Arrange
            Caminhao caminhao = new()
            {
                Marca = "DAF",
                Modelo = Modelo.FH,
                AnoFabricacao = DateTime.Now.Year,
                AnoModelo = DateTime.Now.Year
            };

            // Act
            HttpResponseMessage response = await _testClient.PostAsJsonAsync($"{route}cadastrarcaminhao", caminhao);

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.OK, $"should not be prime");
        }

        [Fact]
        public async Task PostParaCadastrarCaminhaoDeveRetornarCaminhaoComUmIdEOsMesmosAtributosCadastrados()
        {
            // Arrange
            Caminhao caminhao = new()
            {
                Marca = "DAF",
                Modelo = Modelo.FH,
                AnoFabricacao = DateTime.Now.Year,
                AnoModelo = DateTime.Now.Year
            };

            // Act
            HttpResponseMessage response = await _testClient.PostAsJsonAsync($"{route}cadastrarcaminhao", caminhao);
            Caminhao caminhao1 = await response.Content.ReadAsAsync<Caminhao>();

            // Assert
            Assert.False(string.IsNullOrEmpty(caminhao1.Id));
            Assert.True(caminhao1.Marca == caminhao.Marca);
            Assert.True(caminhao1.Modelo == caminhao.Modelo);
            Assert.True(caminhao1.AnoFabricacao == caminhao.AnoFabricacao);
            Assert.True(caminhao1.AnoModelo == caminhao.AnoModelo);
        }

        [Fact]
        public async Task PostParaCadastrarCaminhaoComModeloDiferenteDeFHeFMDeveRetornarHttpStatusCodeBadRequest()
        {
            // Arrange
            Caminhao caminhao = new()
            {
                Marca = "DAF",
                AnoFabricacao = DateTime.Now.Year,
                AnoModelo = DateTime.Now.Year
            };

            // Act
            HttpResponseMessage response = await _testClient.PostAsJsonAsync($"{route}cadastrarcaminhao", caminhao);

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task PostParaCadastrarCaminhaoComAnoDeFabricacaoMenorQueOAtualDeveRetornarHttpStatusCodeBadRequest()
        {
            // Arrange
            Caminhao caminhao = new()
            {
                Marca = "DAF",
                Modelo = Modelo.FH,
                AnoFabricacao = DateTime.Now.Year - 1,
                AnoModelo = DateTime.Now.Year
            };

            // Act
            HttpResponseMessage response = await _testClient.PostAsJsonAsync($"{route}cadastrarcaminhao", caminhao);

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task PostParaCadastrarCaminhaoComAnoDeFabricacaoMaoirQueOAtualDeveRetornarHttpStatusCodeBadRequest()
        {
            // Arrange
            Caminhao caminhao = new()
            {
                Marca = "DAF",
                Modelo = Modelo.FH,
                AnoFabricacao = DateTime.Now.Year + 1,
                AnoModelo = DateTime.Now.Year
            };

            // Act
            HttpResponseMessage response = await _testClient.PostAsJsonAsync($"{route}cadastrarcaminhao", caminhao);

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task PostParaCadastrarCaminhaoComAnoDoModeloSubsequenteAoAtualDeveRetornarHttpStatusCodeOK()
        {
            // Arrange
            Caminhao caminhao = new()
            {
                Marca = "DAF",
                Modelo = Modelo.FH,
                AnoFabricacao = DateTime.Now.Year,
                AnoModelo = DateTime.Now.Year + 1
            };

            // Act
            HttpResponseMessage response = await _testClient.PostAsJsonAsync($"{route}cadastrarcaminhao", caminhao);

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.OK);
        }

        [Fact]
        public async Task PostParaCadastrarCaminhaoComAnoDoModeloMaiorQueOSubsequenteAoAtualDeveRetornarHttpStatusCodeBadRequest()
        {
            // Arrange
            Caminhao caminhao = new()
            {
                Marca = "DAF",
                Modelo = Modelo.FH,
                AnoFabricacao = DateTime.Now.Year,
                AnoModelo = DateTime.Now.Year + 2
            };

            // Act
            HttpResponseMessage response = await _testClient.PostAsJsonAsync($"{route}cadastrarcaminhao", caminhao);

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task PostParaCadastrarCaminhaoComAnoDoModeloMenorQueOAtualDeveRetornarHttpStatusCodeBadRequest()
        {
            // Arrange
            Caminhao caminhao = new()
            {
                Marca = "DAF",
                Modelo = Modelo.FH,
                AnoFabricacao = DateTime.Now.Year,
                AnoModelo = DateTime.Now.Year - 1
            };

            // Act
            HttpResponseMessage response = await _testClient.PostAsJsonAsync($"{route}cadastrarcaminhao", caminhao);

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
        }


        [Fact]
        public async Task PostParaCadastrarCaminhaoComObjetoNuloDeveRetornarHttpStatusCodeBadRequest()
        {
            // Arrange
            Caminhao caminhao = null;

            // Act
            HttpResponseMessage response = await _testClient.PostAsJsonAsync<Caminhao>($"{route}cadastrarcaminhao", caminhao);

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetRequestToGetTodosCaminhoesShouldReturnHttpStatusCodeOK()
        {
            // Arrange
            Caminhao caminhao = new()
            {
                Marca = "Iveco",
                Modelo = Modelo.FM,
                AnoFabricacao = DateTime.Now.Year,
                AnoModelo = DateTime.Now.Year
            };

            await _testClient.PostAsJsonAsync($"{route}create", caminhao);

            HttpResponseMessage response = await _testClient.GetAsync($"{route}getall");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetRequestToGetAllCaminhoesShouldReturnListOfCaminhoes()
        {
            Caminhao caminhao = new()
            {
                Marca = "Mercedes-Benz",
                Modelo = Modelo.FH,
                AnoModelo = 2019
            };

            await _testClient.PostAsJsonAsync($"{route}create", caminhao);

            HttpResponseMessage response = await _testClient.GetAsync($"{route}getall");
            IList<Caminhao> caminhaos = await response.Content.ReadAsAsync<List<Caminhao>>();
            caminhaos.Should().NotBeEmpty();
        }

        [Fact]
        public async Task TestGetById()
        {
            Caminhao caminhao = new()
            {
                Marca = "MAN",
                Modelo = Modelo.FH,
                AnoModelo = 2019
            };

            HttpResponseMessage response = await _testClient.PostAsJsonAsync($"{route}create", caminhao);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            Caminhao caminhao1 = await response.Content.ReadAsAsync<Caminhao>();

            HttpResponseMessage response1 = await _testClient.GetAsync($"{route}getbyid?id=" + caminhao1.Id);
            response1.StatusCode.Should().Be(HttpStatusCode.OK);
            Caminhao u = await response1.Content.ReadAsAsync<Caminhao>();
            u.Equals(caminhao1).Should().BeTrue();

            HttpResponseMessage response2 = await _testClient.DeleteAsync($"{route}delete?id=" + caminhao1.Id);
            response2.StatusCode.Should().Be(HttpStatusCode.OK);

            HttpResponseMessage response3 = await _testClient.GetAsync($"{route}getbyid?id=" + caminhao1.Id);
            response3.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task TestDelete()
        {
            Caminhao caminhao = new()
            {
                Marca = "Scania",
                Modelo = Modelo.FH,
                AnoModelo = 2019
            };

            HttpResponseMessage response = await _testClient.PostAsJsonAsync($"{route}create", caminhao);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            Caminhao caminhao1 = await response.Content.ReadAsAsync<Caminhao>();

            HttpResponseMessage response1 = await _testClient.DeleteAsync($"{route}delete?id=" + caminhao1.Id);
            response1.StatusCode.Should().Be(HttpStatusCode.OK);

            HttpResponseMessage response2 = await _testClient.GetAsync($"{route}getbyid?id=" + caminhao1.Id);
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
