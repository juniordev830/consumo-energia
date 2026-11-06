namespace ConsumoEnergia.Domain;
public record Appliance(string Name, decimal Watts, decimal HoursPerDay);
public static class EnergyCalculator
{
 public static decimal Kwh(Appliance appliance, int days)
 {
  if(string.IsNullOrWhiteSpace(appliance.Name)) throw new ArgumentException("Informe o aparelho");
  if(appliance.Watts <= 0 || appliance.HoursPerDay < 0 || appliance.HoursPerDay > 24 || days < 1 || days > 366)
    throw new ArgumentOutOfRangeException(nameof(appliance),"Potência, horas ou período inválido");
  return appliance.Watts / 1000m * appliance.HoursPerDay * days;
 }
 public static decimal Cost(Appliance appliance,int days,decimal tariff)
 {
  if(tariff < 0) throw new ArgumentOutOfRangeException(nameof(tariff));
  return Kwh(appliance,days) * tariff;
 }
}
