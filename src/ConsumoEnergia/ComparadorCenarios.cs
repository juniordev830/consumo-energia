namespace ConsumoEnergia;

public sealed record DiferencaAparelho(
    string Nome,
    decimal KwhAnterior,
    decimal KwhNovo,
    decimal DiferencaKwh,
    decimal CustoAnterior,
    decimal CustoNovo,
    decimal DiferencaCusto);

public sealed record ComparacaoCenarios(
    Relatorio Anterior,
    Relatorio Novo,
    IReadOnlyList<DiferencaAparelho> PorAparelho,
    decimal DiferencaTotalKwh,
    decimal DiferencaTotalCusto);

public static class ComparadorCenarios
{
    /// <summary>
    /// Computes a new scenario minus the baseline. Negative cost means savings.
    /// Appliances present in only one scenario have a zero on the other side.
    /// </summary>
    public static ComparacaoCenarios Comparar(Cenario anterior, Cenario novo)
    {
        var antes = Estimador.Calcular(anterior);
        var depois = Estimador.Calcular(novo);
        var anteriores = antes.Aparelhos.ToDictionary(
            item => item.Nome, StringComparer.OrdinalIgnoreCase);
        var novos = depois.Aparelhos.ToDictionary(
            item => item.Nome, StringComparer.OrdinalIgnoreCase);

        var nomes = anteriores.Keys
            .Union(novos.Keys, StringComparer.OrdinalIgnoreCase)
            .OrderBy(nome => nome, StringComparer.OrdinalIgnoreCase);
        var diferencas = new List<DiferencaAparelho>();

        foreach (var nome in nomes)
        {
            anteriores.TryGetValue(nome, out var anteriorItem);
            novos.TryGetValue(nome, out var novoItem);
            var kwhAnterior = anteriorItem?.Kwh ?? 0m;
            var kwhNovo = novoItem?.Kwh ?? 0m;
            var custoAnterior = anteriorItem?.Custo ?? 0m;
            var custoNovo = novoItem?.Custo ?? 0m;
            diferencas.Add(new DiferencaAparelho(
                novoItem?.Nome ?? anteriorItem!.Nome,
                kwhAnterior,
                kwhNovo,
                kwhNovo - kwhAnterior,
                custoAnterior,
                custoNovo,
                custoNovo - custoAnterior));
        }

        return new ComparacaoCenarios(
            antes,
            depois,
            diferencas.AsReadOnly(),
            depois.TotalKwh - antes.TotalKwh,
            depois.CustoTotal - antes.CustoTotal);
    }
}
