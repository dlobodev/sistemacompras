using System;

namespace SistemaCompra.Application.SolicitacaoCompra.Command.ItemCompra
{
    public class ItemCompraCommand
    {
        public Guid ProdutoId { get; set; }
        public int Qtde { get; set; }
    }
}
