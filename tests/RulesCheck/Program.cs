using ConsumoEnergia.Domain;
var appliance = new Appliance("Exemplo",100m,5m);
if(EnergyCalculator.Kwh(appliance,30)!=15m) throw new Exception("100 W * 5 horas * 30 dias precisa ser 15 kWh");
if(EnergyCalculator.Cost(appliance,30,1.2m)!=18m) throw new Exception("Custo incorreto");
foreach(var a in new[]{new Appliance("",100m,1m),new Appliance("Erro",-1m,1m),new Appliance("Erro",100m,25m)}){
 try{EnergyCalculator.Kwh(a,30);throw new Exception("Entrada inválida foi aceita");}
 catch(ArgumentException){ /* esperado */ }
}
Console.WriteLine("Regras de energia verificadas.");
