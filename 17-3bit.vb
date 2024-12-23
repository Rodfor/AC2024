Imports System.IO

Module _3bit
    Public Sub Operate()
        Dim fileName As String = "3bit.txt"
        Dim projectPath As String = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName
        Dim pad As String = Path.Combine(projectPath, fileName)

        Dim lijnen() As String = File.ReadAllLines(pad)

        Dim programma As New List(Of Short)
        Dim operands As New List(Of Integer) From {
            0,
            1,
            2,
            3
        }


        For Each L In lijnen
            Dim split = L.Split(":")
            If L.StartsWith("Register") Then
                operands.Add(split(1))
            ElseIf L.StartsWith("Program") Then
                For Each I In split(1)
                    If IsNumeric(I) Then
                        programma.Add(I.ToString)
                    End If
                Next
            End If
        Next

        Dim index As Short = 0
        Dim output As String = ""

        While index < programma.Count
            Select Case programma(index)
                Case 0
                    operands(4) = Math.Round(operands(4) / (2 ^ operands(5)), MidpointRounding.ToZero)
                    index += 2
                Case 1
                    operands(5) = operands(5) Xor programma(index + 1)
                    index += 2
                Case 2
                    operands(5) = operands(programma(index + 1)) Mod 8
                    index += 2
                Case 3
                    If operands(4) <> 0 Then
                        index = programma(index + 1)
                    Else
                        index += 2
                    End If
                Case 4
                    operands(5) = operands(5) Xor operands(6)
                    index += 2
                Case 5
                    output += (operands(programma(index + 1)) Mod 8).ToString + ","
                    index += 2
                Case 6
                    operands(5) = Math.Round(operands(4) / (2 ^ operands(5)), MidpointRounding.ToZero)
                    index += 2
                Case 7
                    operands(6) = Math.Round(operands(4) / (2 ^ operands(5)), MidpointRounding.ToZero)
                    index += 2
            End Select
        End While

        Console.WriteLine(output.TrimEnd(","))
    End Sub
End Module
