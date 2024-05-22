using MediatR;
using Microsoft.EntityFrameworkCore.Internal;
using SistemaCompra.Domain.Core;
using SistemaCompra.Domain.ProdutoAggregate;
using SistemaCompra.Domain.SolicitacaoCompraAggregate;
using SistemaCompra.Infra.Data.UoW;
using System.Threading;
using System.Threading.Tasks;
using SolicitacaoCompraAgg = SistemaCompra.Domain.SolicitacaoCompraAggregate;
using System.Linq;
using SistemaCompra.Domain.Core.Model;
using System;

namespace SistemaCompra.Application.SolicitacaoCompra.Command.RegistrarCompra
{
    public class RegistrarCompraCommandHandler : CommandHandler, IRequestHandler<RegistrarCompraCommand, OperationResult>
    {
        private readonly ISolicitacaoCompraRepository solicitacaoCompraRepository;
        private readonly IProdutoRepository produtoRepository;

        public RegistrarCompraCommandHandler(
            ISolicitacaoCompraRepository solicitacaoCompraRepository,
            IUnitOfWork uow,
            IMediator mediator,
            IProdutoRepository produtoRepository) : base(uow, mediator)
        {
            this.solicitacaoCompraRepository = solicitacaoCompraRepository;
            this.produtoRepository = produtoRepository;
        }

        public Task<OperationResult> Handle(RegistrarCompraCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.UsuarioSolicitante))
                    throw new BusinessRuleException($"Usuário solicitante '{request.UsuarioSolicitante}' inválido.");

                if (string.IsNullOrWhiteSpace(request.NomeFornecedor))
                    throw new BusinessRuleException($"Nome de Fornecedor '{request.NomeFornecedor}' inválido.");

                var condicaoPagamento = new CondicaoPagamento(request.ValorCondicaoPagamento);

                var solicitacaoCompra = new SolicitacaoCompraAgg.SolicitacaoCompra(request.UsuarioSolicitante, request.NomeFornecedor, condicaoPagamento);

                if (request.Itens is null || !request.Itens.Any())
                    throw new BusinessRuleException($"Lista de itens vazia.");

                var produtosIds = request.Itens.Select(i => i.ProdutoId);

                var produtos = produtoRepository.ObterPorListaIds(produtosIds);

                foreach (var item in request.Itens)
                {
                    var produto = produtos.FirstOrDefault(p => p.Id.Equals(item.ProdutoId));

                    if (produto is null)
                        throw new BusinessRuleException($"Produto '{item.ProdutoId}' inválido.");

                    solicitacaoCompra.AdicionarItem(produto, item.Qtde);
                }

                solicitacaoCompra.RegistrarCompra();

                solicitacaoCompraRepository.RegistrarCompra(solicitacaoCompra);

                Commit();

                PublishEvents(solicitacaoCompra.Events);

                return Task.FromResult(new OperationResult { Success = true });
            }
            catch (Exception ex)
            {
                return Task.FromResult(new OperationResult { Success = false, Message = ex.Message });
            }
        }
    }
}
