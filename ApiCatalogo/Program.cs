using ApiCatalogo.ApiEndpoints;
using ApiCatalogo.AppServicesExtensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddApiSwagger();
builder.AddPersistence();
builder.Services.AddCors();
builder.AddAutenticationJwt();

var app = builder.Build();

app.MapAutenticacaoEndpoint();
app.MapCategoriasEndpoints();
app.MapProdutosEndpoints();

var environment = app.Environment;
app.UseExceptionHadling(environment).UseSwaggerMiddleware().UseAppCors();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
