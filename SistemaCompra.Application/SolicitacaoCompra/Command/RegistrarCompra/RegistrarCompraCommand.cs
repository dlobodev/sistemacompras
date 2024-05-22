using MediatR;
using SistemaCompra.Application.SolicitacaoCompra.Command.ItemCompra;
using SistemaCompra.Domain.Core.Model;
using System.Collections.Generic;

namespace SistemaCompra.Application.SolicitacaoCompra.Command.RegistrarCompra
{
    public class RegistrarCompraCommand : IRequest<OperationResult>
    {
        public string UsuarioSolicitante { get; set; }
        public string NomeFornecedor { get; set; } 
        public int ValorCondicaoPagamento { get; set; }
        public List<ItemCompraCommand> Itens { get; set; }
    }
}
