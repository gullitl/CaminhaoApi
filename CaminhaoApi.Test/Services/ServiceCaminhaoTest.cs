using CaminhaoApi.Domain.CaminhaoAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Xunit;

namespace CaminhaoApi.Test.Controllers
{
    public class ServiceCaminhaoTest : StartupTest
    {
        [Fact]
        public async Task ObterTodosOsCaminhoesDeveRetornarUmaListaDeCaminhoesNaoVaziaQuandoTemRegistroNoBancoDeDados()
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
            await _serviceCaminhao.CadastrarCaminhao(caminhao);
            List<Caminhao> caminhoes = await _serviceCaminhao.ObterTodosOsCaminhoes();

            // Assert
            Assert.True(caminhoes?.Any(), "Obter todos os Caminhoes deve retornar uma lista de Caminhoes não vazia quando tem registro no Banco de Dados");
        }

        [Fact]
        public async Task ObterCaminhaoPorIdDeveRetornarUmCaminhaoNaoNullQuandoTemRegistroDoCaminhaoNoBancoDeDados()
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
            Caminhao caminhao1 = await _serviceCaminhao.CadastrarCaminhao(caminhao);
            Caminhao caminhao2 = await _serviceCaminhao.ObterCaminhaoPorId(caminhao1.Id);

