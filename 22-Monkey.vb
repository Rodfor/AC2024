Imports System.IO
Imports System.Security.Cryptography.X509Certificates

Module Monkey

    Public Sub Sell()
        Dim fileName As String = "Monkey.txt"
        Dim projectPath As String = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName
        Dim Filepath As String = Path.Combine(projectPath, fileName)

        Dim Secrets = File.ReadAllLines(Filepath).ToList()

        Dim totaal As Long = 0

        Dim Changes As New List(Of Dictionary(Of (Integer, Integer, Integer, Integer), Integer))

        For Each S As Long In Secrets
            Dim ChangeStack As New Queue
            Dim previous As Integer

            Dim SecretChanges As New Dictionary(Of (Integer, Integer, Integer, Integer), Integer)
            previous = S.ToString.Last.ToString

            For i = 1 To 2000

                S = ((S * 64) Xor S) Mod 16777216
                S = (Math.Round(S / 32, mode:=MidpointRounding.ToNegativeInfinity) Xor S) Mod 16777216
                S = ((S * 2048) Xor S) Mod 16777216

                Dim last As Integer = S.ToString.Last.ToString
                Dim diff = last - previous
                previous = last

                ChangeStack.Enqueue(diff)

                If i >= 4 Then
                    SecretChanges.TryAdd((ChangeStack(0), ChangeStack(1), ChangeStack(2), ChangeStack(3)), last)
                    ChangeStack.Dequeue()
                End If

            Next

            Changes.Add(SecretChanges)
            totaal += S
            Console.WriteLine(S)
        Next

        Dim maxPrijs As Integer = 0

        Console.WriteLine($"Deel 1 : {totaal}")

        For i = -9 To 9
            For j = -9 To 9
                For k = -9 To 9
                    For l = -9 To 9
                        Dim current As Integer = 0

                        For Each C In Changes
                            Dim Enkel As Integer
                            If C.TryGetValue((i, j, k, l), Enkel) Then
                                current += Enkel
                            End If
                        Next

                        If current > maxPrijs Then maxPrijs = current
                    Next
                Next
            Next
        Next

        Console.WriteLine($"Deel 2 : {maxPrijs}")

    End Sub

End Module
