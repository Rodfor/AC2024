Imports System.IO
Imports System.Security.Cryptography
Imports System.Xml.Schema

Module Olympics

    Public Sub Race()
        Dim fileName As String = "Olympics.txt"
        Dim projectPath As String = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName
        Dim Filepath As String = Path.Combine(projectPath, fileName)

        Dim lijnen() As String = File.ReadAllLines(Filepath)
        Dim Map(lijnen.First.Length - 1, lijnen.Length - 1, 3) As Plaats
        Dim startnode As Plaats
        Dim targetnodes As New List(Of Plaats)

        For y = 0 To Map.GetUpperBound(1)
            Dim lijn = lijnen(y)
            For x = 0 To Map.GetUpperBound(0)
                Dim N As New Plaats(x, y, lijn(x))
                N.dir = Direction.Up
                Map(x, y, N.dir) = N
                If N.Waarde = "E" Then targetnodes.Add(N)

                N = New Plaats(x, y, lijn(x))
                N.dir = Direction.Down
                Map(x, y, N.dir) = N
                If N.Waarde = "E" Then targetnodes.Add(N)

                N = New Plaats(x, y, lijn(x))
                N.dir = Direction.Left
                Map(x, y, N.dir) = N
                If N.Waarde = "E" Then targetnodes.Add(N)

                N = New Plaats(x, y, lijn(x))
                N.dir = Direction.Right
                Map(x, y, N.dir) = N
                If N.Waarde = "S" Then startnode = N
                If N.Waarde = "E" Then targetnodes.Add(N)
            Next
        Next

        startnode.hCost = Heap.GetDistance(startnode, targetnode)
        startnode.dir = Direction.Right

        Dim OpenList As New Heap()
        OpenList.Add(startnode)
        startnode.parent = startnode

        Dim currentNode As Plaats = OpenList.RemoveFirst

        Do While currentNode.hCost > 0
            '  Console.WriteLine(currentNode.x.ToString + "," + currentNode.y.ToString + " : H:" + currentNode.hCost.ToString + " G:" + currentNode.gCost.ToString + " F:" + currentNode.fCost.ToString)

            For Each KVP In GetBuren(Map, currentNode)
                Dim B As Plaats = KVP.Key
                Dim dirB As Direction = KVP.Value

                If B.Waarde = "#" OrElse currentNode.Visited Then
                    Continue For
                End If

                Dim CostToBuur As Integer = currentNode.gCost

                If dirB <> currentNode.dir Then
                    CostToBuur += 1001
                Else
                    CostToBuur += 1
                End If

                If CostToBuur < B.gCost OrElse Not OpenList.items.Contains(B) Then

                    B.gCost = CostToBuur
                    B.hCost = Heap.GetDistance(B, targetnode)
                    B.parent = currentNode
                    B.dir = dirB

                    'Console.WriteLine(B.x.ToString + "," + B.y.ToString + " : H:" + B.hCost.ToString + " G:" + B.gCost.ToString + " F:" + B.fCost.ToString + " P:" + currentNode.Name + " -added")


                    If Not OpenList.items.Contains(B) Then
                        OpenList.Add(B)
                    Else
                        OpenList.Update(B)
                    End If
                End If
            Next
            currentNode.Visited = True
            currentNode = OpenList.RemoveFirst

            'If currentNode.hCost = 0 AndAlso OpenList.Count > 0 Then
            '    Dim endnode = currentNode
            '    currentNode = OpenList.RemoveFirst
            '    OpenList.Add(endnode)
            'End If
        Loop

        For y = 0 To Map.GetUpperBound(1)
            Dim lijn As String = ""
            For x = 0 To Map.GetUpperBound(0)
                lijn += Map(x, y).Waarde
            Next
            Console.WriteLine(lijn)
        Next

        Console.WriteLine("Deel1 = " + targetnode.gCost.ToString)


    End Sub
    Private Function GetBuren(Pattern(,) As Plaats, N As Plaats) As Dictionary(Of Plaats, Direction)
        Dim Buren As New Dictionary(Of Plaats, Direction)

        If N.x = 5 AndAlso N.y = 13 Then
            Console.Write("x")
        End If

        If N.dir <> Direction.Down AndAlso N.y > 0 Then
            Buren.Add(Pattern(N.x, N.y - 1), Direction.Up)
        End If

        If N.dir <> Direction.Left AndAlso N.x < Pattern.GetUpperBound(0) Then
            Buren.Add(Pattern(N.x + 1, N.y), Direction.Right)
        End If
        If N.dir <> Direction.Up AndAlso N.y < Pattern.GetUpperBound(1) Then
            Buren.Add(Pattern(N.x, N.y + 1), Direction.Down)
        End If
        If N.dir <> Direction.Right AndAlso N.x > 0 Then
            Buren.Add(Pattern(N.x - 1, N.y), Direction.Left)
        End If

        Return Buren
    End Function

    Friend Enum Direction
        Up
        Right
        Down
        Left
    End Enum

    Private Class Plaats
        Inherits Node

        Public dir As Direction
        Public Parents As New List(Of Plaats)

        Public Sub New(x As Integer, y As Integer, waarde As Char)
            MyBase.New(x, y, waarde)
        End Sub
    End Class
End Module
