Imports System.IO

Module Towels
    Private towels As New List(Of String)

    Public Sub Organize()
        Dim fileName As String = "Towels.txt"
        Dim projectPath As String = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName
        Dim pad As String = Path.Combine(projectPath, fileName)

        Dim lijnen() As String = File.ReadAllLines(pad)
        Dim Patterns As New List(Of String)

        Dim MAXS As Integer = 0

        For Each L In lijnen
            If L.Split(",").Count > 1 Then
                For Each T In L.Split(", ")
                    towels.Add(T)
                    MAXS += T.Length
                Next
            ElseIf L <> "" Then
                Patterns.Add(L.Trim(" "))
            End If
        Next

        Dim correct As Integer = 0

        InitACMatching(26, 1000, towels.ToArray)

        For Each P In Patterns
            searchWords(P)
            If MatchAll(P, 0, New HashSet(Of (Integer, Integer))) Then
                Console.WriteLine(P + " - correct")
                correct += 1
            Else
                Console.WriteLine(P + " - fout")
            End If
        Next

        Console.WriteLine(correct)
    End Sub


    Private Function FindOptions(Current As String, patroon As String) As List(Of String)
        Dim newoptions As New List(Of String)

        For Each T In towels
            Dim newstring = Current + T
            If newstring.Length <= patroon.Length AndAlso newstring = patroon.Substring(0, newstring.Length) Then
                newoptions.Add(newstring)
                'Console.WriteLine(patroon + " -> " + newstring + " - correct")
            Else
                'Console.WriteLine(patroon + " -> " + newstring + " - fout")
            End If
        Next

        Return newoptions
    End Function

    Private Class ACNode
        Public Child
    End Class
End Module
