Imports System

Module Program
    Function CalculateBill(kwh As Decimal, unitPrice As Decimal, tax As Decimal) As Decimal
        If kwh < 0D OrElse unitPrice < 0D OrElse tax < 0D OrElse tax > 1D Then
            Throw New ArgumentOutOfRangeException(NameOf(kwh), "Invalid energy tariff")
        End If
        Return Math.Round(kwh * unitPrice * (1D + tax), 2,
                          MidpointRounding.AwayFromZero)
    End Function

    Sub Main()
        If CalculateBill(100D, 0.75D, 0.1D) <> 82.5D Then
            Throw New Exception("Tariff calculation failed")
        End If
        Dim threw As Boolean = False
        Try
            CalculateBill(-1D, 1D, 0D)
        Catch ex As ArgumentOutOfRangeException
            threw = True
        End Try
        If Not threw Then Throw New Exception("Negative consumption was accepted")
        Console.WriteLine("VB.NET tariff checks passed")
    End Sub
End Module
