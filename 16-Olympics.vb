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
                Dim N As Plaats

                N = New Plaats(x, y, lijn(x), Direction.Right)
                Map(x, y, Direction.Right) = N
                If N.Waarde = "S" Then startnode = N
                If N.Waarde = "E" Then targetnodes.Add(N)

                N = New Plaats(x, y, lijn(x), Direction.Up)
                Map(x, y, Direction.Up) = N
                If N.Waarde = "E" Then targetnodes.Add(N)

                N = New Plaats(x, y, lijn(x), Direction.Left)
                Map(x, y, Direction.Left) = N
                If N.Waarde = "E" Then targetnodes.Add(N)

                N = New Plaats(x, y, lijn(x), Direction.Down)
                Map(x, y, Direction.Down) = N
                If N.Waarde = "E" Then targetnodes.Add(N)

            Next
        Next

        startnode.hCost = Heap.GetDistance(startnode, targetnodes.First)

        Dim OpenList As New Heap()
        OpenList.Add(startnode)
        startnode.parent = startnode

        ' Dim currentNode As Plaats = OpenList.RemoveFirst
        Dim currentNode As Plaats = startnode

        Do While OpenList.Count > 0
            currentNode = OpenList.RemoveFirst

            For Each B In GetBuren(Map, currentNode)

                If B Is Nothing OrElse B.Waarde = "#" OrElse currentNode.Parents.Contains(B) Then
                    Continue For
                End If

                Dim CostToBuur As Integer = currentNode.gCost

                If B.dir <> currentNode.dir Then
                    CostToBuur += 1000
                Else
                    CostToBuur += 1
                End If

                If CostToBuur <= B.gCost OrElse (Not B.Visited AndAlso Not OpenList.items.Contains(B)) Then

                    If CostToBuur = B.gCost Then
                        B.Parents.Add(currentNode)
                        ' Console.WriteLine(B.x.ToString + "," + B.y.ToString + "," + B.dir.ToString + " : H:" + B.hCost.ToString + " G:" + B.gCost.ToString + " F:" + B.fCost.ToString + " P:" + currentNode.Name + ", " + currentNode.dir.ToString + " -added gelijk")
                        Continue For
                    End If

                    B.gCost = CostToBuur
                    B.parent = currentNode
                    If B.Parents.Count > 0 Then
                        ' Console.WriteLine(B.x.ToString + "," + B.y.ToString + "," + B.dir.ToString + "Parents cleared - " + B.Parents.First.Name + ", " + B.Parents.First.dir.ToString + " G: " + B.Parents.First.gCost.ToString)
                        B.Parents.Clear()
                    End If
                    B.Parents.Add(currentNode)
                    B.hCost = Heap.GetDistance(B, targetnodes.First)

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
        Loop

        Dim smallest As New List(Of Plaats)
        Dim pad As New List(Of Plaats)

        For Each targetnode In targetnodes
            If smallest.Count = 0 Then
                smallest.Add(targetnode)
                Console.WriteLine("Deel1 = " + targetnode.gCost.ToString)
                Continue For
            Else
                If targetnode.gCost = smallest.First.gCost Then
                    smallest.Add(targetnode)
                ElseIf targetnode.gCost < smallest.First.gCost Then
                    smallest.Clear()
                    smallest.Add(targetnode)
                End If
            End If
            Console.WriteLine("Deel1 = " + targetnode.gCost.ToString)
        Next


        For Each F In smallest
            GetParentsRecursive(pad, F)
        Next

        Dim aantal = 1

        For y = 0 To Map.GetUpperBound(1)
            Dim lijn As String = ""
            For x = 0 To Map.GetUpperBound(0)
                Dim row = y
                Dim col = x
                If pad.Any(Function(P) P.x = col AndAlso P.y = row) Then
                    lijn += "X"
                    aantal += 1
                Else
                    lijn += Map(x, y, Direction.Right).Waarde
                End If
            Next
            Console.WriteLine(lijn)
        Next


        For Each targetnode In smallest
            Console.WriteLine("Deel1 = " + targetnode.gCost.ToString)
        Next

        Console.WriteLine("Deel2 = " + aantal.ToString)


    End Sub
    Private Function GetBuren(Pattern(,,) As Plaats, N As Plaats) As List(Of Plaats)
        Dim Buren As New List(Of Plaats)

        If N.dir <> Direction.Down AndAlso N.y > 0 Then
            If N.dir = Direction.Up Then
                Buren.Add(Pattern(N.x, N.y - 1, Direction.Up))
            Else
                Buren.Add(Pattern(N.x, N.y, Direction.Up))
            End If
        End If

        If N.dir <> Direction.Left AndAlso N.x < Pattern.GetUpperBound(0) Then
            If N.dir = Direction.Right Then
                Buren.Add(Pattern(N.x + 1, N.y, Direction.Right))
            Else
                Buren.Add(Pattern(N.x, N.y, Direction.Right))
            End If
        End If
        If N.dir <> Direction.Up AndAlso N.y < Pattern.GetUpperBound(1) Then
            If N.dir = Direction.Down Then
                Buren.Add(Pattern(N.x, N.y + 1, Direction.Down))
            Else
                Buren.Add(Pattern(N.x, N.y, Direction.Down))
            End If
        End If
        If N.dir <> Direction.Right AndAlso N.x > 0 Then
            If N.dir = Direction.Left Then
                Buren.Add(Pattern(N.x - 1, N.y, Direction.Left))
            Else
                Buren.Add(Pattern(N.x, N.y, Direction.Left))
            End If
        End If

        Return Buren
    End Function

    Private Sub GetParentsRecursive(pad As List(Of Plaats), N As Plaats)
        For Each P In N.Parents
            If Not pad.Contains(P) Then
                pad.Add(P)
                GetParentsRecursive(pad, P)
            End If
        Next
    End Sub

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
        Public Waarde As Char

        Public Sub New(x As Integer, y As Integer, waarde As Char, richting As Direction)
            MyBase.New(x, y)
            Me.Waarde = waarde
            Me.dir = richting
        End Sub
    End Class
End Module
