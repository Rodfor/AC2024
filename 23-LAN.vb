Imports System.IO

Module LAN
    Public Sub Connect()
        Dim fileName As String = "LAN.txt"
        Dim projectPath As String = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName
        Dim Filepath As String = Path.Combine(projectPath, fileName)

        Dim Connections = File.ReadAllLines(Filepath).ToList()

        Dim PCs As New Dictionary(Of String, PC)

        For Each C In Connections
            Dim CSplit = C.Split("-")

            PCs.TryAdd(CSplit(0), New PC(CSplit(0)))
            PCs.TryAdd(CSplit(1), New PC(CSplit(1)))

            PCs(CSplit(0)).Connections.Add(PCs(CSplit(1)))
            PCs(CSplit(1)).Connections.Add(PCs(CSplit(0)))
        Next

        Dim R As New List(Of PC)
        Dim X As New List(Of PC)

        Dim cliques = BronKerbosch(R, PCs.Values.ToList, X)

        Dim MaxCLiques As New List(Of PC)

        For Each C In cliques
            Dim valid = False
            For Each PC In C
                If PC.Name.StartsWith("t") Then
                    valid = True
                    Exit For
                End If
            Next

            If valid Then
                If MaxCLiques Is Nothing OrElse MaxCLiques.Count < C.Count Then
                    MaxCLiques = C
                End If
            End If
        Next

        Dim Grootste As New List(Of String)

        For Each N In MaxCLiques
            Grootste.Add(N.Name)
        Next

        Grootste.Sort()

        Console.WriteLine(String.Join(",", Grootste))

    End Sub

    Private Function BronKerbosch(R As List(Of PC), P As List(Of PC), X As List(Of PC)) As List(Of List(Of PC))
        Dim Cliques As New List(Of List(Of PC))
        If P.Count = 0 AndAlso X.Count = 0 Then
            'For Each N In R
            '    Console.Write(N.Name + ",")
            'Next
            'Console.WriteLine()
            Cliques.Add(R)
        Else
            For Each V In P.ToArray()
                Dim newR As New List(Of PC)
                newR.AddRange(R)
                newR.Add(V)

                Cliques.AddRange(BronKerbosch(newR, P.Where(Function(y) V.Connections.Contains(y)).ToList, X.Where(Function(y) V.Connections.Contains(y)).ToList))

                P.Remove(V)
                X.Add(V)
            Next
        End If

        Return Cliques
    End Function

    Public Sub Connect1()
        Dim fileName As String = "LAN.txt"
        Dim projectPath As String = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName
        Dim Filepath As String = Path.Combine(projectPath, fileName)

        Dim Connections = File.ReadAllLines(Filepath).ToList()
        Dim Nodes As New Dictionary(Of String, List(Of String))

        For Each C In Connections
            Dim CSplit = C.Split("-")
            Nodes.TryAdd(CSplit(0), New List(Of String))
            Nodes.TryAdd(CSplit(1), New List(Of String))

            Nodes(CSplit(0)).Add(CSplit(1))
            Nodes(CSplit(1)).Add(CSplit(0))

        Next

        Console.WriteLine("Deel 1 : " + Solve1(Nodes).ToString)

    End Sub

    Private Function Solve1(Nodes As Dictionary(Of String, List(Of String))) As Integer
        Dim Sets As New HashSet(Of (String, String, String))

        For Each N In Nodes
            If N.Key.StartsWith("t") Then
                For Each L In N.Value
                    For Each P In Nodes(L)
                        If N.Value.Contains(P) Then
                            Dim T As New List(Of String) From {
                                N.Key, P, L
                            }
                            T.Sort()
                            If Not Sets.Contains((T(0), T(1), T(2))) Then
                                Sets.Add((T(0), T(1), T(2)))
                            End If
                        End If
                    Next
                Next
            End If
        Next

        Return Sets.Count

    End Function

    Private Class PC
        Public Name As String
        Public Connections As New List(Of PC)

        Public Sub New(Naam As String)
            Name = Naam
        End Sub
    End Class


End Module
