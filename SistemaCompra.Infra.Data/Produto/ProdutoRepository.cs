﻿using System;
using System.Collections.Generic;
using System.Linq;
using ProdutoAgg = SistemaCompra.Domain.ProdutoAggregate;

namespace SistemaCompra.Infra.Data.Produto
{
    public class ProdutoRepository : ProdutoAgg.IProdutoRepository
    {
        private readonly SistemaCompraContext context;

        public ProdutoRepository(SistemaCompraContext context)  {
            this.context = context;
        }

        public void Atualizar(ProdutoAgg.Produto entity)
        {
            context.Set<ProdutoAgg.Produto>().Update(entity);
        }

        public void Excluir(ProdutoAgg.Produto entity)
        {
            context.Set<ProdutoAgg.Produto>().Remove(entity);
        }

        public ProdutoAgg.Produto Obter(Guid id)
        {
            return context.Set<ProdutoAgg.Produto>().Where(c=> c.Id == id).FirstOrDefault();
        }

        public IEnumerable<ProdutoAgg.Produto> ObterPorListaIds(IEnumerable<Guid> produtosIds)
        {
            return context.Set<ProdutoAgg.Produto>().Where(p => produtosIds.Contains(p.Id));
        }

        public void Registrar(ProdutoAgg.Produto entity)
        {
            context.Set<ProdutoAgg.Produto>().Add(entity);
        }
    }
}
