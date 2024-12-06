Imports System.IO
Imports System.Security.Cryptography.X509Certificates
Module Guard

    Private upperX As Integer
    Private upperY As Integer
    Private Solutions As New HashSet(Of (x As Integer, y As Integer))
    Private Probeersels As New HashSet(Of (x As Integer, y As Integer))

    Public Sub Patrol()


        Dim fileName As String = "Guard.txt"
        Dim projectPath As String = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName
        Dim pad As String = Path.Combine(projectPath, fileName)

        Dim lijnen() As String = File.ReadAllLines(pad)

        Dim Map As New Dictionary(Of (x As Integer, y As Integer), String)
        Dim guardPos As (x As Integer, y As Integer)

        upperX = lijnen.Length() - 1
        upperY = lijnen.First.Length() - 1

        For x = 0 To upperX
            Dim lijn = lijnen(x)
            For y = 0 To upperY
                Map.Add((x, y), lijn(y))
                If lijn(y) = "^" Then guardPos = (x, y)
            Next
        Next

        Dim stopwatchExec As Stopwatch = Stopwatch.StartNew()

        Dim direction As Short = 0
        Dim XCount As Integer = 0

        Map(guardPos) = "0"

        'Console.WriteLine("Startpose = " + guardPos.x.ToString + " - " + guardPos.y.ToString)

        VindLoop(guardPos, Map, direction, True)

        stopwatchExec.Stop()

        ' Console.WriteLine(Probeersels.Count)
        Console.WriteLine(Solutions.Count.ToString + " - Zonder input in " + stopwatchExec.ElapsedMilliseconds.ToString + "ms")
    End Sub

    Public Sub VindLoop(guardpos As (x As Integer, y As Integer), Map As Dictionary(Of (x As Integer, y As Integer), String), direction As Short, Mainloop As Boolean)
        Dim obstaclePos As (x As Integer, y As Integer) = (-1, -1)

        While True

            Dim nextpos = GetNextPos(direction, guardpos)

            Dim nextChar As String

            If Not Map.TryGetValue(nextpos, nextChar) Then
                'Console.WriteLine("No loop found")
                Exit Sub
            End If

            Select Case nextChar
                Case "."
                    If Mainloop Then
                        If Not Probeersels.Contains(nextpos) Then
                            Dim MapMetExtraObstakel As New Dictionary(Of (x As Integer, y As Integer), String)(Map)
                            'Console.WriteLine(nextpos.x.ToString + " - " + nextpos.y.ToString)
                            MapMetExtraObstakel(nextpos) = "F"
                            VindLoop(guardpos, MapMetExtraObstakel, direction, False)
                            Probeersels.Add(nextpos)
                        End If
                    End If

                    guardpos = nextpos
                    Map(guardpos) = direction.ToString

                Case "#"
                    direction = (direction + 1) Mod 4

                Case "F"
                    If obstaclePos.x <> -1 Then
                    Else
                        obstaclePos = nextpos
                    End If
                    direction = (direction + 1) Mod 4

                Case Else
                    If nextChar.Contains(direction.ToString) Then
                        ' Console.WriteLine(nextpos.x.ToString + " - " + nextpos.y.ToString)                    

                        Solutions.Add(obstaclePos)
                        Exit Sub
                    End If

                    guardpos = nextpos
                    Map(guardpos) = nextChar + direction.ToString
            End Select


            'For x = 0 To upperX
            '    For y = 0 To upperY
            '        Console.Write(Map((x, y)))
            '    Next
            '    Console.WriteLine()
            'Next
            'Console.WriteLine()
        End While

        Console.WriteLine("Error?")
    End Sub

    Private Function GetNextPos(direction As Short, currentPos As (x As Integer, y As Integer)) As (x As Integer, y As Integer)
        Select Case direction
            Case 0
                Return (currentPos.x - 1, currentPos.y)
            Case 1
                Return (currentPos.x, currentPos.y + 1)
            Case 2
                Return (currentPos.x + 1, currentPos.y)
            Case 3
                Return (currentPos.x, currentPos.y - 1)
        End Select

    End Function

    Public Sub Patrol1()
        Dim fileName As String = "Guard.txt"
        Dim projectPath As String = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName
        Dim pad As String = Path.Combine(projectPath, fileName)

        Dim lijnen() As String = File.ReadAllLines(pad)

        Dim Map As New Dictionary(Of (x As Integer, y As Integer), Char)
        Dim guardPos As (x As Integer, y As Integer)

        Dim upperX As Integer = lijnen.Length() - 1
        Dim upperY As Integer = lijnen.First.Length() - 1

        For x = 0 To upperX
            Dim lijn = lijnen(x)
            For y = 0 To upperY
                Map.Add((x, y), lijn(y))
                If lijn(y) = "^" Then guardPos = (x, y)
            Next
        Next

        Dim direction As Short = 0
        Dim XCount As Integer = 0

        Map(guardPos) = "X"
        XCount += 1

        While guardPos.x <= upperX AndAlso guardPos.y <= upperY
            Dim nextpos As (x As Integer, y As Integer)

            Select Case direction
                Case 0
                    nextpos = (guardPos.x - 1, guardPos.y)
                Case 1
                    nextpos = (guardPos.x, guardPos.y + 1)
                Case 2
                    nextpos = (guardPos.x + 1, guardPos.y)
                Case 3
                    nextpos = (guardPos.x, guardPos.y - 1)
            End Select

            Dim nextChar As Char

            If Not Map.TryGetValue(nextpos, nextChar) Then
                Exit While
            End If

            Select Case nextChar
                Case "."
                    guardPos = nextpos
                    Map(guardPos) = "X"
                    XCount += 1
                Case "X"
                    guardPos = nextpos
                Case "#"
                    direction = (direction + 1) Mod 4
            End Select
        End While

        For x = 0 To upperX
            Dim lijn = lijnen(x)
            For y = 0 To upperY
                Console.Write(Map((x, y)))
            Next
            Console.WriteLine()
        Next

        Console.WriteLine(XCount)

    End Sub


End Module
