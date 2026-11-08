using System.Text.Json;
using System.Text.Json.Serialization;

namespace ConsumoEnergia;

public sealed record Aparelho(string Nome, decimal Watts, decimal HorasPorDia, int Dias);
public sealed record Cenario(decimal TarifaPorKwh, List<Aparelho> Aparelhos);
public sealed record ConsumoAparelho(string Nome, decimal Kwh, decimal Custo);
public sealed record Relatorio(decimal TarifaPorKwh, List<ConsumoAparelho> Aparelhos, decimal TotalKwh, decimal CustoTotal);

public static class Estimador
{
    public static Relatorio Calcular(Cenario cenario)
    {
        ArgumentNullException.ThrowIfNull(cenario);
        if (cenario.TarifaPorKwh < 0 || cenario.TarifaPorKwh > 1000)
            throw new ArgumentException("Tarifa fora do intervalo de 0 a 1000 R$/kWh.");
        if (cenario.Aparelhos is null || cenario.Aparelhos.Count is < 1 or > 500)
            throw new ArgumentException("Informe entre 1 e 500 aparelhos.");

        var nomes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var linhas = new List<ConsumoAparelho>();
        decimal totalKwh = 0;
        decimal custoTotal = 0;
        foreach (var aparelho in cenario.Aparelhos)
        {
            if (aparelho is null || string.IsNullOrWhiteSpace(aparelho.Nome) || aparelho.Nome.Length > 100 ||
                aparelho.Watts <= 0 || aparelho.Watts > 1_000_000 ||
                aparelho.HorasPorDia < 0 || aparelho.HorasPorDia > 24 ||
                aparelho.Dias < 1 || aparelho.Dias > 31)
                throw new ArgumentException("Aparelho com nome, potencia, horas ou dias invalidos.");
            var nome = aparelho.Nome.Trim();
            if (!nomes.Add(nome))
                throw new ArgumentException("Nome de aparelho repetido: " + nome);

            var kwh = aparelho.Watts / 1000m * aparelho.HorasPorDia * aparelho.Dias;
            var custo = decimal.Round(kwh * cenario.TarifaPorKwh, 2, MidpointRounding.AwayFromZero);
            linhas.Add(new ConsumoAparelho(nome, kwh, custo));
            totalKwh += kwh;
            custoTotal += custo;
        }
        return new Relatorio(cenario.TarifaPorKwh, linhas, totalKwh, custoTotal);
    }
}

public static class Program
{
    public static int Main(string[] args)
    {
        try
        {
            if (args.Length == 1 && args[0] == "--self-test")
            {
                SelfTest();
                Console.WriteLine("Testes internos OK");
                return 0;
            }
            var options = new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
                PropertyNameCaseInsensitive = true,
                NumberHandling = JsonNumberHandling.Strict
            };
            var outputOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web) { WriteIndented = true };
            if (args.Length == 1)
            {
                var scenario = ReadScenario(args[0], options);
                Console.WriteLine(JsonSerializer.Serialize(Estimador.Calcular(scenario), outputOptions));
                return 0;
            }
            if (args.Length == 3 && args[0] == "--compare")
            {
                var before = ReadScenario(args[1], options);
                var after = ReadScenario(args[2], options);
                Console.WriteLine(JsonSerializer.Serialize(ComparadorCenarios.Comparar(before, after), outputOptions));
                return 0;
            }
            throw new ArgumentException(
                "Uso: dotnet run --project src/ConsumoEnergia -- CENARIO.json | --compare ANTES.json DEPOIS.json");
        }
        catch (Exception error) when (error is ArgumentException or IOException or JsonException or OverflowException)
        {
            Console.Error.WriteLine("Erro: " + error.Message);
            return 2;
        }
    }

    private const long MaxScenarioBytes = 1_000_000;

    private static Cenario ReadScenario(string path, JsonSerializerOptions options)
    {
        if (!File.Exists(path))
            throw new ArgumentException("Caminho deve apontar para um arquivo JSON existente.");

        var file = new FileInfo(path);
        if (file.Length > MaxScenarioBytes)
            throw new ArgumentException("Cenario excede limite de 1 MB.");

        var text = File.ReadAllText(path);
        // Validate again in case the file changed during reading.
        if (System.Text.Encoding.UTF8.GetByteCount(text) > MaxScenarioBytes)
            throw new ArgumentException("Cenario excede limite de 1 MB.");

        return JsonSerializer.Deserialize<Cenario>(text, options)
            ?? throw new ArgumentException("Esperado objeto JSON de cenario.");
    }

    private static void Assert(bool condition)
    {
        if (!condition) throw new InvalidOperationException("Falha em teste interno.");
    }

    private static void SelfTest()
    {
        var report = Estimador.Calcular(new Cenario(0.80m, new List<Aparelho> {
            new("Lampada", 100m, 5m, 30)
        }));
        Assert(report.TotalKwh == 15m && report.CustoTotal == 12m);
        var multiple = Estimador.Calcular(new Cenario(1m, new List<Aparelho> {
            new("A", 1000m, 1m, 30),
            new("B", 500m, 2m, 30)
        }));
        Assert(multiple.TotalKwh == 60m && multiple.CustoTotal == 60m);
        Assert(Throws(() => Estimador.Calcular(new Cenario(1m, new List<Aparelho> {
            new("A", 100m, 25m, 30)
        }))));
        Assert(Throws(() => Estimador.Calcular(new Cenario(1m, new List<Aparelho> {
            new("A", 100m, 1m, 30),
            new("a", 100m, 1m, 30)
        }))));
        var before = new Cenario(1m, new List<Aparelho> {
            new("A", 1000m, 1m, 30)
        });
        var after = new Cenario(0.8m, new List<Aparelho> {
            new("A", 500m, 1m, 30),
            new("B", 100m, 2m, 30)
        });
        var comparison = ComparadorCenarios.Comparar(before, after);
        Assert(comparison.DiferencaTotalKwh == -9m && comparison.DiferencaTotalCusto == -13.2m);
        Assert(comparison.PorAparelho.Count == 2 &&
            comparison.PorAparelho[0].Nome == "A" &&
            comparison.PorAparelho[0].DiferencaCusto == -18m &&
            comparison.PorAparelho[1].Nome == "B" &&
            comparison.PorAparelho[1].CustoAnterior == 0m);
        Assert(Throws(() => ComparadorCenarios.Comparar(before,
            new Cenario(-1m, new List<Aparelho> { new("A", 10m, 1m, 1) }))));

        // The CLI must reject oversized input before JSON deserialization.
        var oversizedPath = Path.Combine(Path.GetTempPath(), "energia-" + Guid.NewGuid() + ".json");
        try
        {
            File.WriteAllText(oversizedPath, new string(' ', 1_000_001));
            var readOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            Assert(Throws(() => ReadScenario(oversizedPath, readOptions)));
        }
        finally
        {
            if (File.Exists(oversizedPath))
                File.Delete(oversizedPath);
        }

    }

    private static bool Throws(Action action)
    {
        try { action(); return false; }
        catch (ArgumentException) { return true; }
    }
}
