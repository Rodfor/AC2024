Public Class TrieNode
    Public Children As New Dictionary(Of Char, TrieNode)
    Public WordEnd As Boolean = False

End Class

Public Class Trie
    Private Root As TrieNode

    Public Sub New()
        Root = New TrieNode()
    End Sub

    Public Sub InsertKey(ByVal key As String)
        Dim curr As TrieNode = Root

        For Each c As Char In key
            Dim value As TrieNode = Nothing
            If Not curr.Children.TryGetValue(c, value) Then
                value = New TrieNode()
                curr.Children(c) = value
            End If
            curr = value
        Next

        curr.WordEnd = True
    End Sub

    Public Function Search(ByVal key As String) As Boolean
        Dim curr As TrieNode = Root

        For Each c As Char In key
            If Not curr.Children.ContainsKey(c) Then
                Return False
            End If
            curr = curr.Children(c)
        Next

        Return curr.WordEnd
    End Function
End Class
