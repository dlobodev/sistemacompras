using System;
using System.Collections.Generic;
using System.Linq;

namespace SistemaCompra.Domain.ProdutoAggregate
{
    public interface IProdutoRepository
    {
        Produto Obter(Guid id);
        void Registrar(Produto entity);
        void Atualizar(Produto entity);
        void Excluir(Produto entity);
        IEnumerable<Produto> ObterPorListaIds(IEnumerable<Guid> produtosIds);
    }
}
