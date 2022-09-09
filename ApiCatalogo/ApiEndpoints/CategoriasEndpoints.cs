using ApiCatalogo.Context;
using ApiCatalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.ApiEndpoints;

public static class CategoriasEndpoints
{
    public static void MapCategoriasEndpoints(this WebApplication app)
    {
        app.MapGet("/categorias", async (AppDbContext context) => await context.Categorias.ToListAsync()).WithTags("Categorias").RequireAuthorization();

        app.MapGet("/categorias/{id}", async (AppDbContext context, int id) =>
        {
            return await context.Categorias.FindAsync(id) is Categoria categoria ? Results.Ok(categoria) : Results.NotFound("Categoria não encontrada.");
        });

        app.MapPost("/categorias", async (AppDbContext context, Categoria categoria) =>
        {
            context.Categorias.Add(categoria);
            await context.SaveChangesAsync();

            return Results.Created($"/categorias/{categoria.CategoriaId}", categoria);
        });

        app.MapPut("/categorias/{id}", async (AppDbContext context, int id, Categoria categoria) =>
        {
            if (categoria.CategoriaId != id) return Results.BadRequest("Não foi possível atualizar a categoria.");

            var categoriaBd = await context.Categorias.FindAsync(id);

            if (categoriaBd is null) return Results.NotFound("Categoria não encontrada.");

            categoriaBd.Nome = categoria.Nome;
            categoriaBd.Descricao = categoria.Descricao;

            await context.SaveChangesAsync();
            return Results.Ok(categoriaBd);
        });

        app.MapDelete("/categorias/{id}", async (AppDbContext context, int id) =>
        {
            var categoria = await context.Categorias.FindAsync(id);

            if (categoria is null) return Results.NotFound("Categoria não encontrada.");

            context.Categorias.Remove(categoria);
            await context.SaveChangesAsync();

            return Results.Ok($"A categoria {categoria.Nome} foi excluida com sucesso.");
        });
    }
}
