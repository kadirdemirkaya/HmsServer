using Hsm.Api.Extensions;
using Hsm.Application.Extensions;
using Hsm.Persistence.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.LoadApiLayerExtension()
                .LoadApplicationLayerExtension()
                .LoadPersistenceLayerExtension();


var app = builder.Build();

app.LoadApiLayerApplicationExtension();

app.MapControllers();

app.Run();
