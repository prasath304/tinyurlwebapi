using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using tinyurlwebapi.Data.TinyUrlWebApi.Data;
using tinyurlwebapi.Model;
using AutoMapper;
using tinyurlwebapi.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy => policy.WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});


// Register DbContext with connection string
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
app.UseCors("AllowAngular");


// ✅ GET all
app.MapGet("/api/GetAll", (AppDbContext context, IMapper mapper) => {
    var urls = context.Urls.ToList();
    var urlsList = mapper.Map<List<Urls>>(urls); // Entity → DTO
   
    return Results.Ok(urlsList);
    });


// ✅ GET by id
app.MapGet("/api/Get/{code:int}", async (int code, AppDbContext context,IMapper mapper) =>
{
    var urls = await context.Urls.FirstOrDefaultAsync(u => u.Id == code);
    var urlsList = mapper.Map<Urls>(urls);
    return urlsList is not null ? Results.Ok(urlsList) : Results.NotFound();
});


// ✅ POST (create new)
app.MapPost("/api/Save", async (Urls url, AppDbContext context,IMapper mapper) =>
{
    url.CreatedAt = DateTime.UtcNow;
    var urls= mapper.Map<UrlsDto>(url);
    context.Urls.Add(urls);
    await context.SaveChangesAsync();
    return Results.Created($"/api/Get/{url.Id}", url);

});


// ✅ PUT (update existing)
app.MapPut("/api/Update/{id:int}", async (int id, Urls updatedUrl, AppDbContext context, IMapper mapper) =>
{
    var urlDto = await context.Urls.FindAsync(id);
    if (urlDto is null) return Results.NotFound();

    urlDto.OriginalUrl = updatedUrl.OriginalUrl;
    urlDto.ShortCode = updatedUrl.ShortCode;
    urlDto.ClickCount = updatedUrl.ClickCount;

    context.Update(context.Urls.FindAsync(urlDto));
    return Results.Created($"/api/Get/{urlDto.Id}", urlDto);

});


// ✅ DELETE
app.MapDelete("/api/Delete/{id:int}", async (int id, AppDbContext context) =>
{
    var url = await context.Urls.FindAsync(id);
    if (url is null) return Results.NotFound();

    context.Remove(url);
    await context.SaveChangesAsync();
    return Results.NoContent();
});
// ✅ DELETE
app.MapDelete("/api/DeleteAll/", async (AppDbContext context) =>
{
    context.Urls.RemoveRange(context.Urls);
    await context.SaveChangesAsync();
        return Results.NoContent();

});


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = "swagger"; 

    });

}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
