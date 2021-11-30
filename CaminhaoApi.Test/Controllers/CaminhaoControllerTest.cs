using CaminhaoApi.Domain.CaminhaoAggregate;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace CaminhaoApi.Test.Controllers
{
    public class CaminhaoControllerTest : StartupTest
    {

        [Fact]
        public async Task GetParaObterTodosOsCaminhoesDeveRetornarHttpStatusCodeOK()
        {
            // Arrange
            Caminhao caminhao = new()
            {
                Marca = "Iveco",
                Modelo = Modelo.FM,
                AnoFabricacao = DateTime.Now.Year,
                AnoModelo = DateTime.Now.Year
            };

            // Act
            await _httpClientTest.PostAsJsonAsync($"{_routeCaminhao}cadastrarcaminhao", caminhao);
            HttpResponseMessage response = await _httpClientTest.GetAsync($"{_routeCaminhao}obtertodososcaminhoes");

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.OK, "Get para obter todos os caminhoes deve retornar HttpStatusCode.OK");
        }

        [Fact]
        public async Task GetParaObterCaminhaoPorIdDeveRetornarHttpStatusCodeOK()
        {
            // Arrange
            Caminhao caminhao = new()
            {
                Marca = "Mercedes-Benz",
                Modelo = Modelo.FH,
                AnoFabricacao = DateTime.Now.Year,
                AnoModelo = DateTime.Now.Year
            };

            // Act
            HttpResponseMessage response = await _httpClientTest.PostAsJsonAsync($"{_routeCaminhao}cadastrarcaminhao", caminhao);
            Caminhao caminhao1 = await response.Content.ReadAsAsync<Caminhao>();

            HttpResponseMessage response2 = await _httpClientTest.GetAsync($"{_routeCaminhao}obtercaminhaoporid?id={caminhao1.Id}");

            // Assert
            Assert.True(response2.StatusCode == HttpStatusCode.OK, "Get para obter Caminhão por Id deve retornar HttpStatusCode.OK");
        }

        [Fact]
        public async Task GetParaObterCaminhaoPorIdInexistenteDeveRetornarHttpStatusCodeNoContent()
        {
            // Arrange
            var id = "IdInexistente";

            // Act
            HttpResponseMessage response = await _httpClientTest.GetAsync($"{_routeCaminhao}obtercaminhaoporid?id={id}");

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.NoContent, "Get para obter Caminhão por Id inexistente " +
                                                                         "deve retornar HttpStatusCode.NoContent");
        }

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
            HttpResponseMessage response = await _httpClientTest.PostAsJsonAsync($"{_routeCaminhao}cadastrarcaminhao", caminhao);

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.OK, "Post para cadastrar Caminhão deve retornar HttpStatusCode.OK");
        }

        [Fact]
        public async Task PostParaCadastrarCaminhaoComIdJaExistenteDeveRetornarHttpStatusCodeInternalServerError()
        {
            // Arrange
            Caminhao caminhao = new()
            {
                Marca = "Hyundai",
                Modelo = Modelo.FH,
                AnoFabricacao = DateTime.Now.Year,
                AnoModelo = DateTime.Now.Year
            };

            // Act
            HttpResponseMessage response = await _httpClientTest.PostAsJsonAsync($"{_routeCaminhao}cadastrarcaminhao", caminhao);
            Caminhao caminhao1 = await response.Content.ReadAsAsync<Caminhao>();

            Caminhao caminhao2 = new()
            {
                Id = caminhao1.Id,
                Marca = "Foton",
                Modelo = Modelo.FH,
                AnoFabricacao = DateTime.Now.Year,
                AnoModelo = DateTime.Now.Year
            };

            HttpResponseMessage response2 = await _httpClientTest.PostAsJsonAsync($"{_routeCaminhao}cadastrarcaminhao", caminhao2);

            // Assert
            Assert.True(response2.StatusCode == HttpStatusCode.InternalServerError, "Post para cadastrar Caminhão com Id já existente " +
                                                                                    "deve retornar HttpStatusCode.BadRequest");
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
            HttpResponseMessage response = await _httpClientTest.PostAsJsonAsync($"{_routeCaminhao}cadastrarcaminhao", caminhao);
            Caminhao caminhao1 = await response.Content.ReadAsAsync<Caminhao>();

            // Assert
            Assert.False(string.IsNullOrEmpty(caminhao1.Id), "Post para cadastrar Caminhão deve retornar caminhao com um Id");
            Assert.True(caminhao1.Marca == caminhao.Marca, "Post para cadastrar Caminhão deve retornar caminhao com a Marca cadastrada");
            Assert.True(caminhao1.Modelo == caminhao.Modelo, "Post para cadastrar Caminhão deve retornar caminhao com o Modelo cadastrado");
            Assert.True(caminhao1.AnoFabricacao == caminhao.AnoFabricacao, "Post para cadastrar Caminhão deve retornar caminhao com Ano de Fabricação cadastrado");
            Assert.True(caminhao1.AnoModelo == caminhao.AnoModelo, "Post para cadastrar Caminhão deve retornar caminhao com o Ano de Modelo cadastrado");
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
            HttpResponseMessage response = await _httpClientTest.PostAsJsonAsync($"{_routeCaminhao}cadastrarcaminhao", caminhao);

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest, "Post para cadastrar Caminhão com Modelo diferente de FH e FM " +
                                                                          "deve retornar HttpStatusCode.BadRequest");
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
            HttpResponseMessage response = await _httpClientTest.PostAsJsonAsync($"{_routeCaminhao}cadastrarcaminhao", caminhao);

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest, "Post para cadastrar Caminhão com Ano de Fabricação menor que o atual " +
                                                                          "deve retornar HttpStatusCode.BadRequest");
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
            HttpResponseMessage response = await _httpClientTest.PostAsJsonAsync($"{_routeCaminhao}cadastrarcaminhao", caminhao);

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest, "Post para cadastrar Caminhão com Ano de Fabricação maoir que o atual " +
                                                                          "deve retornar HttpStatusCode.BadRequest");
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
            HttpResponseMessage response = await _httpClientTest.PostAsJsonAsync($"{_routeCaminhao}cadastrarcaminhao", caminhao);

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.OK, "Post para cadastrar Caminhão com Ano do Modelo subsequente ao atual " +
                                                                  "deve retornar HttpStatusCode.OK");
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
            HttpResponseMessage response = await _httpClientTest.PostAsJsonAsync($"{_routeCaminhao}cadastrarcaminhao", caminhao);

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest, "Post para cadastrar Caminhão com Ano do Modelo maior que o subsequente ao atual " +
                                                                          "deve retornar HttpStatusCode.BadRequest");
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
            HttpResponseMessage response = await _httpClientTest.PostAsJsonAsync($"{_routeCaminhao}cadastrarcaminhao", caminhao);

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest, "Post para cadastrar Caminhão com Ano do Modelo menor que o atual " +
                                                                          "deve retornar HttpStatusCode.BadRequest");
        }


        [Fact]
        public async Task PostParaCadastrarCaminhaoComObjetoNuloDeveRetornarHttpStatusCodeBadRequest()
        {
            // Arrange
            Caminhao caminhao = null;

            // Act
            HttpResponseMessage response = await _httpClientTest.PostAsJsonAsync<Caminhao>($"{_routeCaminhao}cadastrarcaminhao", caminhao);

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest, "Post para cadastrar Caminhão com Objeto nulo " +
                                                                          "deve retornar HttpStatusCode.BadRequest");
        }

        [Fact]
        public async Task PutParaAtualizarCaminhaoDeveRetornarHttpStatusCodeOK()
        {
            // Arrange
            Caminhao caminhao = new()
            {
                Marca = "Volkswagen",
                Modelo = Modelo.FM,
                AnoFabricacao = DateTime.Now.Year,
                AnoModelo = DateTime.Now.Year
            };

            // Act
            var response = await _httpClientTest.PostAsJsonAsync($"{_routeCaminhao}cadastrarcaminhao", caminhao);
            Caminhao caminhao1 = await response.Content.ReadAsAsync<Caminhao>();

            Caminhao caminhao2 = new()
            {
                Id = caminhao1.Id,
                Marca = "Volvo",
                Modelo = Modelo.FH,
                AnoFabricacao = DateTime.Now.Year,
                AnoModelo = DateTime.Now.Year + 1
            };

            var response1 = await _httpClientTest.PutAsJsonAsync($"{_routeCaminhao}atualizarcaminhao", caminhao2);

            // Assert
            Assert.True(response1.StatusCode == HttpStatusCode.OK, "Put para atualizar Caminhão deve retornar HttpStatusCode.OK");
        }

        [Fact]
        public async Task PutParaAtualizarCaminhaoDeveRetornarCaminhaoComMesmoIdEAtributosAtualizados()
        {
            // Arrange
            Caminhao caminhao = new()
            {
                Marca = "Volkswagen",
                Modelo = Modelo.FM,
                AnoFabricacao = DateTime.Now.Year,
                AnoModelo = DateTime.Now.Year
            };

            // Act
            var response = await _httpClientTest.PostAsJsonAsync($"{_routeCaminhao}cadastrarcaminhao", caminhao);
            Caminhao caminhao1 = await response.Content.ReadAsAsync<Caminhao>();

            Caminhao caminhao2 = new()
            {
                Id = caminhao1.Id,
                Marca = "Volvo",
                Modelo = Modelo.FH,
                AnoFabricacao = DateTime.Now.Year,
                AnoModelo = DateTime.Now.Year + 1
            };

            var response1 = await _httpClientTest.PutAsJsonAsync($"{_routeCaminhao}atualizarcaminhao", caminhao2);
            Caminhao caminhao3 = await response1.Content.ReadAsAsync<Caminhao>();

            // Assert
            Assert.True(caminhao3.Id == caminhao1.Id, "Put para atualizar Caminhão deve retornar Caminhão com mesmo Id");
            Assert.True(caminhao3.Equals(caminhao2), "Put para atualizar Caminhão deve retornar Caminhão igual o atualizado (com todos os atributos iguais)");
        }

        [Fact]
        public async Task PutParaAtualizarCaminhaoComModeloDiferenteDeFHeFMDeveRetornarHttpStatusCodeBadRequest()
        {
            // Arrange
            Caminhao caminhao = new()
            {
                Marca = "Volkswagen",
                Modelo = Modelo.FM,
                AnoFabricacao = DateTime.Now.Year,
                AnoModelo = DateTime.Now.Year
            };

            // Act
            var response = await _httpClientTest.PostAsJsonAsync($"{_routeCaminhao}cadastrarcaminhao", caminhao);
            Caminhao caminhao1 = await response.Content.ReadAsAsync<Caminhao>();

            Caminhao caminhao2 = new()
            {
                Id = caminhao1.Id,
                Marca = "Volvo",
                AnoFabricacao = DateTime.Now.Year,
                AnoModelo = DateTime.Now.Year + 1
            };

            var response1 = await _httpClientTest.PutAsJsonAsync($"{_routeCaminhao}atualizarcaminhao", caminhao2);

            // Assert
            Assert.True(response1.StatusCode == HttpStatusCode.BadRequest, "Put para atualizar Caminhão com Modelo diferente de FH e FM " +
                                                                           "deve retornar HttpStatusCode.BadRequest");
        }

        [Fact]
        public async Task PutParaAtualizarCaminhaoComAnoDeFabricacaoMenorQueOAtualDeveRetornarHttpStatusCodeBadRequest()
        {
            // Arrange
            Caminhao caminhao = new()
            {
                Marca = "Volkswagen",
                Modelo = Modelo.FM,
                AnoFabricacao = DateTime.Now.Year,
                AnoModelo = DateTime.Now.Year
            };

            // Act
            var response = await _httpClientTest.PostAsJsonAsync($"{_routeCaminhao}cadastrarcaminhao", caminhao);
            Caminhao caminhao1 = await response.Content.ReadAsAsync<Caminhao>();

            Caminhao caminhao2 = new()
            {
                Id = caminhao1.Id,
                Marca = "Volvo",
                Modelo = Modelo.FH,
                AnoFabricacao = DateTime.Now.Year - 1,
                AnoModelo = DateTime.Now.Year
            };

            var response1 = await _httpClientTest.PutAsJsonAsync($"{_routeCaminhao}atualizarcaminhao", caminhao2);

            // Assert
            Assert.True(response1.StatusCode == HttpStatusCode.BadRequest, "Put para atualizar Caminhão com Ano de Fabricação menor que o atual " +
                                                                           "deve retornar HttpStatusCode.BadRequest");
        }

        [Fact]
        public async Task PutParaAtualizarCaminhaoComAnoDeFabricacaoMaoirQueOAtualDeveRetornarHttpStatusCodeBadRequest()
        {
            // Arrange
            Caminhao caminhao = new()
            {
                Marca = "Volkswagen",
                Modelo = Modelo.FM,
                AnoFabricacao = DateTime.Now.Year,
                AnoModelo = DateTime.Now.Year
            };

            // Act
            var response = await _httpClientTest.PostAsJsonAsync($"{_routeCaminhao}cadastrarcaminhao", caminhao);
            Caminhao caminhao1 = await response.Content.ReadAsAsync<Caminhao>();

            Caminhao caminhao2 = new()
            {
                Id = caminhao1.Id,
                Marca = "Volvo",
                Modelo = Modelo.FH,
                AnoFabricacao = DateTime.Now.Year + 1,
                AnoModelo = DateTime.Now.Year
            };

            var response1 = await _httpClientTest.PutAsJsonAsync($"{_routeCaminhao}atualizarcaminhao", caminhao2);

            // Assert
            Assert.True(response1.StatusCode == HttpStatusCode.BadRequest, "Put para atualizar Caminhão com Ano de Fabricação maoir que o atual " +
                                                                           "deve retornar HttpStatusCode.BadRequest");
        }

        [Fact]
        public async Task PutParaAtualizarCaminhaoComAnoDoModeloSubsequenteAoAtualDeveRetornarHttpStatusCodeOK()
        {
            // Arrange
            Caminhao caminhao = new()
            {
                Marca = "Volkswagen",
                Modelo = Modelo.FM,
                AnoFabricacao = DateTime.Now.Year,
                AnoModelo = DateTime.Now.Year
            };

            // Act
            var response = await _httpClientTest.PostAsJsonAsync($"{_routeCaminhao}cadastrarcaminhao", caminhao);
            Caminhao caminhao1 = await response.Content.ReadAsAsync<Caminhao>();

            Caminhao caminhao2 = new()
            {
                Id = caminhao1.Id,
                Marca = "Volvo",
                Modelo = Modelo.FH,
                AnoFabricacao = DateTime.Now.Year,
                AnoModelo = DateTime.Now.Year + 1
            };

            var response1 = await _httpClientTest.PutAsJsonAsync($"{_routeCaminhao}atualizarcaminhao", caminhao2);

            // Assert
            Assert.True(response1.StatusCode == HttpStatusCode.OK, "Put para atualizar Caminhão com Ano do Modelo subsequente ao atual " +
                                                                   "deve retornar HttpStatusCode.OK");
        }

        [Fact]
        public async Task PutParaAtualizarCaminhaoComAnoDoModeloMaiorQueOSubsequenteAoAtualDeveRetornarHttpStatusCodeBadRequest()
        {
            // Arrange
            Caminhao caminhao = new()
            {
                Marca = "Volkswagen",
                Modelo = Modelo.FM,
                AnoFabricacao = DateTime.Now.Year,
                AnoModelo = DateTime.Now.Year
            };

            // Act
            var response = await _httpClientTest.PostAsJsonAsync($"{_routeCaminhao}cadastrarcaminhao", caminhao);
            Caminhao caminhao1 = await response.Content.ReadAsAsync<Caminhao>();

            Caminhao caminhao2 = new()
            {
                Id = caminhao1.Id,
                Marca = "Volvo",
                Modelo = Modelo.FH,
                AnoFabricacao = DateTime.Now.Year,
                AnoModelo = DateTime.Now.Year + 2
            };

            var response1 = await _httpClientTest.PutAsJsonAsync($"{_routeCaminhao}atualizarcaminhao", caminhao2);

            // Assert
            Assert.True(response1.StatusCode == HttpStatusCode.BadRequest, "Put para atualizar Caminhão com Ano do Modelo maior que o subsequente ao atual " +
                                                                           "deve retornar HttpStatusCode.BadRequest");
        }

        [Fact]
        public async Task PutParaAtualizarCaminhaoComAnoDoModeloMenorQueOAtualDeveRetornarHttpStatusCodeBadRequest()
        {
            // Arrange
            Caminhao caminhao = new()
            {
                Marca = "Volkswagen",
                Modelo = Modelo.FM,
                AnoFabricacao = DateTime.Now.Year,
                AnoModelo = DateTime.Now.Year
            };

            // Act
            var response = await _httpClientTest.PostAsJsonAsync($"{_routeCaminhao}cadastrarcaminhao", caminhao);
            Caminhao caminhao1 = await response.Content.ReadAsAsync<Caminhao>();

            Caminhao caminhao2 = new()
            {
                Id = caminhao1.Id,
                Marca = "Volvo",
                Modelo = Modelo.FH,
                AnoFabricacao = DateTime.Now.Year,
                AnoModelo = DateTime.Now.Year - 1
            };

            var response1 = await _httpClientTest.PutAsJsonAsync($"{_routeCaminhao}atualizarcaminhao", caminhao2);

            // Assert
            Assert.True(response1.StatusCode == HttpStatusCode.BadRequest, "Put para atualizar Caminhão com Ano do Modelo menor que o atual " +
                                                                           "deve retornar HttpStatusCode.BadRequest");
        }

        [Fact]
        public async Task PutParaAtualizarCaminhaoComObjetoNuloDeveRetornarHttpStatusCodeBadRequest()
        {
            // Arrange
            Caminhao caminhao = null;

            // Act
            var response = await _httpClientTest.PutAsJsonAsync($"{_routeCaminhao}atualizarcaminhao", caminhao);

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest, "Put para atualizar Caminhão com Objeto nulo " +
                                                                          "deve retornar HttpStatusCode.BadRequest");
        }

        [Fact]
        public async Task PutParaAtualizarCaminhaoComIdInexistenteDeveRetornarHttpStatusCodeInternalServerError()
        {
            // Arrange
            Caminhao caminhao = new()
            {
                Id = "AtualizarComIdInexistente",
                Marca = "Volkswagen",
                Modelo = Modelo.FM,
                AnoFabricacao = DateTime.Now.Year,
                AnoModelo = DateTime.Now.Year
            };

            // Act
            var response = await _httpClientTest.PutAsJsonAsync($"{_routeCaminhao}atualizarcaminhao", caminhao);

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.InternalServerError, "Put para atualizar Caminhão com Id inexistente " +
                                                                                   "deve retornar HttpStatusCode.InternalServerError");
        }

        [Fact]
        public async Task DeleteParaRemoverCaminhaoPorIdDeveRetornarHttpStatusCodeOK()
        {
            // Arrange
            Caminhao caminhao = new()
            {
                Marca = "Scania",
                Modelo = Modelo.FM,
                AnoFabricacao = DateTime.Now.Year,
                AnoModelo = DateTime.Now.Year
            };

            // Act
            HttpResponseMessage response = await _httpClientTest.PostAsJsonAsync($"{_routeCaminhao}cadastrarcaminhao", caminhao);
            Caminhao caminhao1 = await response.Content.ReadAsAsync<Caminhao>();

            HttpResponseMessage response1 = await _httpClientTest.DeleteAsync($"{_routeCaminhao}removercaminhao?id={caminhao1.Id}");

            // Assert
            Assert.True(response1.StatusCode == HttpStatusCode.OK, "Delete para remover Caminhão deve retornar HttpStatusCode.OK");
        }

        [Fact]
        public async Task DeleteParaRemoverCaminhaoComIdInexistenteDeveRetornarHttpStatusCodeInternalServerError()
        {
            // Arrange
            var id = "IdInexistente2";

            // Act
            HttpResponseMessage response = await _httpClientTest.DeleteAsync($"{_routeCaminhao}removercaminhao?id={id}");

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.InternalServerError, "Delete para remover Caminhão com Id inexistente " +
                                                                                   "deve retornar HttpStatusCode.InternalServerError");
        }
    }
}
