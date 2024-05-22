using SistemaCompra.Domain.Core;
using SistemaCompra.Domain.ProdutoAggregate;
using SistemaCompraAgg = SistemaCompra.Domain.SolicitacaoCompraAggregate;
using System.Collections.Generic;
using Xunit;

namespace SistemaCompra.Domain.Test.SolicitacaoCompraAggregate
{
    public class SolicitacaoCompra_RegistrarCompraDeve
    {
        [Fact]
        public void NotificarCondicaoPagamentoDeveSer30DiasAoComprarMais50mil()
        {
            //Dado
            var item = new SistemaCompraAgg.Item(new Produto("Notebook", "Notebook", "Outros", 6000), 10);
            var solicitacao = new SistemaCompraAgg.SolicitacaoCompra("rodrigoasth", "rodrigoasth", new SistemaCompraAgg.CondicaoPagamento(90));
            solicitacao.Itens.Add(item);

            //Quando 
            var ex = Assert.Throws<BusinessRuleException>(() => solicitacao.RegistrarCompra());

            //Então
            Assert.Equal("Para compras com valor acima de R$50.000,00, a condição de pagamento deve ser igual a 30 dias!", ex.Message);
        }

        [Fact]
        public void NotificarErroQuandoNaoInformarItensCompra()
        {
            //Dado
            var solicitacao = new SistemaCompraAgg.SolicitacaoCompra("rodrigoasth", "rodrigoasth", new SistemaCompraAgg.CondicaoPagamento(30));
            
            //Quando 
            var ex = Assert.Throws<BusinessRuleException>(() => solicitacao.RegistrarCompra());

            //Então
            Assert.Equal("A solicitação de compra deve possuir itens!", ex.Message);
        }
    }
}
