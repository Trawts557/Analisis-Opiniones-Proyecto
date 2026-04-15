using AnalisisOpiniones.Data.Entities.Api;
using AnalisisOpiniones.Data.Entities.Csv;
using AnalisisOpiniones.Data.Entities.Db;
using AnalisisOpiniones.Data.Interfaces;
using AnalisisOpiniones.Data.Persistence.Context;
using AnalisisOpiniones.Data.Persistence.Repositories.Api;
using AnalisisOpiniones.Data.Persistence.Repositories.Csv;
using AnalisisOpiniones.Data.Persistence.Repositories.Db;
using AnalisisOpiniones.Data.Persistence.Repositories.Dwh;
using AnalisisOpiniones.WkService;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<Worker>();

builder.Services.AddDbContext<DwhContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DwhDb")));

builder.Services.AddTransient<IFileReaderRepository<CsvModel>, CsvReaderRepository>();
builder.Services.AddTransient<IFileReaderRepository<ClientCsvModel>, ClientCsvReaderRepository>();
builder.Services.AddTransient<IFileReaderRepository<ProductCsvModel>, ProductCsvReaderRepository>();
builder.Services.AddTransient<IFileReaderRepository<FuenteCsvModel>, FuenteCsvReaderRepository>();

builder.Services.AddTransient<IDbReaderRepository<DbModel>, DbReaderRepository>();

builder.Services.AddScoped<IDwhRepository, DwhRepository>();

builder.Services.AddHttpClient<IApiReaderRepository<ApiModel>, ApiReaderRepository>();

var host = builder.Build();
host.Run();