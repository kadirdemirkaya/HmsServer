using Hsm.Api.Extensions;
using Hsm.Application.Extensions;
using Hsm.Persistence.Extensions;

public class Program
{
    static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.LoadApiLayerExtension()
                        .LoadApplicationLayerExtension()
                        .LoadPersistenceLayerExtension(Hsm.Domain.AssemblyReference.Assembly);

        var app = builder.Build();

        app.LoadApiLayerApplicationExtension()
           .LoadPersistenceLayerApplicationExtension();

        app.MapControllers();

        app.Run();
    }
}