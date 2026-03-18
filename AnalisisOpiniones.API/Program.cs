using AnalisisOpiniones.Data.Entities.Api;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/api/social-comments", () =>
{
    var comments = new List<ApiModel>
    {
        new ApiModel
        {
            IdComment = 1,
            IdCliente = 201,
            IdProducto = 3001,
            Fuente = "RedSocial",
            Fecha = new DateTime(2025, 1, 10),
            Comentario = "Excelente producto, muy recomendado."
        },
        new ApiModel
        {
            IdComment = 2,
            IdCliente = 202,
            IdProducto = 3002,
            Fuente = "RedSocial",
            Fecha = new DateTime(2025, 1, 11),
            Comentario = "No me gustó la calidad."
        },
        new ApiModel
        {
            IdComment = 3,
            IdCliente = 203,
            IdProducto = 3001,
            Fuente = "RedSocial",
            Fecha = new DateTime(2025, 1, 12),
            Comentario = "Buen precio y entrega rápida."
        },
        new ApiModel
        {
            IdComment = 4,
            IdCliente = 204,
            IdProducto = 3003,
            Fuente = "RedSocial",
            Fecha = new DateTime(2025, 1, 13),
            Comentario = "Llegó con retraso, pero funciona bien."
        },
        new ApiModel
        {
            IdComment = 5,
            IdCliente = 205,
            IdProducto = 3004,
            Fuente = "RedSocial",
            Fecha = new DateTime(2025, 1, 14),
            Comentario = "Muy mala experiencia."
        }
    };

    return Results.Ok(comments);
});

app.Run();