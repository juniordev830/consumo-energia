namespace ConsumoEnergia;

public sealed record RelatorioOrcamento(
    decimal LimiteMensal,
    decimal CustoEstimado,
    decimal SaldoPrevisto,
    bool AcimaDoLimite,
    decimal PercentualUtilizado,
    List<ConsumoAparelho> MaioresCustos
);

/// <summary>
/// Read-only analysis of an estimate against a user-provided monthly budget.
/// No tariff lookup, billing integration or automatic cost reduction is performed.
/// </summary>
public static class AnaliseOrcamento
{
    public static RelatorioOrcamento Analisar(Relatorio relatorio, decimal limiteMensal)
    {
        ArgumentNullException.ThrowIfNull(relatorio);
        if (limiteMensal <= 0 || limiteMensal > 1_000_000m)
            throw new ArgumentException("Orcamento deve ser maior que zero e ate R$ 1.000.000,00.");
        if (relatorio.CustoTotal < 0 || relatorio.Aparelhos is null)
            throw new ArgumentException("Relatorio de consumo invalido.");

        var top = relatorio.Aparelhos
            .OrderByDescending(aparelho => aparelho.Custo)
            .ThenByDescending(aparelho => aparelho.Kwh)
            .ThenBy(aparelho => aparelho.Nome, StringComparer.Ordinal)
            .Take(3)
            .ToList();

        return new RelatorioOrcamento(
            limiteMensal,
            relatorio.CustoTotal,
            limiteMensal - relatorio.CustoTotal,
            relatorio.CustoTotal > limiteMensal,
            decimal.Round(relatorio.CustoTotal / limiteMensal * 100m, 2, MidpointRounding.AwayFromZero),
            top
        );
    }
}
