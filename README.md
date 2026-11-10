# Consumo de Energia

MVP em **C# / .NET 8** para estimar o consumo mensal por aparelho e comparar o custo total.

## Funcionalidades

- Recebe um cenario JSON com tarifa informada pelo usuario, potencia em watts, horas por dia e dias de uso.
- Calcula kWh = W / 1000 × horas/dia × dias para cada aparelho.
- Calcula custo em reais com arredondamento monetario por aparelho e total como soma desses valores.
- Valida dados negativos, dias fora de 1 a 31, horas acima de 24, nomes vazios e duplicados.
- Gera relatorio JSON, sem banco de dados, contas, coleta de dados ou consulta a tarifas externas.

## Limites de entrada

Cada cenário deve ser um arquivo JSON regular com no máximo **1 MB**
(1.000.000 bytes). O programa verifica o tamanho antes de ler o conteúdo
e rejeita arquivos maiores. Os documentos devem usar **UTF-8 válido**: bytes
inválidos são rejeitados, evitando substituir silenciosamente caracteres em
nomes dos aparelhos. Esse limite também se aplica aos dois cenários
passados no modo de comparação.

## Executar

Instale o SDK .NET 8 e execute na raiz:

```bash
dotnet run --project src/ConsumoEnergia -- examples/cenario.json
dotnet run --project src/ConsumoEnergia -- --self-test
```

Os exemplos sao ficticios e podem ser alterados. Em um aparelho de **100 W** usado **5 h/dia** durante **30 dias**, o resultado e **15 kWh**. Com tarifa de R$ 0,80 por kWh, custo estimado de **R$ 12,00**.

## CI e limites

Executa compilacao Release, sete verificacoes internas e exemplos de relatorio e comparacao no GitHub Actions.

Estimativas nao consideram impostos, bandeiras tarifarias, potencia variavel ou tarifas escalonadas. Valores sao aproximados e dependem dos dados informados.

Proximas etapas: persistencia local, interface acessivel e avaliacao de Blazor.


## Comparar dois cenários

O comparador recebe **dois arquivos JSON** e mostra o consumo e custo anterior, o novo valor e a diferença para cada aparelho (inclusive quando existe em apenas um cenário).

```bash
dotnet run --project src/ConsumoEnergia -- --compare examples/cenario.json examples/cenario-economico.json
```

**Diferença = novo − anterior**. Portanto, valores negativos representam redução de consumo ou despesa. O custo depende da tarifa informada em cada cenário; é possível testar a troca de tarifa e o tempo de utilização sem alterar dados anteriores. Resultados são estimativas, não uma promessa de economia na conta real.

## Planejar um limite de gastos mensais

Uma análise opcional compara a estimativa de consumo com um **orçamento mensal em reais** fornecido por você e mostra saldo previsto, percentual utilizado e os três aparelhos com maior custo estimado.

```bash
dotnet run --project src/ConsumoEnergia -- --budget examples/cenario.json 100.00
```

Use ponto para separar os centavos (ex.: `100.00`). O limite deve ficar entre R$ 0,01 e R$ 1.000.000,00. Saldo negativo indica ultrapassagem da meta. O ranking serve para revisão dos dados de consumo e **não** recomenda desligar equipamentos essenciais. O programa não acessa contas de energia, não altera tarifas e não promete redução real dos custos.

## Simular mudanças de horas por dia

O modo `--simulate` permite alterar **apenas na simulação** o número de horas diárias de um aparelho do cenário, sem editar o arquivo original. O cálculo reutiliza o estimador e comparador de cenários e apresenta as diferenças em consumo de kWh e reais.

```bash
dotnet run --project src/ConsumoEnergia -- --simulate examples/cenario.json Lampada 2.5
```

O nome do aparelho aceita diferenças entre maiúsculas e minúsculas; as novas horas devem ficar entre **0 e 24**. Uma diferença negativa de custo indica redução estimada, não economia garantida na conta real. Os testes internos abrangem redução, simulação sem alteração, aparelho inexistente e horas inválidas.
