using backend_SG.Data;
using backend_SG.DTOs;
using backend_SG.Extensions;
using backend_SG.Services;
using Microsoft.EntityFrameworkCore;

namespace backend_SG.Tests
{
    public class UsuarioServiceTest
    {
        private AppDbContext CriarDbContextNaMemoria()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task CadastrarUsuario_DeveRetornarSucesso()
        {
            var dbContext = CriarDbContextNaMemoria();
            var service = new UsuarioService(dbContext);
            var dto = new CriarUsuarioDTO
            {
                Nome = "Rafael Silva Fortuna",
                Email = "teste@teste.com",
                Senha = "123123123"
            };

            var resultado = await service.Cadastrar(dto);

            Assert.Null(resultado);
        }

        [Theory]
        [InlineData("Carlos123")]
        [InlineData("Ana@Empresa")]
        [InlineData("Zé")]
        public async Task CadastrarUsuario_DeveRetornarErro_QuandoNomeForInvalido(string nomeInvalido)
        {
            var dbContext = CriarDbContextNaMemoria();
            var service = new UsuarioService(dbContext);

            var dto = new CriarUsuarioDTO
            {
                Nome = nomeInvalido,
                Email = "teste@teste.com",
                Senha = "123"
            };

            var resultado = await service.Cadastrar(dto);

            Assert.Null(resultado);
        }

        [Theory]
        [InlineData("mine2")]
        [InlineData("testedesenha")]
        [InlineData("")]
        [InlineData("123")]
        public async Task CadastrarUsuario_DeveRetornarErro_QuandoSenhaForInvalido(string senhaInvalida)
        {
            var dbContext = CriarDbContextNaMemoria();
            var service = new UsuarioService(dbContext);

            var dto = new CriarUsuarioDTO
            {
                Nome = "NomeValido",
                Email = "teste@teste.com",
                Senha = senhaInvalida
            };

            var resultado = await service.Cadastrar(dto);

            Assert.Null(resultado);
        }

        [Theory]
        [InlineData("email.com")]
        [InlineData("email@email")]
        [InlineData("")]
        public async Task CadastrarUsuario_DeveRetornarErro_QuandoEmailForInvalido(string emailInvalido)
        {
            var dbContext = CriarDbContextNaMemoria();
            var service = new UsuarioService(dbContext);

            var dto = new CriarUsuarioDTO
            {
                Nome = "NomeValido",
                Email = emailInvalido,
                Senha = "SenhaValida1"
            };

            var resultado = await service.Cadastrar(dto);

            Assert.Null(resultado);
        }

        [Theory]
        [InlineData("SenhaCom1", true)]
        [InlineData("12345678", true)]
        [InlineData("Abcdefg8", true)]
        [InlineData("SenhaS1", false)]
        [InlineData("SenhaSemNumero", false)]
        [InlineData("", false)]
        public void IsSenhaValida_DeveValidarTamanhoENumero_ConformeRegras(string senhaParaTestar, bool resultadoEsperado)
        {
            bool resultadoActual = senhaParaTestar.IsSenhaValida();

            Assert.Equal(resultadoEsperado, resultadoActual);
        }

        [Fact]
        public async Task AutenticarUsuario_DeveRetornarSucesso_QuandoUsuarioExistirESenhaForCorreta()
        {
            var dbContext = CriarDbContextNaMemoria();
            var service = new UsuarioService(dbContext);

            var dto = new CriarUsuarioDTO
            {
                Nome = "Usuario Teste",
                Cpf = "595.613.560-31",
                Email = "olamundo@email.com",
                Senha = "123123123Senha1"
            };

            var usuarioCadastrado = await service.Cadastrar(dto);
            Assert.NotNull(usuarioCadastrado);

            var resultadoLogin = await service.Autenticar(dto.Email, dto.Senha);

            Assert.NotNull(resultadoLogin);
            Assert.Equal(dto.Email, resultadoLogin.Email);
        }

        [Theory]
        [InlineData("emailInvalido.com", "123123123Senha1")]
        [InlineData("olamundo@email.com", "senhaErrada123")]
        [InlineData("naoexiste@email.com", "123123123Senha1")]
        public async Task AutenticarUsuario_DeveRetornarNull_QuandoDadosForemIncorretosOuInexistentes(string emailTestar, string senhaTestar)
        {
            var dbContext = CriarDbContextNaMemoria();
            var service = new UsuarioService(dbContext);

            var dtoBase = new CriarUsuarioDTO
            {
                Nome = "Usuario Base",
                Cpf = "111.222.333-44",
                Email = "olamundo@email.com",
                Senha = "123123123Senha1"
            };
            await service.Cadastrar(dtoBase);

            var resultado = await service.Autenticar(emailTestar, senhaTestar);

            Assert.Null(resultado);
        }


    }
}
