using backend_dotnet.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGatewayServices();

var app = builder.Build();
app.UseGatewayPipeline();

app.Run();
