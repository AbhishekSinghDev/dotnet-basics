using GameStore.Api.Dtos;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
    const string GetGamesEndpoint = "GetGame";
    private static readonly List<GameDto> _games = [
        new (1, "Valorant", "Shooting", 19.99M, new DateOnly(1992, 7,15)),
        new (2, "League of Legends", "MOBA", 0.00M, new DateOnly(2009, 10, 27)),
        new (3, "Minecraft", "Sandbox", 26.95M, new DateOnly(2011, 11, 18)),
        new (4, "The Witcher 3: Wild Hunt", "RPG", 39.99M, new DateOnly(2015, 5, 19)),
        new (5, "Cyberpunk 2077", "RPG", 59.99M, new DateOnly(2020, 12, 10))
    ];

    public static void MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/games");

        // GET /games
        group.MapGet("/", () => _games);

        // GET /games/{id}
        group.MapGet("/{id}", (int id) =>
        {
            var exists = _games.Find(game => game.Id == id);
            if (exists is null) return Results.NotFound();

            return Results.Ok(exists);
        }).WithName(GetGamesEndpoint);

        // POST /games
        group.MapPost("/", (CreateGameDto newGame) =>
        {
            GameDto game = new(_games.Count + 1, newGame.Name, newGame.Genre, newGame.Price, newGame.ReleaseDate);

            _games.Add(game);

            return Results.CreatedAtRoute("GetGame", new { id = game.Id }, game);
        });

        // PUT /games/{id}
        group.MapPut("/{id}", (int id, UpdateGameDto updateGame) =>
        {
            var index = _games.FindIndex(game => game.Id == id);
            if (index == -1) return Results.NotFound();

            _games[index] = new GameDto(
                id,
                updateGame.Name,
                updateGame.Genre,
                updateGame.Price,
                updateGame.ReleaseDate
            );

            return Results.NoContent();
        });

        // DELETE /games/{id}
        group.MapDelete("/{id}", (int id) =>
        {
            var exists = _games.Find(game => game.Id == id);
            if (exists is null) return Results.NotFound();

            _games.Remove(exists);
            return Results.NoContent();
        });
    }
}
