using GameStore.Api.Dtos;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

List<GameDto> games = [
    new (1, "Valorant", "Shooting", 19.99M, new DateOnly(1992, 7,15)),
    new (2, "League of Legends", "MOBA", 0.00M, new DateOnly(2009, 10, 27)),
    new (3, "Minecraft", "Sandbox", 26.95M, new DateOnly(2011, 11, 18)),
    new (4, "The Witcher 3: Wild Hunt", "RPG", 39.99M, new DateOnly(2015, 5, 19)),
    new (5, "Cyberpunk 2077", "RPG", 59.99M, new DateOnly(2020, 12, 10))
];

// GET /games
app.MapGet("/games", () => games);

// GET /games/{id}
app.MapGet("/games/{id}", (int id) =>
{
    var exists = games.Find(game => game.Id == id);
    if (exists is null) return Results.NotFound();

    return Results.Ok(exists);
}).WithName("GetGame");

// POST /games
app.MapPost("/games", (CreateGameDto newGame) =>
{
    GameDto game = new(games.Count + 1, newGame.Name, newGame.Genre, newGame.Price, newGame.ReleaseDate);

    games.Add(game);

    return Results.CreatedAtRoute("GetGame", new { id = game.Id }, game);
});

// PUT /games/{id}
app.MapPut("/games/{id}", (int id, UpdateGameDto updateGame) =>
{
    var index = games.FindIndex(game => game.Id == id);
    if (index == -1) return Results.NotFound();

    games[index] = new GameDto(
        id,
        updateGame.Name,
        updateGame.Genre,
        updateGame.Price,
        updateGame.ReleaseDate
    );

    return Results.NoContent();
});

// DELETE /games/{id}
app.MapDelete("/games/{id}", (int id) =>
{
    var exists = games.Find(game => game.Id == id);
    if (exists is null) return Results.NotFound();

    games.Remove(exists);
    return Results.NoContent();
});

app.Run();
