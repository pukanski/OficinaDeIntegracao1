using FluentAssertions;
using Moq;
using ProfessorAPI.src.DTOs;
using ProfessorAPI.src.Model;
using ProfessorAPI.src.Repositories;
using ProfessorAPI.src.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProfessorAPI.Tests.Services
{
    public class ProfessorServiceTests
    {
        //mock do repositório — simula o banco sem precisar de conexão real
        private readonly Mock<IProfessorRepository> _repositoryMock;
        private readonly ProfessorService _service;

        public ProfessorServiceTests()
        {
            _repositoryMock = new Mock<IProfessorRepository>();
            _service = new ProfessorService(_repositoryMock.Object);
        }

        //helpers
        private static ProfessorRequestDTO MockCreateProfessorDTO() => new()
        {
            primeiroNome = "Roberto",
            ultimoNome = "Carlos",
            email = "rcarlos@escola.com",
            siape = "2837483",
            disciplina = "Matemática"
        };

        private static Professor MockCreateProfessorModel(long id = 1) => new()
        {
            Id = id,
            PrimeiroNome = "Roberto",
            UltimoNome = "Carlos",
            Email = "rcarlos@escola.com",
            Siape = "2837483",
            Disciplina = "Matemática",
            CriadoEm = DateTime.UtcNow
        };

        //testar a criação de um professor
        [Fact]
        public async Task CreateProfessorAsync_DeveCriarProfessor_QuandoDadosValidos()
        {
            //arrange — prepara o cenári
            var requestDTO = MockCreateProfessorDTO();
            var professor = MockCreateProfessorModel();

            _repositoryMock
                .Setup(r => r.ExisteSiapeAsync(requestDTO.siape, null))
                .ReturnsAsync(false);

            _repositoryMock
                .Setup(r => r.ExisteEmailAsync(requestDTO.email, null))
                .ReturnsAsync(false);

            _repositoryMock
                .Setup(r => r.CreateProfessorAsync(It.IsAny<Professor>()))
                .ReturnsAsync(professor);

            //act — executa o método
            var resultado = await _service.CreateProfessorAsync(requestDTO);

            // Assert — verifica o resultado
            resultado.Should().NotBeNull();
            resultado.primeiroNome.Should().Be("Roberto");
            resultado.siape.Should().Be("2837483");

            // Verifica que o repositório foi chamado exatamente 1 vez
            _repositoryMock.Verify(r => r.CreateProfessorAsync(It.IsAny<Professor>()), Times.Once);
        }

        [Fact]
        public async Task CreateProfessorAsync_DeveLancarExcecao_QuandoSIAPEDuplicado()
        {
            //arrange

            var requestDTO = MockCreateProfessorDTO();

            _repositoryMock
                .Setup(r => r.ExisteSiapeAsync(requestDTO.siape, null))
                .ReturnsAsync(true); // simula SIAPE já existente

            //act
            var acao = async () => await _service.CreateProfessorAsync(requestDTO);

            //assert — espera a exceção com a mensagem certa
            await acao.Should()
                .ThrowAsync<InvalidOperationException>()
                .WithMessage("SIAPE já cadastrado.");

            //garante que o repositório NÃO foi chamado para criar
            _repositoryMock.Verify(r => r.CreateProfessorAsync(It.IsAny<Professor>()), Times.Never);
        }

        [Fact]
        public async Task GetProfessorByIdAsync_DeveRetornarProfessor_QuandoExiste()
        {
            //arrange
            var professor = MockCreateProfessorModel(id: 1);
            _repositoryMock
                .Setup(r => r.GetProfessorByIdAsync(1))
                .ReturnsAsync(professor);

            //act
            var resultado = await _service.GetProfessorByIdAsync(1);

            //assert
            resultado.Should().NotBeNull();
            resultado!.id.Should().Be(1);
            resultado.primeiroNome.Should().Be("Roberto");

        }

        [Fact]
        public async Task GetAllProfessoresByIdAsync_DeveRetornarProfessor_QuandoExistem()
        {
            //arrange

            var professores = new List<Professor>
            {
                MockCreateProfessorModel(id: 1),
                MockCreateProfessorModel(id: 2)
            };

            _repositoryMock
                .Setup(r => r.GetAllProfessoresAsync())
                .ReturnsAsync(professores);

            //act
            var resultado = await _service.GetAllProfessoresAsync();

            //assert
            resultado.Should().NotBeNull();
            resultado.Should().HaveCount(2);
        }
    }
}

