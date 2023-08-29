using CustomerSupportChatApi.BusinessLayer.Interfaces;
using CustomerSupportChatApi.BusinessLayer.Services;
using CustomerSupportChatApi.DataLayer.Contexts;
using CustomerSupportChatApi.DataLayer.Interfaces;
using CustomerSupportChatApi.DataLayer.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient(typeof(IRepository<>), typeof(GenericRepository<>));
builder.Services.AddTransient(typeof(IService<>), typeof(BaseService<>));
builder.Services.AddTransient(typeof(IObservableQueueWrapper<>), typeof(ObservableQueueWrapper<>));
builder.Services.AddTransient(typeof(IChatSessionsService), typeof(ChatSessionsService));
builder.Services.AddTransient(typeof(ISupportTeamService), typeof(SupportTeamService));

builder.Services.AddDbContext<CustomerSupportContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
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

app.UseAuthorization();

app.MapControllers();

app.Run();
