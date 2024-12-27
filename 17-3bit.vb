Imports System.IO

Module _3bit
    Public Sub Operate()
        Dim fileName As String = "3bit.txt"
        Dim projectPath As String = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName
        Dim pad As String = Path.Combine(projectPath, fileName)

        Dim lijnen() As String = File.ReadAllLines(pad)

        Dim programma As New List(Of Short)
        Dim operands As New List(Of Long) From {
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

        Dim output As New List(Of Short)
        Dim newOperands As New List(Of Long)

        Dim registerA As Long = 0

        Dim outputindex As Short = programma.Count - 1 - 2

        While outputindex >= 0

            newOperands.Clear()
            newOperands.AddRange(operands)
            newOperands(4) = registerA

            output.Clear()

            Dim count As Integer = 0

            Dim index As Short = 0
            Dim found As Boolean = False

            While True
                If count > 99999 Then Exit While
                count += 1
                Select Case programma(index)
                    Case 0
                        newOperands(4) = Math.Round(newOperands(4) / (2 ^ newOperands(programma(index + 1))), MidpointRounding.ToZero)
                        index += 2
                    Case 1
                        newOperands(5) = newOperands(5) Xor programma(index + 1)
                        index += 2
                    Case 2
                        newOperands(5) = newOperands(programma(index + 1)) Mod 8
                        index += 2
                    Case 3
                        If newOperands(4) <> 0 Then
                            index = programma(index + 1)
                        Else
                            index += 2
                        End If
                    Case 4
                        newOperands(5) = newOperands(5) Xor newOperands(6)
                        index += 2
                    Case 5
                        Dim newOutput As Short = newOperands(programma(index + 1)) Mod 8
                        'If newOutput <> programma(output.Count) Then Exit While
                        If output.Count = outputindex Then
                            If Not programma(outputindex) = newOutput Then
                                registerA += 8 ^ (Math.Max(outputindex, 2) - 2)
                                'Console.WriteLine("A : " + registerA.ToString)
                                Exit While
                            Else
                                Console.WriteLine(outputindex.ToString + " Found - " + newOutput.ToString + " - A: " + registerA.ToString)
                                found = True
                                'If outputindex <> -1 Then
                                '    output.Add(newOutput)
                                '    Exit While
                                'End If
                            End If
                        End If
                        output.Add(newOutput)
                        index += 2
                    Case 6
                        newOperands(5) = Math.Round(newOperands(4) / (2 ^ newOperands(programma(index + 1))), MidpointRounding.ToZero)
                        index += 2
                    Case 7
                        newOperands(6) = Math.Round(newOperands(4) / (2 ^ newOperands(programma(index + 1))), MidpointRounding.ToZero)
                        index += 2
                End Select

                If index >= programma.Count Then
                    ' Console.WriteLine("A : " + registerA.ToString + "- " + String.Join(",", output))
                    If Not found Then
                        registerA += 8 ^ (Math.Max(outputindex, 2) - 2)
                    Else
                        If output.Count > outputindex + 2 AndAlso output(outputindex + 1) = programma(outputindex + 1) AndAlso output(outputindex + 2) = programma(outputindex + 2) Then
                            Console.WriteLine("A : " + registerA.ToString + "- " + String.Join(",", output))
                            outputindex -= 1
                        Else
                            registerA += 8 ^ (Math.Max(outputindex, 3) - 2)
                        End If
                    End If
                    Exit While
                End If

            End While

        End While

        Console.WriteLine(registerA.ToString + " - " + String.Join(",", output))
    End Sub

    Public Sub Operate1()
        Dim fileName As String = "3bit.txt"
        Dim projectPath As String = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName
        Dim pad As String = Path.Combine(projectPath, fileName)

        Dim lijnen() As String = File.ReadAllLines(pad)

        Dim programma As New List(Of Short)
        Dim operands As New List(Of Long) From {
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

        Dim output As New List(Of Short)


        Dim index As Short = 0
            While index < programma.Count
                Select Case programma(index)
                    Case 0
                    operands(4) = Math.Round(operands(4) / (2 ^ operands(programma(index + 1))), MidpointRounding.ToZero)
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
                    Dim newOutput As Short = operands(programma(index + 1)) Mod 8
                    output.Add(newOutput)
                    index += 2
                    Case 6
                    operands(5) = Math.Round(operands(4) / (2 ^ operands(programma(index + 1))), MidpointRounding.ToZero)
                    index += 2
                    Case 7
                    operands(6) = Math.Round(operands(4) / (2 ^ operands(programma(index + 1))), MidpointRounding.ToZero)
                    index += 2
                End Select
            End While

        Console.WriteLine(String.Join(",", output))

    End Sub
End Module
