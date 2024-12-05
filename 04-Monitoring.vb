Imports System.IO
Imports System.Text.Json
Imports System.Xml.XPath
Imports Microsoft.VisualBasic.FileIO

Module Monitoring
    Private Pattern(,) As Node


    Public Sub Monitor()
        Dim fileName As String = "Monitoring.txt"
        Dim projectPath As String = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName
        Dim pad As String = Path.Combine(projectPath, fileName)

        Dim lijnen() As String = File.ReadAllLines(pad)


        ReDim Pattern(lijnen.Length - 1, lijnen.First.Length - 1)
        Dim LijstA As New List(Of Node)

        For i = 0 To lijnen.Length() - 1
            Dim lijn = lijnen(i)
            For j = 0 To lijn.Length - 1
                Dim N As New Node(i, j, lijn(j))

                Pattern(i, j) = N
                If N.Value = "A" Then
                    LijstA.Add(N)
                End If
            Next
        Next

        Console.WriteLine(LijstA.Count)

        Dim total As Integer = 0

        For Each A In LijstA
            If FindMS(A) Then
                total += 1
            End If
        Next

        Console.WriteLine(total)
        Console.WriteLine()

        For i = 0 To Pattern.GetUpperBound(0)
            For j = 0 To Pattern.GetUpperBound(1)
                Dim N = Pattern(i, j)
                If N.inXmas Then
                    Console.Write(N.Value)
                Else
                    Console.Write(".")
                End If
            Next
            Console.WriteLine()
        Next

    End Sub

    Private Function FindMS(N As Node) As Boolean
        Dim col = N.col
        Dim row = N.row

        Dim maxrow = Pattern.GetUpperBound(0)
        Dim maxcol = Pattern.GetUpperBound(1)

        If N.row = 0 Then Return False
        If N.col = 0 Then Return False
        If N.row = maxrow Then Return False
        If N.col = maxcol Then Return False

        Dim X As New XMAS From {
            Pattern(row - 1, col - 1),
            Pattern(row - 1, col + 1),
            Pattern(row + 1, col - 1),
            Pattern(row + 1, col + 1)
        }

        Dim ListM As New List(Of Node)
        Dim ListS As New List(Of Node)

        For Each ND In X
            Select Case ND.Value
                Case "M"
                    ListM.Add(ND)
                Case "S"
                    ListS.Add(ND)
                Case Else
                    Return False
            End Select
        Next

        If ListM.Count <> 2 OrElse ListS.Count <> 2 Then Return False

        If Math.Abs(ListM(0).col + ListM(0).row - ListM(1).col - ListM(1).row) <> 2 Then Return False

        X.Add(N)

        For Each L In X
            L.inXmas = True
        Next

        Return True

    End Function

    Public Sub Monitor1()
        Dim fileName As String = "Monitoring.txt"
        Dim projectPath As String = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName
        Dim pad As String = Path.Combine(projectPath, fileName)

        Dim lijnen() As String = File.ReadAllLines(pad)

        ReDim Pattern(lijnen.Length - 1, lijnen.First.Length - 1)
        Dim LijstX As New List(Of XMAS)

        For i = 0 To lijnen.Length() - 1
            Dim lijn = lijnen(i)
            For j = 0 To lijn.Length - 1
                Dim N As New Node(i, j, lijn(j))

                Pattern(i, j) = N
                If N.Value = "X" Then
                    Dim X As New XMAS From {
                            N
                        }
                    X.Kant = XMAS.Richting.TBT
                    LijstX.Add(X)
                End If
            Next
        Next

        Console.WriteLine(LijstX.Count)

        Dim LijstM As New List(Of XMAS)

        For Each X In LijstX
            LijstM.AddRange(SearchNodeByChar(X, "M"))
        Next

        Console.WriteLine(LijstM.Count)

        Dim LijstA As New List(Of XMAS)

        For Each M In LijstM
            LijstA.AddRange(SearchNodeByChar(M, "A"))
        Next

        Console.WriteLine(LijstA.Count)

        Dim LijstS As New List(Of XMAS)

        For Each A In LijstA
            LijstS.AddRange(SearchNodeByChar(A, "S"))
        Next

        Console.WriteLine(LijstS.Count)

    End Sub


    Private Function SearchNodeByChar(N As XMAS, c As Char) As List(Of XMAS)
        Dim Adjacent As New List(Of XMAS)
        Dim last As Node = N.Last
        Dim col = last.col
        Dim row = last.row

        Dim maxrow = Pattern.GetUpperBound(0)
        Dim maxcol = Pattern.GetUpperBound(1)


        If (N.Kant = XMAS.Richting.TBT OrElse N.Kant = XMAS.Richting.LinksBoven) AndAlso last.row > 0 AndAlso last.col > 0 AndAlso Pattern(row - 1, col - 1).Value = c Then
            Dim nieuw As New XMAS
            nieuw.AddRange(N)
            nieuw.Add(Pattern(row - 1, col - 1))
            nieuw.Kant = XMAS.Richting.LinksBoven
            Adjacent.Add(nieuw)
        End If

        If (N.Kant = XMAS.Richting.TBT OrElse N.Kant = XMAS.Richting.Boven) AndAlso last.row > 0 AndAlso Pattern(row - 1, col).Value = c Then
            Dim nieuw As New XMAS
            nieuw.AddRange(N)
            nieuw.Add(Pattern(row - 1, col))
            nieuw.Kant = XMAS.Richting.Boven
            Adjacent.Add(nieuw)
        End If

        If (N.Kant = XMAS.Richting.TBT OrElse N.Kant = XMAS.Richting.RechtsBoven) AndAlso last.row > 0 AndAlso last.col < maxcol AndAlso Pattern(row - 1, col + 1).Value = c Then
            Dim nieuw As New XMAS
            nieuw.AddRange(N)
            nieuw.Add(Pattern(row - 1, col + 1))
            nieuw.Kant = XMAS.Richting.RechtsBoven
            Adjacent.Add(nieuw)
        End If

        If (N.Kant = XMAS.Richting.TBT OrElse N.Kant = XMAS.Richting.Links) AndAlso last.col > 0 AndAlso Pattern(row, col - 1).Value = c Then
            Dim nieuw As New XMAS
            nieuw.AddRange(N)
            nieuw.Add(Pattern(row, col - 1))
            nieuw.Kant = XMAS.Richting.Links
            Adjacent.Add(nieuw)
        End If

        If (N.Kant = XMAS.Richting.TBT OrElse N.Kant = XMAS.Richting.Rechts) AndAlso last.col < maxcol AndAlso Pattern(row, col + 1).Value = c Then
            Dim nieuw As New XMAS
            nieuw.AddRange(N)
            nieuw.Add(Pattern(row, col + 1))
            nieuw.Kant = XMAS.Richting.Rechts
            Adjacent.Add(nieuw)
        End If

        If (N.Kant = XMAS.Richting.TBT OrElse N.Kant = XMAS.Richting.LinksOnder) AndAlso last.col > 0 AndAlso last.row < maxrow AndAlso Pattern(row + 1, col - 1).Value = c Then
            Dim nieuw As New XMAS
            nieuw.AddRange(N)
            nieuw.Add(Pattern(row + 1, col - 1))
            nieuw.Kant = XMAS.Richting.LinksOnder
            Adjacent.Add(nieuw)
        End If

        If (N.Kant = XMAS.Richting.TBT OrElse N.Kant = XMAS.Richting.Onder) AndAlso last.row < maxrow AndAlso Pattern(row + 1, col).Value = c Then
            Dim nieuw As New XMAS
            nieuw.AddRange(N)
            nieuw.Add(Pattern(row + 1, col))
            nieuw.Kant = XMAS.Richting.Onder
            Adjacent.Add(nieuw)
        End If

        If (N.Kant = XMAS.Richting.TBT OrElse N.Kant = XMAS.Richting.RechtsOnder) AndAlso last.col < maxcol AndAlso last.row < maxrow AndAlso Pattern(row + 1, col + 1).Value = c Then
            Dim nieuw As New XMAS
            nieuw.AddRange(N)
            nieuw.Add(Pattern(row + 1, col + 1))
            nieuw.Kant = XMAS.Richting.RechtsOnder
            Adjacent.Add(nieuw)
        End If

        Return Adjacent
    End Function

    Private Class Node

        Public row As Int16
        Public col As Int16
        Public Value As Char
        Public inXmas As Boolean = False

        Public Sub New(row As Short, col As Short, value As Char)
            Me.row = row
            Me.col = col
            Me.Value = value
        End Sub
    End Class

    Private Class XMAS
        Inherits List(Of Node)
        Public Enum Richting
            Boven
            Onder
            Links
            Rechts
            LinksBoven
            LinksOnder
            RechtsBoven
            RechtsOnder
            TBT
        End Enum

        Public Kant As Richting?

    End Class
End Module
