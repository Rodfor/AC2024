Imports System.IO

Module Bytes
    Private Map(70, 70) As MemoryNode
    Private startnode As MemoryNode
    Private targetnode As MemoryNode

    Public Sub Ren()
        Dim fileName As String = "Bytes.txt"
        Dim projectPath As String = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName
        Dim Filepath As String = Path.Combine(projectPath, fileName)
        Dim lijnen() As String = File.ReadAllLines(Filepath)

        Dim aantalCorrupted As Integer = 1024
        Dim Offset As Integer = (lijnen.Count - aantalCorrupted) / 2


        While Offset <> 0
            aantalCorrupted += Offset

            InitMap(lijnen, aantalCorrupted)

            Console.WriteLine("offset = " + Offset.ToString + " Index = " + aantalCorrupted.ToString)

            If Pathfind() Then
                Offset = Math.Abs(Offset / 2)
            Else
                Offset = -Math.Abs(Offset / 2)
            End If

        End While

        InitMap(lijnen, aantalCorrupted + 1)
        Pathfind()

        Console.WriteLine("offset = " + Offset.ToString + " Index = " + aantalCorrupted.ToString + " + 1")
        Console.WriteLine(lijnen(aantalCorrupted))

    End Sub

    Private Sub InitMap(Lijnen() As String, AantalCorrupted As Integer)

        Dim CorruptedNodes(AantalCorrupted) As String

        Array.Copy(Lijnen, CorruptedNodes, AantalCorrupted)

        For y = 0 To Map.GetUpperBound(1)
            Dim lijnstring = ""
            For x = 0 To Map.GetUpperBound(0)
                Dim N As New MemoryNode(x, y)
                If CorruptedNodes.Contains(x.ToString + "," + y.ToString) Then
                    N.Corrupted = True
                    lijnstring += "#"
                Else
                    N.Corrupted = False
                    lijnstring += "."
                End If
                Map(x, y) = N
            Next
            'Console.WriteLine(lijnstring)
        Next

        startnode = Map(0, 0)
        targetnode = Map(Map.GetUpperBound(0), Map.GetUpperBound(1))

    End Sub

    Public Sub Ren1()
        Dim fileName As String = "Bytes.txt"
        Dim projectPath As String = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName
        Dim Filepath As String = Path.Combine(projectPath, fileName)
        Dim lijnen() As String = File.ReadAllLines(Filepath)

        InitMap(lijnen, 1024)

        Pathfind()
    End Sub

    Private Function Pathfind() As Boolean

        Dim OpenList As New Heap()
        OpenList.Add(startnode)
        startnode.parent = startnode

        startnode.hCost = Heap.GetDistance(startnode, targetnode)

        Dim currentNode As MemoryNode = OpenList.RemoveFirst

        Do While currentNode.hCost > 0

            For Each B In GetBuren(Map, currentNode)

                If B Is Nothing OrElse B.Corrupted OrElse B.Visited Then
                    Continue For
                End If

                Dim CostToBuur As Integer = currentNode.gCost + 1

                If CostToBuur < B.gCost OrElse Not OpenList.items.Contains(B) Then

                    B.gCost = CostToBuur
                    B.parent = currentNode
                    B.hCost = Heap.GetDistance(B, targetnode)

                    'Console.WriteLine(B.x.ToString + "," + B.y.ToString + "," + B.dir.ToString + " : H:" + B.hCost.ToString + " G:" + B.gCost.ToString + " F:" + B.fCost.ToString + " P:" + currentNode.Name + ", " + currentNode.dir.ToString + " -added")

                    If Not OpenList.items.Contains(B) Then
                        OpenList.Add(B)
                    Else
                        OpenList.Update(B)
                    End If
                Else
                    ' Console.WriteLine(B.x.ToString + "," + B.y.ToString + "," + B.dir.ToString + " : H:" + B.hCost.ToString + " G:" + B.gCost.ToString + " F:" + B.fCost.ToString + " P:" + currentNode.Name + ", " + currentNode.dir.ToString + " - te groot")
                End If
            Next

            currentNode.Visited = True

            If OpenList.Count = 0 Then

                Console.WriteLine("Geen pad gevonden")
                Return False
            End If
            currentNode = OpenList.RemoveFirst

        Loop

        Console.WriteLine(currentNode.gCost)

        Return True
    End Function

    Private Function GetBuren(Pattern(,) As MemoryNode, N As MemoryNode) As List(Of MemoryNode)
        Dim Buren As New List(Of MemoryNode)

        If N.y > 0 Then
            Buren.Add(Pattern(N.x, N.y - 1))
        End If


        If N.x > 0 Then
            Buren.Add(Pattern(N.x - 1, N.y))
        End If


        If N.y < Pattern.GetUpperBound(1) Then
            Buren.Add(Pattern(N.x, N.y + 1))
        End If


        If N.x < Pattern.GetUpperBound(0) Then
            Buren.Add(Pattern(N.x + 1, N.y))
        End If

        Return Buren
    End Function


    Private Class MemoryNode
        Inherits Node

        Public Corrupted As Boolean

        Public Sub New(x As Integer, y As Integer)
            MyBase.New(x, y)
        End Sub
    End Class
End Module
