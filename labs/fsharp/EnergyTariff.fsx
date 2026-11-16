// Pure tariff calculator with decimal arithmetic to avoid money rounding drift.
module EnergyTariff =
    let total (peakKwh: decimal) (offPeakKwh: decimal)
              (peakPrice: decimal) (offPeakPrice: decimal)
              (taxRate: decimal) =
        if peakKwh < 0m || offPeakKwh < 0m || peakPrice < 0m ||
           offPeakPrice < 0m || taxRate < 0m || taxRate > 1m then
            invalidArg "tariff" "All quantities and prices must be nonnegative; tax is 0..1"
        let subtotal = peakKwh * peakPrice + offPeakKwh * offPeakPrice
        System.Math.Round(subtotal * (1m + taxRate), 2,
                          System.MidpointRounding.AwayFromZero)

let result = EnergyTariff.total 100m 50m 0.9m 0.5m 0.1m
if result <> 126.50m then failwithf "Unexpected bill: %M" result
let invalid =
    try
        EnergyTariff.total (-1m) 0m 0.5m 0.5m 0m |> ignore
        false
    with :? System.ArgumentException -> true
if not invalid then failwith "Negative consumption was accepted"
printfn "F# tariff checks passed"
