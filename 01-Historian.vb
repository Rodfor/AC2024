Imports System.Security.Cryptography
Imports Microsoft.VisualBasic.FileIO

Module Historian
    Public Sub History()
        Dim pad As String = "C:\Users\mle.SERVER\source\repos\AC2024\Historian.txt"
        Dim lijnen As New List(Of String())

        Using parser = New TextFieldParser(pad)
            parser.SetDelimiters("  ")
            While Not parser.EndOfData
                Dim fields As String() = parser.ReadFields()
                lijnen.Add(fields)
            End While

            parser.Close()
            parser.Dispose()
        End Using

        Dim Lijst1 As New List(Of Integer)
        Dim Lijst2 As New List(Of Integer)

        For Each L In lijnen
            Lijst1.Add(L(0))
            Lijst2.Add(L(1))
        Next

        Lijst1.Sort()
        Lijst2.Sort()

        Dim total As Integer = 0
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim Icnt As Integer = 0

        While i < Lijst1.Count
            Icnt += 1
            Dim nr As Integer = Lijst1(i)
            If i < Lijst1.Count - 1 AndAlso nr = Lijst1(i + 1) Then
                i += 1
                Continue While
            End If

            Dim Jcnt As Integer = 0

            While j <= Lijst2.Count
                If Lijst2(j) > Lijst1(i) Then
                    Exit While
                End If

                If Lijst1(i) = Lijst2(j) Then
                    Jcnt += 1
                End If

                j += 1
            End While

            Dim score = Jcnt * nr * Icnt
            Console.WriteLine(score)
            total += score
            Icnt = 0
            i += 1
        End While

        'Dim total As Integer = 0

        'For i = 0 To Lijst1.Count - 1
        '    total += Math.Abs(Lijst1(i) - Lijst2(i))
        'Next

        Console.WriteLine(total)
    End Sub
End Module
