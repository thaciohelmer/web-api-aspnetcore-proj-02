using ApiCatalogo.Context;
using ApiCatalogo.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();


app.MapGet("/categorias", async (AppDbContext context) => await context.Categorias.ToListAsync());

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
    var categoria =  await context.Categorias.FindAsync(id);

    if (categoria is null) return Results.NotFound("Categoria não encontrada.");

    context.Categorias.Remove(categoria);
    await context.SaveChangesAsync();

    return Results.Ok($"A categoria {categoria.Nome} foi excluida com sucesso.");
});

/* ENDPOINTS PRODUTOS */

app.MapGet("/produtos", async (AppDbContext context) => await context.Produtos.ToListAsync());

app.MapGet("/produtos/{id}", async(AppDbContext context, int id) =>
{
    var produto = await context.Produtos.FindAsync(id);

    if (produto is null) return Results.NotFound("Produto não encontrada.");

    return Results.Ok(produto);
});

app.MapPost("/produtos", async (AppDbContext context, Produto produto) =>
{
    context.Produtos.Add(produto);
    context.SaveChangesAsync();

    return Results.Created($"/produtos/{produto.ProdutoId})",produto);
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


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
