# Consumo de Energia

MVP em **C# / .NET 8** para estimar o consumo mensal por aparelho e comparar o custo total.

## Funcionalidades

- Recebe um cenario JSON com tarifa informada pelo usuario, potencia em watts, horas por dia e dias de uso.
- Calcula kWh = W / 1000 × horas/dia × dias para cada aparelho.
- Calcula custo em reais com arredondamento monetario por aparelho e total como soma desses valores.
- Valida dados negativos, dias fora de 1 a 31, horas acima de 24, nomes vazios e duplicados.
- Gera relatorio JSON, sem banco de dados, contas, coleta de dados ou consulta a tarifas externas.

## Executar

Instale o SDK .NET 8 e execute na raiz:

```bash
dotnet run --project src/ConsumoEnergia -- examples/cenario.json
dotnet run --project src/ConsumoEnergia -- --self-test
```

Os exemplos sao ficticios e podem ser alterados. Em um aparelho de **100 W** usado **5 h/dia** durante **30 dias**, o resultado e **15 kWh**. Com tarifa de R$ 0,80 por kWh, custo estimado de **R$ 12,00**.

## CI e limites

Executa compilacao Release, quatro casos de teste internos e exemplo no GitHub Actions.

Estimativas nao consideram impostos, bandeiras tarifarias, potencia variavel ou tarifas escalonadas. Valores sao aproximados e dependem dos dados informados.

Proximas etapas: persistencia local, comparacao de dois cenarios, interface acessivel e avaliacao de Blazor.
