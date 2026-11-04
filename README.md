# Consumo de Energia

> Status: concepção. Este repositório contém apenas o escopo inicial; ainda não há aplicação executável ou funcionalidades implementadas.

## Ideia

Estimar o consumo mensal de aparelhos e comparar cenários de uso.

## Primeira versão planejada

- [ ] Cadastrar potência e horas de uso de um aparelho.
- [ ] Informar tarifa por kWh.
- [ ] Exibir estimativa mensal por aparelho e total.

## Modelo inicial

Aparelho, perfil de uso e tarifa.

## Regras e limites

Calcular kWh = watts / 1000 × horas/dia × dias; tarifa informada pelo usuário, sem consulta externa.

Escopo mantido pequeno: sem pagamentos, integrações externas, infraestrutura de produção ou uso de dados pessoais reais nesta etapa.

## Tecnologia proposta

C# + .NET + Blazor.

A escolha é uma proposta para a futura implementação; nenhuma dependência foi instalada e a tecnologia ainda não foi implementada.

## Critério de conclusão do primeiro incremento

Um aparelho de 100 W usado 5 h/dia por 30 dias deve resultar em 15 kWh.

## Próximos passos

1. Revisar este escopo antes de começar a implementação.
2. Preparar o ambiente mínimo da tecnologia escolhida.
3. Implementar um fluxo completo e validar o critério acima.
4. Documentar a execução e acrescentar exemplos demonstrativos.

## Possíveis evoluções

Depois de validar o fluxo principal, avaliar persistência, acessibilidade e uma interface mais completa conforme a plataforma. Evoluções não fazem parte da entrega atual.

## Como executar

Ainda não há comandos de execução. Eles serão adicionados quando existir uma primeira versão funcional.
