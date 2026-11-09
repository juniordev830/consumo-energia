namespace ConsumoEnergia;

public sealed record SimulacaoHoras(
    string Aparelho,
    decimal HorasOriginais,
    decimal HorasSimuladas,
    ComparacaoCenarios Comparacao);

/// <summary>
/// Simulates one appliance's daily usage change without modifying source input.
/// Difference = new scenario minus original; negative values indicate reduction.
/// </summary>
public static class SimuladorHoras
{
    public static SimulacaoHoras Simular(Cenario cenario, string nomeAparelho, decimal horasPorDia)
    {
        ArgumentNullException.ThrowIfNull(cenario);
        // Run the same range and duplicate-name validation as the existing estimator.
        Estimador.Calcular(cenario);
        if (string.IsNullOrWhiteSpace(nomeAparelho) || nomeAparelho.Length > 100)
            throw new ArgumentException("Nome do aparelho invalido.", nameof(nomeAparelho));
        if (horasPorDia < 0m || horasPorDia > 24m)
            throw new ArgumentOutOfRangeException(nameof(horasPorDia),
                "Horas diarias devem estar entre zero e 24.");

        var nome = nomeAparelho.Trim();
        var appliance = cenario.Aparelhos.SingleOrDefault(a =>
            string.Equals(a.Nome.Trim(), nome, StringComparison.OrdinalIgnoreCase));
        if (appliance is null)
            throw new ArgumentException("Aparelho nao encontrado no cenario.", nameof(nomeAparelho));

        var updated = new Cenario(cenario.TarifaPorKwh, cenario.Aparelhos.Select(a =>
            string.Equals(a.Nome.Trim(), nome, StringComparison.OrdinalIgnoreCase)
                ? a with { HorasPorDia = horasPorDia }
                : a).ToList());
        return new SimulacaoHoras(
            appliance.Nome.Trim(), appliance.HorasPorDia, horasPorDia,
            ComparadorCenarios.Comparar(cenario, updated));
    }
}
