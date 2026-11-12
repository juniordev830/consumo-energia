# Consumo de Energia: guia técnico

## Escopo real

CLI .NET 8 de estimativa de kWh e custo baseado na tarifa e no uso informados pelo usuário.

## Arquivos relacionados

`src/ConsumoEnergia/Program.cs`, `SimuladorHoras.cs`, `ComparadorCenarios.cs`, `AnaliseOrcamento.cs`.

## Procedimento de validação

Usar o `.csproj` em `src/ConsumoEnergia/`, compilar com `dotnet build` e executar exemplos definidos no README.

Se algo não puder ser executado, registrar como pendente, sem afirmar sucesso. Preservar resultados reais de CI vinculados aos commits relevantes.

## Casos para revisão

Testar potência zero, tarifa inválida, dias acima de 31, UTF-8 inválido, arquivo maior que 1 MB e soma monetária com arredondamento por aparelho.

## Próxima evolução

Os números são estimativas, sem leitura de medidor ou consulta à distribuidora. Documentar premissas de cada cálculo.

## Para submeter uma mudança

- [ ] Especificar problema e comportamento esperado.
- [ ] Anexar evidência de teste ou reprodução real.
- [ ] Revisar acessibilidade, privacidade e licenças quando aplicável.
- [ ] Submeter em branch separada e revisar antes de merge.
