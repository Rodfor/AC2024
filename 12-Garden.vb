Imports System.IO

Module Garden
    Private upperX As Integer
    Private upperY As Integer
    Private Map As New Dictionary(Of (x As Integer, y As Integer), Char)
    Private areas As New Dictionary(Of Integer, Dictionary(Of (x As Integer, y As Integer), Integer))
    Private hoekenInt As New Dictionary(Of Integer, Integer)

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
            hoekenInt.Add(area, 0)
            FindNeighbours(N.Key, area)

            For Each T In areas(area)
                subtotaal += T.Value
                Map(T.Key) = "/"
            Next

            Dim prijs As Integer = areas(area).Count * subtotaal
            Dim prijsBulk As Integer = areas(area).Count * hoekenInt(area)

            ' Console.WriteLine(N.Value + " - " + areas(area).Count.ToString + " * " + subtotaal.ToString + " = " + prijs.ToString)

            Console.WriteLine(N.Value + " - " + areas(area).Count.ToString + " * " + hoekenInt(area).ToString + " = " + prijsBulk.ToString)

            totaal += prijsBulk

            area += 1
        Next


        Console.WriteLine(totaal)
    End Sub


    Private Sub FindNeighbours(Node As (x As Integer, y As Integer), Area As Integer)
        If Not areas(Area).TryAdd(Node, 0) Then
            Exit Sub
        End If

        Dim aantalNeighbours As Integer = 0
        Dim Teken As Char = Map(Node)
        Dim links As Boolean = False
        Dim rechts As Boolean = False
        Dim boven As Boolean = False
        Dim onder As Boolean = False

        Dim linksboven As Boolean = False
        Dim rechtsboven As Boolean = False
        Dim linksonder As Boolean = False
        Dim rechtsonder As Boolean = False

        If Node.x > 0 Then
            If Map((Node.x - 1, Node.y)) = Teken Then
                FindNeighbours((Node.x - 1, Node.y), Area)
                aantalNeighbours += 1
                boven = True
            End If

            If Node.y > 0 Then
                If Map((Node.x - 1, Node.y - 1)) = Teken Then linksboven = True
            End If
            If Node.y < upperY Then
                If Map((Node.x - 1, Node.y + 1)) = Teken Then rechtsboven = True
            End If
        End If

        If Node.x < upperX Then
            If Map((Node.x + 1, Node.y)) = Teken Then
                FindNeighbours((Node.x + 1, Node.y), Area)
                aantalNeighbours += 1
                onder = True
            End If

            If Node.y > 0 Then
                If Map((Node.x + 1, Node.y - 1)) = Teken Then linksonder = True
            End If
            If Node.y < upperY Then
                If Map((Node.x + 1, Node.y + 1)) = Teken Then rechtsonder = True
            End If
        End If

        If Node.y > 0 Then
            If Map((Node.x, Node.y - 1)) = Teken Then
                FindNeighbours((Node.x, Node.y - 1), Area)
                aantalNeighbours += 1
                links = True
            End If
        End If

        If Node.y < upperY Then
            If Map((Node.x, Node.y + 1)) = Teken Then
                FindNeighbours((Node.x, Node.y + 1), Area)
                aantalNeighbours += 1
                rechts = True
            End If
        End If

        areas(Area)(Node) = 4 - aantalNeighbours

        If Not links Then
            If Not boven OrElse linksboven Then
                hoekenInt(Area) += 1
            End If

            If Not onder OrElse linksonder Then
                hoekenInt(Area) += 1
            End If

        End If

        If Not rechts Then
            If Not boven OrElse rechtsboven Then
                hoekenInt(Area) += 1
            End If


            If Not onder OrElse rechtsonder Then
                hoekenInt(Area) += 1
            End If

        End If

    End Sub


End Module
