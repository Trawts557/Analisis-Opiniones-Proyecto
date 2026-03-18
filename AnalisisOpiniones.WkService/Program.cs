using AnalisisOpiniones.Data.Entities.Api;
using AnalisisOpiniones.Data.Entities.Csv;
using AnalisisOpiniones.Data.Entities.Db;
using AnalisisOpiniones.Data.Interfaces;
using AnalisisOpiniones.Data.Persistence.Repositories.Api;
using AnalisisOpiniones.Data.Persistence.Repositories.Csv;
using AnalisisOpiniones.Data.Persistence.Repositories.Db;
using AnalisisOpiniones.WkService;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<Worker>();

builder.Services.AddTransient<IFileReaderRepository<CsvModel>, CsvReaderRepository>();
builder.Services.AddTransient<IDbReaderRepository<DbModel>, DbReaderRepository>();
builder.Services.AddHttpClient<IApiReaderRepository<ApiModel>, ApiReaderRepository>();

var host = builder.Build();
host.Run();