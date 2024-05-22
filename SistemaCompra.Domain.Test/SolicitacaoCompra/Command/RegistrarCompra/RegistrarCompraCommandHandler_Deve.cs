using MediatR;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using SistemaCompra.Application.SolicitacaoCompra.Command.ItemCompra;
using SistemaCompra.Application.SolicitacaoCompra.Command.RegistrarCompra;
using SistemaCompra.Domain.ProdutoAggregate;
using SistemaCompra.Domain.SolicitacaoCompraAggregate;
using SistemaCompra.Infra.Data.UoW;
using System;
using System.Collections.Generic;
using System.Threading;
using Xunit;

namespace SistemaCompra.Domain.Test.SolicitacaoCompra.Command.RegistrarCompra
{
    public class RegistrarCompraCommandHandler_Deve
    {
        private readonly RegistrarCompraCommandHandler command;

        private readonly ISolicitacaoCompraRepository solicitacaoCompraRepository;
        private readonly IUnitOfWork uow;
        private readonly IMediator mediator;
        private readonly IProdutoRepository produtoRepository;

        public RegistrarCompraCommandHandler_Deve()
        {
            solicitacaoCompraRepository = Substitute.For<ISolicitacaoCompraRepository>();
            uow = Substitute.For<IUnitOfWork>();
            mediator = Substitute.For<IMediator>();
            produtoRepository = Substitute.For<IProdutoRepository>();

            command = new RegistrarCompraCommandHandler(solicitacaoCompraRepository, uow, mediator, produtoRepository);
        }

        [Fact]
        public void NotificarErroQuandoNaoInformadoUsuarioSolicitacao()
        {
            //Dado
            var registraCompraCommand = new RegistrarCompraCommand { UsuarioSolicitante = "" };

            //Quando 
            var result = command.Handle(registraCompraCommand, new CancellationToken()).Result;

            //Então
            Assert.False(result.Success);
            Assert.Equal("Usuário solicitante '' inválido.", result.Message);
        }

        [Fact]
        public void NotificarErroQuandoNaoInformadoNomeFornecedor()
        {
            //Dado
            var registraCompraCommand = new RegistrarCompraCommand { UsuarioSolicitante = "teste", NomeFornecedor = "" };

            //Quando 
            var result = command.Handle(registraCompraCommand, new CancellationToken()).Result;

            //Então
            Assert.False(result.Success);
            Assert.Equal("Nome de Fornecedor '' inválido.", result.Message);
        }

        [Fact]
        public void NotificarErroQuandoNomeFornecedorNaoContemPeloMenos10Caracteres()
        {
            //Dado
            var registraCompraCommand = new RegistrarCompraCommand { UsuarioSolicitante = "teste", NomeFornecedor = "teste" };

            //Quando 
            var result = command.Handle(registraCompraCommand, new CancellationToken()).Result;

            //Então
            Assert.False(result.Success);
            Assert.Equal("Nome de fornecedor deve ter pelo menos 10 caracteres.", result.Message);
        }

        [Fact]
        public void NotificarErroQuandoCondicaoPagamentoNaoEstaEntreValoresPossiveis()
        {
            //Dado
            var registraCompraCommand = new RegistrarCompraCommand { UsuarioSolicitante = "teste", NomeFornecedor = "teste xpto 1234", ValorCondicaoPagamento = 120  };

            //Quando 
            var result = command.Handle(registraCompraCommand, new CancellationToken()).Result;

            //Então
            Assert.False(result.Success);
            Assert.Equal("Condição de pagamento deve ser 0, 30, 60, 90", result.Message);
        }

        [Fact]
        public void NotificarErroQuandoListaItensVazia()
        {
            //Dado
            var registraCompraCommand = new RegistrarCompraCommand { UsuarioSolicitante = "teste", NomeFornecedor = "teste xpto 1234" };

            //Quando 
            var result = command.Handle(registraCompraCommand, new CancellationToken()).Result;

            //Então
            Assert.False(result.Success);
            Assert.Equal("Lista de itens vazia.", result.Message);
        }

        [Fact]
        public void NotificarErroQuandoAlgumProdutoInvalido()
        {
            //Dado
            var produtoId = Guid.NewGuid();
            var registraCompraCommand = new RegistrarCompraCommand
            {
                UsuarioSolicitante = "teste",
                NomeFornecedor = "teste xpto 1234",
                Itens = new List<ItemCompraCommand>
                {
                    new ItemCompraCommand { ProdutoId = produtoId, Qtde = 1 }
                }
            };

            produtoRepository.ObterPorListaIds(Arg.Any<List<Guid>>())
                .ReturnsNull();

            //Quando 
            var result = command.Handle(registraCompraCommand, new CancellationToken()).Result;

            //Então
            Assert.False(result.Success);
            Assert.Equal($"Produto '{produtoId}' inválido.", result.Message);
        }

        [Fact]
        public void NotificarErroQuandoQuantidadeProdutoZero()
        {
            //Dado
            var produto = new Produto("xpto", "xpto", "Outros", 100);

            var itens = new List<ItemCompraCommand>  {
                            new ItemCompraCommand { ProdutoId = produto.Id, Qtde = 0 }
                        };

            var registraCompraCommand = new RegistrarCompraCommand
            {
                UsuarioSolicitante = "teste",
                NomeFornecedor = "teste xpto 1234",
                Itens = itens
            };

            produtoRepository.ObterPorListaIds(Arg.Any<List<Guid>>())
                .ReturnsForAnyArgs(new List<Produto> { produto });

            //Quando 
            var result = command.Handle(registraCompraCommand, new CancellationToken()).Result;

            //Então
            Assert.False(result.Success);
            Assert.Equal("Quantidade do item deve ser maior que zero!", result.Message);
        }


        [Fact]
        public void RealizarCompraComSucesso()
        {
            //Dado
            var produto = new Produto("xpto", "xpto", "Outros", 100);

            var itens = new List<ItemCompraCommand>  {
                            new ItemCompraCommand { ProdutoId = produto.Id, Qtde = 1 }
                        };

            var registraCompraCommand = new RegistrarCompraCommand
            {
                UsuarioSolicitante = "teste",
                NomeFornecedor = "teste xpto 1234",
                Itens = itens
            };

            produtoRepository.ObterPorListaIds(Arg.Any<List<Guid>>())
                .ReturnsForAnyArgs(new List<Produto> { produto });

            //Quando 
            var result = command.Handle(registraCompraCommand, new CancellationToken()).Result;

            //Então
            Assert.True(result.Success);
            Assert.Null(result.Message);
        }
    }
}
