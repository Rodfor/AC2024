Imports System.IO
Imports Microsoft.VisualBasic.FileIO

Module Warehouse
    Private upperX As Integer
    Private upperY As Integer
    Private Map As New Dictionary(Of (x As Integer, y As Integer), Char)
    Private guardpos As (x As Integer, y As Integer)

    Public Sub Push()
        Dim fileName As String = "Warehouse.txt"
        Dim projectPath As String = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName
        Dim pad As String = Path.Combine(projectPath, fileName)

        Dim lijnen() As String = File.ReadAllLines(pad)
        Dim moves As New List(Of Char)

        Dim deel1 As Boolean = True
        Dim y As Integer = -1

        For Each L In lijnen
            If L = "" Then
                deel1 = False
                Continue For
            ElseIf deel1 Then
                y += 1
            End If

            If deel1 Then
                upperX = L.Length * 2 - 1
                For x = 0 To L.Length - 1
                    Select Case L(x)
                        Case "."
                            Map.Add((x * 2, y), L(x))
                            Map.Add((x * 2 + 1, y), L(x))
                        Case "#"
                            Map.Add((x * 2, y), L(x))
                            Map.Add((x * 2 + 1, y), L(x))
                        Case "@"
                            Map.Add((x * 2, y), L(x))
                            Map.Add((x * 2 + 1, y), ".")
                            guardpos = (x * 2, y)
                        Case "O"
                            Map.Add((x * 2, y), "[")
                            Map.Add((x * 2 + 1, y), "]")
                    End Select
                Next
            Else
                moves.AddRange(L.ToArray)
            End If
        Next

        upperY = y

        For Each M In moves

            Move(guardpos, M, True)
        Next

        Dim totaal As Integer
        For y = 0 To upperY
            Dim lijnstring = ""

            For x = 0 To upperX
                lijnstring += Map((x, y))
                If Map((x, y)) = "[" Then
                    totaal += 100 * y + x
                End If
            Next

            Console.WriteLine(lijnstring)
        Next

        Console.WriteLine(totaal)
    End Sub


    Private Function Move(source As (x As Integer, y As Integer), direction As Char, execute As Boolean) As Boolean
        Dim target As (x As Integer, y As Integer)
        Select Case direction
            Case ">"
                target = (source.x + 1, source.y)
            Case "<"
                target = (source.x - 1, source.y)
            Case "v"
                target = (source.x, source.y + 1)
            Case "^"
                target = (source.x, source.y - 1)
        End Select

        Dim moving As Boolean = False
        Dim moveDouble As Boolean = False
        Dim secondTarget As (x As Integer, y As Integer)

        Select Case Map(target)
            Case "."
                moving = True
            Case "["
                If direction = "v" OrElse direction = "^" Then
                    secondTarget = (target.x + 1, target.y)
                    moving = Move(target, direction, False) AndAlso Move(secondTarget, direction, False)
                    If moving Then
                        Move(target, direction, execute)
                        Move(secondTarget, direction, execute)
                    End If
                Else
                    moveDouble = True
                    If direction = "<" Then
                        Console.WriteLine("error")
                    ElseIf direction = ">" Then
                        secondTarget = (target.x + 1, target.y)
                    End If
                    moving = Move(secondTarget, direction, execute)
                End If
            Case "]"
                If direction = "v" OrElse direction = "^" Then
                    secondTarget = (target.x - 1, target.y)
                    moving = Move(target, direction, False) AndAlso Move(secondTarget, direction, False)
                    If moving Then
                        Move(target, direction, execute)
                        Move(secondTarget, direction, execute)
                    End If
                Else
                    moveDouble = True
                    If direction = ">" Then
                        Console.WriteLine("error")
                    ElseIf direction = "<" Then
                        secondTarget = (target.x - 1, target.y)
                    End If
                    moving = Move(secondTarget, direction, execute)
                End If
            Case "#"
                moving = False
        End Select

        If moving Then
            If execute Then
                If Map(source) = "@" Then
                    guardpos = target
                End If
                If moveDouble Then
                    Map(secondTarget) = Map(target)
                End If
                Map(target) = Map(source)
                Map(source) = "."
            End If
        Else
            ' Console.WriteLine("Blocked : " + target.x.ToString + "," + target.y.ToString + " - " + direction)
        End If

        Return moving

    End Function


    Public Sub Push1()
        Dim fileName As String = "Warehouse.txt"
        Dim projectPath As String = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName
        Dim pad As String = Path.Combine(projectPath, fileName)

        Dim lijnen() As String = File.ReadAllLines(pad)
        Dim moves As New List(Of Char)

        Dim deel1 As Boolean = True
        Dim y As Integer = -1

        For Each L In lijnen
            If L = "" Then
                deel1 = False
                Continue For
            ElseIf deel1 Then
                y += 1
            End If

            If deel1 Then
                upperX = L.Length - 1
                For x = 0 To upperX
                    Map.Add((x, y), L(x))
                    If L(x) = "@" Then
                        guardpos = (x, y)
                    End If
                Next
            Else
                moves.AddRange(L.ToArray)
            End If
        Next

        upperY = y

        For Each M In moves
            Move1(guardpos, M)
        Next

        Dim totaal As Integer

        For y = 0 To upperY
            Dim lijnstring = ""

            For x = 0 To upperX
                lijnstring += Map((x, y))
                If Map((x, y)) = "O" Then
                    totaal += 100 * y + x
                End If
            Next

            Console.WriteLine(lijnstring)
        Next

        Console.WriteLine(totaal)
    End Sub

    Private Function Move1(source As (x As Integer, y As Integer), direction As Char) As Boolean
        Dim target As (x As Integer, y As Integer)
        Select Case direction
            Case ">"
                target = (source.x + 1, source.y)
            Case "<"
                target = (source.x - 1, source.y)
            Case "v"
                target = (source.x, source.y + 1)
            Case "^"
                target = (source.x, source.y - 1)
        End Select

        Dim moving As Boolean = False

        Select Case Map(target)
            Case "."
                moving = True
            Case "O"
                moving = Move1(target, direction)
            Case "#"
                moving = False
        End Select

        If moving Then
            If Map(source) = "@" Then
                guardpos = target
            End If
            Map(target) = Map(source)
            Map(source) = "."
        End If

        Return moving

    End Function

End Module
