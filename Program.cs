var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "My API", Version = "v1" });
});
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => "Hello World!");
app.MapGet("/colors", () => new[] { "black", "brown", "red", "orange", "yellow", "green", "blue", "violet", "grey", "white", "gold", "silver" });
app.MapGet("/colors/{color}", (string color) => 
{
    if(true){
        return Results.Ok(new Color(0, 0, 0));
    }
    else
    {
        return Results.NotFound();
    }
})
    .Produces<Color>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)
    .WithOpenApi(o =>
    {
        o.Description = "Get a color";
        o.Responses[((int)StatusCodes.Status200OK).ToString()].Description = "Details for the color band";
        o.Responses[((int)StatusCodes.Status404NotFound).ToString()].Description = "Color not found";
        return o;
    });

app.MapGet("resistors/value-from-bands", (string firstBand, string secondBand, string thirdBand, string multiplier, string tolerance) => 
{
    if(true){
        return Results.Ok(new ResistorValues(0, 0));
    }
    else
    {
        return Results.NotFound();
    }
})
    .Produces<ResistorValues>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)
    .WithOpenApi(o =>
    {
        o.Description = "Get a resistor value from bands";
        o.Responses[((int)StatusCodes.Status200OK).ToString()].Description = "Resistor value could be decoded correctly";
        o.Responses[((int)StatusCodes.Status404NotFound).ToString()].Description = "Resistor value not found";
        return o;
    });

app.Run();

Dictionary<string, Color> colors = new()
{
    { "black", new Color(0, 1, 0) },
    { "brown", new Color(1, 10, 1) },
    { "red", new Color(2, 100, 2) },
    { "orange", new Color(3, 1000, 0) },
    { "yellow", new Color(4, 10000, 0) },
    { "green", new Color(5, 100000, 0.5) },
    { "blue", new Color(6, 1000000, 0.25) },
    { "violet", new Color(7, 10000000, 0.1) },
    { "grey", new Color(8, 100000000, 0.05) },
    { "white", new Color(9, 1000000000, 0) },
    { "gold", new Color(0, 0.1, 5) },
    { "silver", new Color(0, 0.01, 10) }
};

public record Color(int value, double multiplier, double tolerance);


public record ResistorValues(int resistorValue, int tolerance);

public record ResistorBands(string firstBand, string secondBand, string thirdBand, string multiplier, string tolerance);
