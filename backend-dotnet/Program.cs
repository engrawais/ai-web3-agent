using backend_dotnet.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGatewayServices(builder.Configuration);

var app = builder.Build();
app.UseGatewayPipeline();

app.Run();
