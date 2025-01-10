Imports System.IO
Imports System.Net.Mime.MediaTypeNames
Imports System.Runtime.InteropServices

Module Fruit
    Private Draden As New Dictionary(Of String, Integer?)
    Private Gates As New List(Of Gate)
    Private xWires As New List(Of String)
    Private yWires As New List(Of String)
    Private zWires As New List(Of String)

    Public Sub Wires()
        Dim fileName As String = "Fruit.txt"
        Dim projectPath As String = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName
        Dim Filepath As String = Path.Combine(projectPath, fileName)

        Dim Lijnen = File.ReadAllLines(Filepath)



        Dim nextPart As Boolean = False
        For Each L In Lijnen
            If Not nextPart Then
                If L = "" Then
                    nextPart = True
                Else
                    Dim str = L.Substring(0, 3)
                    Draden.Add(str, L.Last.ToString)
                    If str.StartsWith("x") Then
                        xWires.Add(str)
                    Else
                        yWires.Add(str)
                    End If
                End If
            Else
                Dim SplitGate = L.Split(" ")
                Dim G As New Gate
                G.Inputs.Add(SplitGate(0))
                G.Inputs.Add(SplitGate(2))
                G.Output = SplitGate(4)
                G.GateType = SplitGate(1)

                Gates.Add(G)
                Draden.TryAdd(SplitGate(4), Nothing)

                If SplitGate(4).StartsWith("z") Then
                    zWires.Add(SplitGate(4))
                End If
            End If
        Next

        xWires.Sort()
        yWires.Sort()
        zWires.Sort()


        Console.WriteLine("Deel1 : " + solve1())
        Console.WriteLine("Deel2 : " + Solve2())

    End Sub

    Private Function solve1() As String
        Dim OpenGates As New Queue(Gates)

        While OpenGates.Count > 0
            Dim current As Gate = OpenGates.Dequeue

            If Draden(current.Output) Is Nothing Then
                For Each I In current.Inputs
                    If Draden(I) Is Nothing Then
                        OpenGates.Enqueue(current)
                        Continue While
                    End If
                Next

                current.Solve(Draden)

            End If

        End While

        Dim Zires As New List(Of String)

        For Each W In Draden

            If W.Key.StartsWith("z") Then
                Zires.Add(W.Key)
            End If
        Next

        Zires.Sort()

        Dim output As Long = 0

        For i = 0 To Zires.Count - 1
            If Draden(Zires(i)) = 1 Then output += 2 ^ i
        Next

        Console.WriteLine(output)

        Return output
    End Function

    Private Function Solve2() As String

        Dim Adders As New List(Of Adder)
        Dim GatesToDivide As New List(Of Gate)(Gates)
        Dim Swaps As New List(Of String)

        For i = 0 To xWires.Count - 1
            Dim A As New Adder
            A.index = i
            If A.index > 0 Then A.Carry = Adders.Last.CarryOut

            A.XOR1 = GatesToDivide.Where(Function(X) X.GateType = "XOR" AndAlso X.Inputs.Contains(xWires(A.index)) AndAlso X.Inputs.Contains(yWires(A.index))).First

            If A.index < xWires.Count - 1 Then
                A.AND1 = GatesToDivide.Where(Function(X) X.GateType = "AND" AndAlso X.Inputs.Contains(xWires(A.index)) AndAlso X.Inputs.Contains(yWires(A.index))).First
            End If

            If A.index > 0 Then
                Dim XOR2 = GatesToDivide.Where(Function(X) X.GateType = "XOR" AndAlso X.Inputs.Contains(A.Carry) AndAlso X.Inputs.Contains(A.XOR1.Output)).FirstOrDefault

                If XOR2 Is Nothing Then
                    XOR2 = GatesToDivide.Where(Function(X) X.GateType = "XOR" AndAlso X.Output = zWires(A.index)).First
                    If XOR2.Inputs.Contains(A.Carry) Then
                        Swaps.AddRange(SwapOutputs(GatesToDivide.Where(Function(y) y.Output = XOR2.Inputs.Where(Function(x) x <> A.Carry).First).First, A.XOR1))
                    Else
                        Dim carryGate = GatesToDivide.Where(Function(y) y.Output = A.Carry).First
                        Swaps.AddRange(SwapOutputs(carryGate, GatesToDivide.Where(Function(y) y.Output = XOR2.Inputs.Where(Function(x) x <> A.XOR1.Output).First).First))
                        A.Carry = carryGate.Output
                    End If
                End If

                A.XOR2 = XOR2
            End If

            If A.index < xWires.Count - 1 AndAlso A.index > 0 Then
                A.AND2 = GatesToDivide.Where(Function(X) X.GateType = "AND" AndAlso X.Inputs.Contains(A.Carry) AndAlso X.Inputs.Contains(A.XOR1.Output)).First

                Dim OR1 = GatesToDivide.Where(Function(X) X.GateType = "OR" AndAlso X.Inputs.Contains(A.AND1.Output) AndAlso X.Inputs.Contains(A.AND2.Output)).FirstOrDefault
                If OR1 Is Nothing Then
                    OR1 = GatesToDivide.Where(Function(X) X.GateType = "OR" AndAlso X.Inputs.Contains(A.AND1.Output)).FirstOrDefault
                    If OR1 Is Nothing Then
                        OR1 = GatesToDivide.Where(Function(X) X.GateType = "OR" AndAlso X.Inputs.Contains(A.AND2.Output)).FirstOrDefault
                        Swaps.AddRange(SwapOutputs(GatesToDivide.Where(Function(y) y.Output = OR1.Inputs.Where(Function(x) x <> A.AND2.Output).First).First, A.AND1))
                    Else
                        Swaps.AddRange(SwapOutputs(GatesToDivide.Where(Function(y) y.Output = OR1.Inputs.Where(Function(x) x <> A.AND1.Output).First).First, A.AND2))
                    End If
                End If
                A.OR1 = OR1

            End If

            If A.index = xWires.Count - 1 Then
                A.Out = A.XOR2.Output
            ElseIf A.index > 0 Then
                A.CarryOut = A.OR1.Output
                A.Out = A.XOR2.Output
            Else
                A.Out = A.XOR1.Output
                A.CarryOut = A.AND1.Output
            End If

            Adders.Add(A)
        Next

        Swaps.Sort()

        Return String.Join(",", Swaps)
    End Function

    Private Function SwapOutputs(G1 As Gate, G2 As Gate) As List(Of String)
        Dim swaps As New List(Of String)
        swaps.Add(G1.Output)
        swaps.Add(G2.Output)

        G2.Output = G1.Output
        G1.Output = swaps.Last

        Console.WriteLine(G1.ToString + " swapt met " + G2.ToString)

        Return swaps
    End Function


    Private Class Gate
        Public Inputs As New List(Of String)
        Public Output As String

        Public GateType As String

        Public Sub Solve(Wires As Dictionary(Of String, Integer?))
            Select Case GateType
                Case "AND"
                    If Wires(Inputs(0)) = 1 AndAlso Wires(Inputs(1)) = 1 Then
                        Wires(Output) = 1
                    Else
                        Wires(Output) = 0
                    End If
                Case "OR"
                    If Wires(Inputs(0)) = 1 OrElse Wires(Inputs(1)) = 1 Then
                        Wires(Output) = 1
                    Else
                        Wires(Output) = 0
                    End If
                Case "XOR"
                    If Wires(Inputs(0)) = Wires(Inputs(1)) Then
                        Wires(Output) = 0
                    Else
                        Wires(Output) = 1
                    End If
            End Select
        End Sub

        Public Overrides Function ToString() As String
            Return Inputs(0) + " " + GateType + " " + Inputs(1) + " -> " + Output
        End Function

    End Class

    Private Class Adder
        Public index As Integer

        Public Carry As String

        Public XOR1 As Gate
        Public XOR2 As Gate
        Public AND1 As Gate
        Public AND2 As Gate
        Public OR1 As Gate

        Public CarryOut As String
        Public Out As String

    End Class


End Module
