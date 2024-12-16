Imports System.IO

Module Garden
    Private upperX As Integer
    Private upperY As Integer
    Private Map As New Dictionary(Of (x As Integer, y As Integer), Char)
    Private areas As New Dictionary(Of Integer, Dictionary(Of (x As Integer, y As Integer), Integer))

    Public Sub Perimeter()
        Dim fileName As String = "Garden.txt"
        Dim projectPath As String = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName
        Dim pad As String = Path.Combine(projectPath, fileName)

        Dim lijnen() As String = File.ReadAllLines(pad)

        upperX = lijnen.Length() - 1
        upperY = lijnen.First.Length() - 1

        For x = 0 To upperX
            Dim lijn = lijnen(x)
            For y = 0 To upperY
                Map.Add((x, y), lijn(y))
            Next
        Next

        Dim area As Integer = 0
        Dim totaal As Integer = 0

        For Each N In Map
            If N.Value = "/" Then Continue For
            Dim subtotaal As Integer = 0
            areas.Add(area, New Dictionary(Of (x As Integer, y As Integer), Integer))
            Dim kanten = FindNeighboursAndHoeken(N.Key, area, -1)

            For Each T In areas(area)
                subtotaal += T.Value
                Map(T.Key) = "/"
            Next

            Dim prijs As Integer = areas(area).Count * subtotaal
            Dim prijsBulk As Integer = areas(area).Count * kanten

            ' Console.WriteLine(N.Value + " - " + areas(area).Count.ToString + " * " + subtotaal.ToString + " = " + prijs.ToString)

            Console.WriteLine(N.Value + " - " + areas(area).Count.ToString + " * " + kanten.ToString + " = " + prijsBulk.ToString)

            totaal += prijs

            area += 1
        Next


        Console.WriteLine(totaal)
    End Sub

    Private Function FindNeighboursAndHoeken(Node As (x As Integer, y As Integer), Area As Integer, richting As Short) As Integer
        Dim aantalNeighbours As Integer = 0
        Dim Teken As Char = Map(Node)
        Dim hoeken As Integer = 0

        If Node.x > 0 Then
            If Map((Node.x - 1, Node.y)) = Teken Then
                aantalNeighbours += 1
                If areas(Area).TryAdd((Node.x - 1, Node.y), 0) Then
                    hoeken += FindNeighboursAndHoeken((Node.x - 1, Node.y), Area, 0)
                    If richting <> 0 Then
                        hoeken += 1
                    End If
                End If
            End If
        End If
        If Node.x < upperX Then
            If Map((Node.x + 1, Node.y)) = Teken Then
                aantalNeighbours += 1
                If areas(Area).TryAdd((Node.x + 1, Node.y), 0) Then
                    hoeken += FindNeighboursAndHoeken((Node.x + 1, Node.y), Area, 1)
                    If richting <> 1 Then
                        hoeken += 1
                    End If
                End If
            End If
        End If
        If Node.y > 0 Then
            If Map((Node.x, Node.y - 1)) = Teken Then
                aantalNeighbours += 1
                If areas(Area).TryAdd((Node.x, Node.y - 1), 0) Then
                    hoeken += FindNeighboursAndHoeken((Node.x, Node.y - 1), Area, 2)
                    If richting <> 2 Then
                        hoeken += 1
                    End If
                End If
            End If
        End If
        If Node.y < upperY Then
            If Map((Node.x, Node.y + 1)) = Teken Then
                aantalNeighbours += 1
                If areas(Area).TryAdd((Node.x, Node.y + 1), 0) Then
                    hoeken += FindNeighboursAndHoeken((Node.x, Node.y + 1), Area, 3)
                    If richting <> 3 Then
                        hoeken += 1
                    End If
                End If
            End If
        End If

        areas(Area)(Node) = 4 - aantalNeighbours

        Return hoeken

    End Function


    Private Sub FindNeighbours(Node As (x As Integer, y As Integer), Area As Integer)
        Dim aantalNeighbours As Integer = 0
        Dim Teken As Char = Map(Node)

        If Not areas(Area).TryAdd(Node, 0) Then
            Exit Sub
        End If

        If Node.x > 0 Then
            If Map((Node.x - 1, Node.y)) = Teken Then
                FindNeighbours((Node.x - 1, Node.y), Area)
                aantalNeighbours += 1
            End If
        End If
        If Node.x < upperX Then
            If Map((Node.x + 1, Node.y)) = Teken Then
                FindNeighbours((Node.x + 1, Node.y), Area)
                aantalNeighbours += 1
            End If
        End If
        If Node.y > 0 Then
            If Map((Node.x, Node.y - 1)) = Teken Then
                FindNeighbours((Node.x, Node.y - 1), Area)
                aantalNeighbours += 1
            End If
        End If
        If Node.y < upperY Then
            If Map((Node.x, Node.y + 1)) = Teken Then
                FindNeighbours((Node.x, Node.y + 1), Area)
                aantalNeighbours += 1
            End If
        End If

        areas(Area)(Node) = 4 - aantalNeighbours

    End Sub


End Module
