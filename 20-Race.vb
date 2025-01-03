Imports System.IO

Module Race

    Public Sub Condition()
        Dim fileName As String = "Race.txt"
        Dim projectPath As String = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName
        Dim Filepath As String = Path.Combine(projectPath, fileName)

        Dim lijnen() As String = File.ReadAllLines(Filepath)
        Dim startnode As Track
        Dim targetnode As Track

        Dim Map(lijnen.First.Length - 1, lijnen.Length - 1) As Track
        Dim normalTracks As New Dictionary(Of (x As Integer, y As Integer), Integer)

        For y = 0 To Map.GetUpperBound(1)
            Dim lijn = lijnen(y)
            For x = 0 To Map.GetUpperBound(0)
                Dim N As New Track(x, y)
                Select Case lijn(x)
                    Case "S"
                        startnode = N
                        N.Wall = False
                        normalTracks.Add((x, y), 0)
                    Case "E"
                        targetnode = N
                        N.Wall = False
                        normalTracks.Add((x, y), 0)
                    Case "."
                        N.Wall = False
                        normalTracks.Add((x, y), 0)
                    Case "#"
                        N.Wall = True
                End Select

                Map(x, y) = N
            Next
        Next

        Dim initialTime = FindTimeAll(Map, startnode, targetnode)

        Dim InitialMap(lijnen.First.Length - 1, lijnen.Length - 1) As Track

        Array.Copy(Map, InitialMap, Map.Length)

        ResetMap(Map)

        For Each T In normalTracks.Keys
            normalTracks(T) = FindTime(Map, Map(T.x, T.y), Map(targetnode.x, targetnode.y))
            ResetMap(Map)
        Next

        Dim cheats As New Dictionary(Of String, Integer)


        For Each S In normalTracks.Keys
            Dim start = InitialMap(S.x, S.y)

            For Each E In normalTracks.Keys
                Dim afstand = Math.Abs(E.x - start.x) + Math.Abs(E.y - start.y)
                If afstand > 0 AndAlso afstand <= 20 Then
                    Dim Tijd = start.gCost + afstand + normalTracks(E)
                    Dim Saved = initialTime - Tijd

                    If Saved >= 100 Then
                        'Console.WriteLine(start.Name + " - " + InitialMap(E.x, E.y).Name + " Saved : " + Saved.ToString + " - " + start.gCost.ToString + ", " + afstand.ToString + ", " + normalTracks(E).ToString)
                        cheats.Add(start.Name + " - " + InitialMap(E.x, E.y).Name, Saved)

                    End If
                End If
            Next

        Next

        Console.WriteLine(cheats.Count)
    End Sub

    Private Function FindTimeAll(Map(,) As Track, startnode As Track, targetnode As Track) As Integer
        startnode.hCost = Heap.GetDistance(startnode, targetnode)
        Dim OpenList As New Heap()
        OpenList.Add(startnode)
        startnode.parent = startnode

        Dim currentNode As Track

        Do While OpenList.Count > 0
            currentNode = OpenList.RemoveFirst

            For Each B In GetBuren(Map, currentNode)

                If B Is Nothing OrElse B.Wall = True OrElse B.Visited Then
                    Continue For
                End If

                Dim CostToBuur As Integer = currentNode.gCost + 1

                If CostToBuur < B.gCost OrElse Not OpenList.items.Contains(B) Then

                    B.gCost = CostToBuur
                    B.parent = currentNode

                    B.hCost = Heap.GetDistance(B, targetnode)

                    ' Console.WriteLine(B.x.ToString + "," + B.y.ToString + " : H:" + B.hCost.ToString + " G:" + B.gCost.ToString + " F:" + B.fCost.ToString + " P:" + currentNode.Name + " -added")

                    If Not OpenList.items.Contains(B) Then
                        OpenList.Add(B)
                    Else
                        OpenList.Update(B)
                    End If
                Else
                    'Console.WriteLine(B.x.ToString + "," + B.y.ToString + " : H:" + B.hCost.ToString + " G:" + B.gCost.ToString + " F:" + B.fCost.ToString + " P:" + currentNode.Name + " - te groot")
                End If
            Next

            currentNode.Visited = True
        Loop

        ' Console.WriteLine(startnode.Name + " - " + targetnode.Name + " : " + targetnode.gCost.ToString)

        Return targetnode.gCost
    End Function

    Public Sub Condition1()
        Dim fileName As String = "Race.txt"
        Dim projectPath As String = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName
        Dim Filepath As String = Path.Combine(projectPath, fileName)

        Dim lijnen() As String = File.ReadAllLines(Filepath)
        Dim startnode As Track
        Dim targetnode As Track

        Dim Map(lijnen.First.Length - 1, lijnen.Length - 1) As Track
        Dim Muren As New List(Of Track)

        For y = 0 To Map.GetUpperBound(1)
            Dim lijn = lijnen(y)
            For x = 0 To Map.GetUpperBound(0)
                Dim N As New Track(x, y)
                Select Case lijn(x)
                    Case "S"
                        startnode = N
                        N.Wall = False
                    Case "E"
                        targetnode = N
                        N.Wall = False
                    Case "."
                        N.Wall = False
                    Case "#"
                        N.Wall = True
                        If N.x > 0 AndAlso N.y > 0 AndAlso N.y < Map.GetUpperBound(1) AndAlso N.x < Map.GetUpperBound(0) Then
                            Muren.Add(N)
                        End If
                End Select

                Map(x, y) = N
            Next
        Next

        Dim initialTime = FindTime(Map, startnode, targetnode)

        Dim currentNode As Track = targetnode

        Dim pad As New List(Of Track)

        While Not currentNode.Name = startnode.Name
            pad.Add(currentNode)
            currentNode = currentNode.parent
        End While

        ResetMap(Map)

        Dim aantalMinBovenInitial As Integer

        For Each W In Muren
            Map(W.x, W.y).Wall = False
            If FindTime(Map, Map(startnode.x, startnode.y), Map(targetnode.x, targetnode.y)) <= initialTime - 100 Then
                aantalMinBovenInitial += 1
            End If
            Map(W.x, W.y).Wall = True
            ResetMap(Map)
        Next

        Console.WriteLine(aantalMinBovenInitial)

    End Sub

    Private Function GetBuren(Pattern(,) As Track, N As Track) As List(Of Track)
        Dim Buren As New List(Of Track)

        If N.y > 0 Then
            Buren.Add(Pattern(N.x, N.y - 1))
        End If

        If N.y < Pattern.GetUpperBound(1) Then
            Buren.Add(Pattern(N.x, N.y + 1))
        End If

        If N.x > 0 Then
            Buren.Add(Pattern(N.x - 1, N.y))
        End If

        If N.x < Pattern.GetUpperBound(0) Then
            Buren.Add(Pattern(N.x + 1, N.y))
        End If

        Return Buren
    End Function

    Private Sub ResetMap(Map(,) As Track)
        For Each N In Map
            Dim nieuw As New Track(N.x, N.y)
            nieuw.Wall = N.Wall
            Map(N.x, N.y) = nieuw
        Next
    End Sub

    Private Function FindTime(Map(,) As Track, startnode As Track, targetnode As Track) As Integer
        startnode.hCost = Heap.GetDistance(startnode, targetnode)
        Dim OpenList As New Heap()
        OpenList.Add(startnode)
        startnode.parent = startnode

        Dim currentNode As Track = OpenList.RemoveFirst

        Do While currentNode.hCost > 0
            For Each B In GetBuren(Map, currentNode)

                If B Is Nothing OrElse B.Wall = True OrElse B.Visited Then
                    Continue For
                End If

                Dim CostToBuur As Integer = currentNode.gCost + 1

                If CostToBuur < B.gCost OrElse Not OpenList.items.Contains(B) Then

                    B.gCost = CostToBuur
                    B.parent = currentNode

                    B.hCost = Heap.GetDistance(B, targetnode)

                    ' Console.WriteLine(B.x.ToString + "," + B.y.ToString + " : H:" + B.hCost.ToString + " G:" + B.gCost.ToString + " F:" + B.fCost.ToString + " P:" + currentNode.Name + " -added")

                    If Not OpenList.items.Contains(B) Then
                        OpenList.Add(B)
                    Else
                        OpenList.Update(B)
                    End If
                Else
                    'Console.WriteLine(B.x.ToString + "," + B.y.ToString + " : H:" + B.hCost.ToString + " G:" + B.gCost.ToString + " F:" + B.fCost.ToString + " P:" + currentNode.Name + " - te groot")
                End If
            Next

            currentNode.Visited = True
            currentNode = OpenList.RemoveFirst
        Loop

        ' Console.WriteLine(startnode.Name + " - " + targetnode.Name + " : " + targetnode.gCost.ToString)

        Return targetnode.gCost
    End Function


    Private Class Track
        Inherits Node

        Public Wall As Boolean
        Public ReadOnly Property Coords As (x As Integer, y As Integer)
            Get
                Return (x, y)
            End Get
        End Property

        Public Sub New(x As Integer, y As Integer)
            MyBase.New(x, y)
        End Sub
    End Class
End Module
