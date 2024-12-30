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

        Dim correct As Integer
        Dim totaalCorrect As Long
        Dim Trie As New Trie

        For Each T In towels
            Trie.InsertKey(T)
        Next


        For Each P In Patterns
            Dim n = P.Length
            Dim dp(n) As Long
            dp(0) = 1

            For i = 1 To n
                For j = 0 To i - 1
                    If dp(j) > 0 AndAlso Trie.Search(P.Substring(j, i - j)) Then
                        dp(i) += dp(j)
                    End If
                Next
            Next

            If dp(n) > 0 Then
                Console.WriteLine(P + " - correct - " + dp(n).ToString)
                correct += 1
                totaalCorrect += dp(n)
            Else
                Console.WriteLine(P + " - fout")
            End If
        Next


        Console.WriteLine("Deel1: " + correct.ToString)
        Console.WriteLine("Deel2: " + totaalCorrect.ToString)

        'InitACMatching(26, 1000, towels.ToArray)

        'For Each P In Patterns
        '    searchWords(P)
        '    If MatchAll(P, 0, New HashSet(Of (Integer, Integer))) Then
        '        Console.WriteLine(P + " - correct")
        '        correct += 1
        '    Else
        '        Console.WriteLine(P + " - fout")
        '    End If
        'Next

    End Sub

End Module
