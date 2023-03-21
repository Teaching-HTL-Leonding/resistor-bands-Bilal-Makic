using System.Collections.Concurrent;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "My API", Version = "v1" });
});
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

var colors = new ConcurrentDictionary<string, Color>(){
    ["black"] = new(0, 1, 0),
    ["brown"] = new(1, 10, 1),
    ["red"] = new(2, 100, 2),
    ["orange"] = new(3, 1000, 0),
    ["yellow"] = new(4, 10000, 0),
    ["green"] = new(5, 100000, 0.5),
    ["blue"] = new(6, 1000000, 0.25),
    ["violet"] = new(7, 10000000, 0.1),
    ["grey"] = new(8, 100000000, 0.05),
    ["white"] = new(9, 1000000000, 0),
    ["gold"] = new(0, 0.1, 5),
    ["silver"] = new(0, 0.01, 10)
};

app.MapGet("/", () => "Hello World!");
app.MapGet("/colors", () => colors.Keys)
    .WithTags("Colors")
    .WithOpenApi(o =>
    {
        o.Summary = "Returns all colors for bands on resistors";

        o.Responses[((int)StatusCodes.Status200OK).ToString()].Description = "A list of all colors";

        return o;
    });


app.MapGet("/colors/{color}", (string color) => 
{
    if(colors.TryGetValue(color, out var value))
    {
        return Results.Ok(value);
    }
    else
    {
        return Results.NotFound();
    }
})
    .WithTags("Colors")
    .Produces<Color>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)
    .WithOpenApi(o =>
    {
        o.Summary = "Returns a color for bands on resistors";
        o.Parameters[0].Description = "The color to get";
        o.Responses[((int)StatusCodes.Status200OK).ToString()].Description = "A color";
        o.Responses[((int)StatusCodes.Status404NotFound).ToString()].Description = "Color not found";
        return o;
    });

app.MapGet("resistors/value-from-bands", (string firstBand, string secondBand, string thirdBand, string multiplier, string tolerance) => 
{
    
});


app.Run();


public record Color(int value, double multiplier, double tolerance);


public record ResistorValues(int resistorValue, int tolerance){
    public int ResistorValue { get; init; } = resistorValue;

    public int Tolerance { get; init; } = tolerance;
};

public record ResistorBands(string firstBand, string secondBand, string? thirdBand, string multiplier, string tolerance){
    
    public string FirstBand { get; init; } = firstBand;

    public string SecondBand { get; init; } = secondBand;

    public string? ThirdBand { get; init; } = thirdBand;

    public string Multiplier { get; init; } = multiplier;

    public string Tolerance { get; init; } = tolerance;

};
