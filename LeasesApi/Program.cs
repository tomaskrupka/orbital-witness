using LeasesApi.Extensions;
using LeasesApi.EyeExam;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLeasesApi(builder.Configuration); // TODO: Don't pass full app configuration, just minimal sufficient subsection.

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();


var options = app.Services.GetRequiredService<IOptions<EyeExamClientOptions>>().Value;

app.Run();
