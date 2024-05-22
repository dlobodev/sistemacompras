using SistemaCompra.Domain.Core;
using SistemaCompra.Domain.Core.Model;
using SistemaCompra.Domain.ProdutoAggregate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SistemaCompra.Domain.SolicitacaoCompraAggregate
{
    public class SolicitacaoCompra : Entity
    {
        public UsuarioSolicitante UsuarioSolicitante { get; private set; }
        public NomeFornecedor NomeFornecedor { get; private set; }
        public IList<Item> Itens { get; private set; }
        public DateTime Data { get; private set; }
        public Money TotalGeral { get; private set; }
        public Situacao Situacao { get; private set; }
        public CondicaoPagamento CondicaoPagamento { get; private set; }

        private SolicitacaoCompra() { }

        public SolicitacaoCompra(string usuarioSolicitante, string nomeFornecedor, CondicaoPagamento condicaoPagamento)
        {
            Id = Guid.NewGuid();
            UsuarioSolicitante = new UsuarioSolicitante(usuarioSolicitante);
            NomeFornecedor = new NomeFornecedor(nomeFornecedor);
            Data = DateTime.Now;
            Situacao = Situacao.Solicitado;
            CondicaoPagamento = condicaoPagamento;
            Itens = new List<Item>();
        }

        public void AdicionarItem(Produto produto, int qtde)
        {
            if(qtde == default) 
                throw new BusinessRuleException("Quantidade do item deve ser maior que zero!");

            Itens.Add(new Item(produto, qtde));
        }

        public void RegistrarCompra()
        {
            if (Itens.Count() == default)
                throw new BusinessRuleException("A solicitação de compra deve possuir itens!");

            TotalGeral = new Money(Itens.Sum(i => i.Subtotal.Value));

            if(TotalGeral.Value > 50000 && CondicaoPagamento.Valor != 30)
                throw new BusinessRuleException("Para compras com valor acima de R$50.000,00, a condição de pagamento deve ser igual a 30 dias!");
        }
    }
}