            // Assert
            Assert.True(caminhao2 != null, "Obter Caminhão por Id deve retornar um Caminhão não nulo quando tem registro do Caminhão no Banco de Dados");
        }

        [Fact]
        public async Task ObterCaminhaoPorIdInexistenteDeveRetornarCaminhaoNulo()
        {
            // Arrange
            var id = "IdInexistenteCaminhaoServico";

            // Act
            Caminhao caminhao = await _serviceCaminhao.ObterCaminhaoPorId(id);

            // Assert
            Assert.True(caminhao == null, "Obter Caminhão por Id inexistente deve retornar Caminhão nulo");
        }

        [Fact]
        public async Task CadastrarCaminhaoComIdJaExistenteDeveRetornarInvalidOperationException()
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
            Caminhao caminhao1 = await _serviceCaminhao.CadastrarCaminhao(caminhao);

            Caminhao caminhao2 = new()
            {
                Id = caminhao1.Id,
                Marca = "Foton",
                Modelo = Modelo.FH,
                AnoFabricacao = DateTime.Now.Year,
                AnoModelo = DateTime.Now.Year
            };

            var mensagem = $"An item with the same key has already been added. Key: {caminhao2.Id}";

            // Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await _serviceCaminhao.CadastrarCaminhao(caminhao2));
            Assert.True(exception.Message == mensagem, $"Cadastrar Caminhão com Id já existente deve retornar InvalidOperationException com a mensagem: {mensagem}");
        }

        [Fact]
        public async Task CadastrarCaminhaoDeveRetornarCaminhaoComUmIdEOsMesmosAtributosCadastrados()
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
            Caminhao caminhao1 = await _serviceCaminhao.CadastrarCaminhao(caminhao);

            // Assert
            Assert.False(string.IsNullOrEmpty(caminhao1.Id), "Cadastrar Caminhão deve retornar caminhao com um Id");
            Assert.True(caminhao1.Marca == caminhao.Marca, "Cadastrar Caminhão deve retornar caminhao com a Marca cadastrada");
            Assert.True(caminhao1.Modelo == caminhao.Modelo, "Cadastrar Caminhão deve retornar caminhao com o Modelo cadastrado");
            Assert.True(caminhao1.AnoFabricacao == caminhao.AnoFabricacao, "Cadastrar Caminhão deve retornar caminhao com Ano de Fabricação cadastrado");
            Assert.True(caminhao1.AnoModelo == caminhao.AnoModelo, "Cadastrar Caminhão deve retornar caminhao com o Ano de Modelo cadastrado");
        }

        [Fact]
        public async Task CadastrarCaminhaoComModeloDiferenteDeFHeFMDeveRetornarValidationException()
        {
            // Arrange
            Caminhao caminhao = new()
            {
                Marca = "DAF",
                Modelo = Modelo.BU,
                AnoFabricacao = DateTime.Now.Year,
                AnoModelo = DateTime.Now.Year
            };

            var mensagem = "Modelo pode aceitar apenas FH e FM";

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(async () => await _serviceCaminhao.CadastrarCaminhao(caminhao));
            Assert.True(exception.Message == mensagem, $"Cadastrar Caminhão com Modelo diferente de FH e FM deve retornar ValidationException com a mensagem: {mensagem}");
        }

        [Fact]
        public async Task CadastrarCaminhaoComAnoDeFabricacaoMenorQueOAtualDeveRetornarValidationException()
        {
            // Arrange
            Caminhao caminhao = new()
            {
                Marca = "DAF",
                Modelo = Modelo.FH,
                AnoFabricacao = DateTime.Now.Year - 1,
                AnoModelo = DateTime.Now.Year
            };

            var mensagem = "Ano de Fabricação deve ser o atual";

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(async () => await _serviceCaminhao.CadastrarCaminhao(caminhao));
            Assert.True(exception.Message == mensagem, "Cadastrar Caminhão com Ano de Fabricação menor que o atual deve retornar ValidationException " +
                                                       $"com a mensagem: {mensagem}");
        }

        [Fact]
        public async Task CadastrarCaminhaoComAnoDeFabricacaoMaoirQueOAtualDeveRetornarValidationException()
        {
            // Arrange
            Caminhao caminhao = new()
            {
                Marca = "DAF",
                Modelo = Modelo.FH,
                AnoFabricacao = DateTime.Now.Year + 1,
                AnoModelo = DateTime.Now.Year
            };

            var mensagem = "Ano de Fabricação deve ser o atual";

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(async () => await _serviceCaminhao.CadastrarCaminhao(caminhao));
            Assert.True(exception.Message == mensagem, "Cadastrar Caminhão com Ano de Fabricação maoir que o atual deve retornar ValidationException " +
                                                       $"com a mensagem: {mensagem}");
        }

        [Fact]
        public async Task PostParaCadastrarCaminhaoComAnoDoModeloSubsequenteAoAtualDeveRetornarCaminhaoComUmIdEOsMesmosAtributosCadastrados()
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
            Caminhao caminhao1 = await _serviceCaminhao.CadastrarCaminhao(caminhao);

            // Assert
            Assert.False(string.IsNullOrEmpty(caminhao1.Id), "Cadastrar Caminhão deve retornar caminhao com um Id");
            Assert.True(caminhao1.Marca == caminhao.Marca, "Cadastrar Caminhão deve retornar caminhao com a Marca cadastrada");
            Assert.True(caminhao1.Modelo == caminhao.Modelo, "Cadastrar Caminhão deve retornar caminhao com o Modelo cadastrado");
            Assert.True(caminhao1.AnoFabricacao == caminhao.AnoFabricacao, "Cadastrar Caminhão deve retornar caminhao com Ano de Fabricação cadastrado");
            Assert.True(caminhao1.AnoModelo == caminhao.AnoModelo, "Cadastrar Caminhão deve retornar caminhao com o Ano de Modelo cadastrado");
        }

        [Fact]
        public async Task CadastrarCaminhaoComAnoDoModeloMaiorQueOSubsequenteAoAtualDeveRetornarValidationException()
        {
            // Arrange
            Caminhao caminhao = new()
            {
                Marca = "DAF",
                Modelo = Modelo.FH,
                AnoFabricacao = DateTime.Now.Year,
                AnoModelo = DateTime.Now.Year + 2
            };

            var mensagem = "Ano do Modelo pode ser o atual ou o ano subsequente";

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(async () => await _serviceCaminhao.CadastrarCaminhao(caminhao));
            Assert.True(exception.Message == mensagem, "Cadastrar Caminhão com Ano do Modelo maior que o subsequente ao atual deve retornar ValidationException " +
                                                       $"com a mensagem: {mensagem}");
        }

        [Fact]
        public async Task CadastrarCaminhaoComAnoDoModeloMenorQueOAtualDeveRetornarValidationException()
        {
            // Arrange
            Caminhao caminhao = new()
            {
                Marca = "DAF",
                Modelo = Modelo.FH,
                AnoFabricacao = DateTime.Now.Year,
                AnoModelo = DateTime.Now.Year - 1
            };

            var mensagem = "Ano do Modelo pode ser o atual ou o ano subsequente";

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(async () => await _serviceCaminhao.CadastrarCaminhao(caminhao));
            Assert.True(exception.Message == mensagem, "Cadastrar Caminhão com Ano do Modelo menor que o atual deve retornar ValidationException " +
                                                       $"com a mensagem: {mensagem}");
        }

        [Fact]
        public async Task CadastrarCaminhaoComObjetoNuloDeveRetornarArgumentNullException()
        {
            // Arrange
            Caminhao caminhao = null;

            var mensagem = "Value cannot be null. (Parameter 'instance')";

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(async () => await _serviceCaminhao.CadastrarCaminhao(caminhao));
            Assert.True(exception.Message == mensagem, "Cadastrar Caminhão com Objeto nulo deve retornar ArgumentNullException " +
                                                       $"com a mensagem: {mensagem}");
        }

        [Fact]
        public async Task AtualizarCaminhaoDeveRetornarCaminhaoComMesmoIdEAtributosAtualizados()
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
            Caminhao caminhao1 = await _serviceCaminhao.CadastrarCaminhao(caminhao);

            Caminhao caminhao2 = new()
            {
                Id = caminhao1.Id,
                Marca = "Volvo",
                Modelo = Modelo.FH,
                AnoFabricacao = DateTime.Now.Year,
                AnoModelo = DateTime.Now.Year + 1
            };

            Caminhao caminhao3 = await _serviceCaminhao.AtualizarCaminhao(caminhao2);

            // Assert
            Assert.True(caminhao3.Id == caminhao1.Id, "Atualizar Caminhão deve retornar Caminhão com mesmo Id");
            Assert.True(caminhao3.Equals(caminhao2), "Atualizar Caminhão deve retornar Caminhão igual o atualizado (com todos os atributos iguais)");
        }

        [Fact]
        public async Task AtualizarCaminhaoComModeloDiferenteDeFHeFMDeveRetornarValidationException()
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
            Caminhao caminhao1 = await _serviceCaminhao.CadastrarCaminhao(caminhao);

            Caminhao caminhao2 = new()
            {
                Id = caminhao1.Id,
                Marca = "Volvo",
                Modelo = Modelo.CE,
                AnoFabricacao = DateTime.Now.Year,
                AnoModelo = DateTime.Now.Year + 1
            };

            var mensagem = "Modelo pode aceitar apenas FH e FM";

            // Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(async () => await _serviceCaminhao.AtualizarCaminhao(caminhao2));
            Assert.True(exception.Message == mensagem, $"Atualizar Caminhão com Modelo diferente de FH e FM deve retornar ValidationException com a mensagem: {mensagem}");
        }

        [Fact]
        public async Task AtualizarCaminhaoComAnoDeFabricacaoMenorQueOAtualDeveRetornarValidationException()
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
            Caminhao caminhao1 = await _serviceCaminhao.CadastrarCaminhao(caminhao);

            Caminhao caminhao2 = new()
            {
                Id = caminhao1.Id,
                Marca = "Volvo",
                Modelo = Modelo.FH,
                AnoFabricacao = DateTime.Now.Year - 1,
                AnoModelo = DateTime.Now.Year
            };

            var mensagem = "Ano de Fabricação deve ser o atual";

            // Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(async () => await _serviceCaminhao.AtualizarCaminhao(caminhao2));
            Assert.True(exception.Message == mensagem, "Atualizar Caminhão com Ano de Fabricação menor que o atual deve retornar ValidationException " +
                                                      $"com a mensagem: {mensagem}");
        }

        [Fact]
        public async Task AtualizarCaminhaoComAnoDeFabricacaoMaoirQueOAtualDeveRetornarValidationException()
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
            Caminhao caminhao1 = await _serviceCaminhao.CadastrarCaminhao(caminhao);

            Caminhao caminhao2 = new()
            {
                Id = caminhao1.Id,
                Marca = "Volvo",
                Modelo = Modelo.FH,
                AnoFabricacao = DateTime.Now.Year + 1,
                AnoModelo = DateTime.Now.Year
            };

            var mensagem = "Ano de Fabricação deve ser o atual";

            // Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(async () => await _serviceCaminhao.AtualizarCaminhao(caminhao2));
            Assert.True(exception.Message == mensagem, "Atualizar Caminhão com Ano de Fabricação maoir que o atual deve retornar ValidationException " +
                                                       $"com a mensagem: {mensagem}");
        }

        [Fact]
        public async Task AtualizarCaminhaoComAnoDoModeloSubsequenteAoAtualDeveRetornarCaminhaoComMesmoIdEAtributosAtualizados()
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
            Caminhao caminhao1 = await _serviceCaminhao.CadastrarCaminhao(caminhao);

            Caminhao caminhao2 = new()
            {
                Id = caminhao1.Id,
                Marca = "Volvo",
                Modelo = Modelo.FH,
                AnoFabricacao = DateTime.Now.Year,
                AnoModelo = DateTime.Now.Year + 1
            };

            Caminhao caminhao3 = await _serviceCaminhao.AtualizarCaminhao(caminhao2);

            // Assert
            Assert.True(caminhao3.Id == caminhao1.Id, "Atualizar Caminhão deve retornar Caminhão com mesmo Id");
            Assert.True(caminhao3.Equals(caminhao2), "Atualizar Caminhão deve retornar Caminhão igual o atualizado (com todos os atributos iguais)");
        }

        [Fact]
        public async Task AtualizarCaminhaoComAnoDoModeloMaiorQueOSubsequenteAoAtualDeveRetornarValidationException()
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
            Caminhao caminhao1 = await _serviceCaminhao.CadastrarCaminhao(caminhao);

            Caminhao caminhao2 = new()
            {
                Id = caminhao1.Id,
                Marca = "Volvo",
                Modelo = Modelo.FH,
                AnoFabricacao = DateTime.Now.Year,
                AnoModelo = DateTime.Now.Year + 2
            };

            var mensagem = "Ano do Modelo pode ser o atual ou o ano subsequente";

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(async () => await _serviceCaminhao.AtualizarCaminhao(caminhao2));
            Assert.True(exception.Message == mensagem, "Atualizar Caminhão com Ano do Modelo maior que o subsequente ao atual deve retornar ValidationException " +
                                                      $"com a mensagem: {mensagem}");
        }

        [Fact]
        public async Task AtualizarCaminhaoComAnoDoModeloMenorQueOAtualDeveRetornarValidationException()
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
            Caminhao caminhao1 = await _serviceCaminhao.CadastrarCaminhao(caminhao);

            Caminhao caminhao2 = new()
            {
                Id = caminhao1.Id,
                Marca = "Volvo",
                Modelo = Modelo.FH,
                AnoFabricacao = DateTime.Now.Year,
                AnoModelo = DateTime.Now.Year - 1
            };

            var mensagem = "Ano do Modelo pode ser o atual ou o ano subsequente";

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(async () => await _serviceCaminhao.AtualizarCaminhao(caminhao2));
            Assert.True(exception.Message == mensagem, "Atualizar Caminhão com Ano do Modelo menor que o atual deve retornar ValidationException " +
                                                      $"com a mensagem: {mensagem}");
        }

        [Fact]
        public async Task AtualizarCaminhaoComObjetoNuloDeveRetornarArgumentNullException()
        {
            // Arrange
            Caminhao caminhao = null;

            var mensagem = "Value cannot be null. (Parameter 'instance')";

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(async () => await _serviceCaminhao.AtualizarCaminhao(caminhao));
            Assert.True(exception.Message == mensagem, "Atualizar Caminhão com Objeto nulo deve retornar ArgumentNullException " +
                                                       $"com a mensagem: {mensagem}");
        }

        [Fact]
        public async Task AtualizarCaminhaoComIdInexistenteDeveRetornarDbUpdateConcurrencyException()
        {
            // Arrange
            Caminhao caminhao = new()
            {
                Id = "AtualizarComIdInexistenteServiceCaminhao",
                Marca = "Volkswagen",
                Modelo = Modelo.FM,
                AnoFabricacao = DateTime.Now.Year,
                AnoModelo = DateTime.Now.Year
            };

            var mensagem = "Attempted to update or delete an entity that does not exist in the store.";

            // Act & Assert
            var exception = await Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => await _serviceCaminhao.AtualizarCaminhao(caminhao));
            Assert.True(exception.Message == mensagem, "Atualizar Caminhão com Id inexistente deve retornar DbUpdateConcurrencyException " +
                                                       $"com a mensagem: {mensagem}");
        }

        [Fact]
        public async Task RemoverCaminhaoPorIdDeveRetornarTrue()
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
            Caminhao caminhao1 = await _serviceCaminhao.CadastrarCaminhao(caminhao);
            var retorno = await _serviceCaminhao.RemoverCaminhao(caminhao1.Id);

            // Assert
            Assert.True(retorno, "Remover Caminhão deve retornar True");
        }

        [Fact]
        public async Task RemoverCaminhaoComIdInexistenteDeveRetornarKeyNotFoundException()
        {
            // Arrange
            var id = "IdInexistente2ServiceCaminhao";

            var mensagem = "The Id specified for accessing Caminhao record does not match any Id in the database";

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(async () => await _serviceCaminhao.RemoverCaminhao(id));
            Assert.True(exception.Message == mensagem, "Remover Caminhão com Id inexistente deve retornar KeyNotFoundException " +
                                                       $"com a mensagem: {mensagem}");
        }
    }
}
