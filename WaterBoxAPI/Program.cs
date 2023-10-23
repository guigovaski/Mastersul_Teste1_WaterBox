using System.Runtime.CompilerServices;
using WaterBoxAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/water-box", (WaterBoxVO vo) =>
{    
    try
    {
        var result = WaterBoxService.VerifySensors(vo.S1, vo.S2, vo.S3, vo.S4);
        return Results.Ok(result);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
})
.WithName("WaterBox");

app.Run();

internal record WaterBoxVO(int S1, int S2, int S3, int S4);
