# Consumo de Energia — Blazor MVP

Aplicação web em **C# / .NET 8 / Blazor Server** para cadastrar aparelhos, simular consumo e estimar o valor mensal da energia.

## Implementado
- Nome, potência em watts e horas de uso por dia por aparelho.
- Tarifa de energia (R$/kWh) e número de dias configuráveis.
- Tabela com kWh e custo individual, totais e remoção.
- Validação: potência positiva, horas entre 0 e 24, dias de 1 a 366, tarifa não negativa.

Cálculo: `kWh = (watts/1000) × horas/dia × dias`. Exemplo: **100 W × 5 horas × 30 dias = 15 kWh**.

## Rodar
Requer SDK .NET 8:
```bash
dotnet run --project ConsumoEnergia.csproj
```
Abra a URL local exibida no terminal.

## Validar
```bash
dotnet build ConsumoEnergia.csproj -c Release
dotnet run --project tests/RulesCheck/RulesCheck.csproj
```
O GitHub Actions executa as duas verificações.

## Limitações
Estimativa informativa; não mede consumo real. Dados só durante a sessão e sem banco de dados nesta versão; o histórico persistente pode ser implementado posteriormente.
