using ApiCatalogo.Context;
using ApiCatalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.ApiEndpoints;

public static class ProdutosEndpoints
{
    public static void MapProdutosEndpoints(this WebApplication app)
    {
        app.MapGet("/produtos", async (AppDbContext context) => await context.Produtos.ToListAsync()).WithTags("Produtos").RequireAuthorization();

        app.MapGet("/produtos/{id}", async (AppDbContext context, int id) =>
        {
            var produto = await context.Produtos.FindAsync(id);

            if (produto is null) return Results.NotFound("Produto não encontrada.");

            return Results.Ok(produto);
        });

        app.MapPost("/produtos", async (AppDbContext context, Produto produto) =>
        {
            context.Produtos.Add(produto);
            await context.SaveChangesAsync();

            return Results.Created($"/produtos/{produto.ProdutoId})", produto);
        });

        app.MapPut("/produtos/{id}", async (AppDbContext context, int id, Produto produto) =>
        {
            if (id != produto.ProdutoId) return Results.BadRequest("Não foi possivel atualizar o produto");

            var produtoDb = context.Produtos.Find(id);

            if (produtoDb is null) return Results.NotFound("Produto não encontrado.");

            produtoDb.Nome = produto.Nome;
            produtoDb.Descricao = produto.Descricao;
            produtoDb.Preco = produto.Preco;
            produtoDb.Estoque = produto.Estoque;
            produtoDb.Imagem = produto.Imagem;
            produtoDb.Categoria = produto.Categoria;

            await context.SaveChangesAsync();
            return Results.Ok(produtoDb);
        });

        app.MapDelete("/produtos/{id}", async (AppDbContext context, int id) =>
        {
            var produto = await context.Produtos.FindAsync(id);

            if (produto is null) return Results.NotFound("Produto não encontrado.");

            context.Produtos.Remove(produto);
            await context.SaveChangesAsync();

            return Results.Ok($"O produto {produto.Nome} foi excluido.");
        });

    }
}
