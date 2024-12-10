Imports System.IO
Imports Microsoft.VisualBasic.FileIO

Module Bridge

    Public Sub Repair()
        Dim fileName As String = "Bridge.txt"
        Dim projectPath As String = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName
        Dim pad As String = Path.Combine(projectPath, fileName)

        Dim lijnen As New List(Of String())

        Dim updates As New List(Of String())

        Using parser = New TextFieldParser(pad)
            parser.TextFieldType = FieldType.Delimited
            parser.SetDelimiters(": ", " ")

            While Not parser.EndOfData
                Dim fields As String() = parser.ReadFields()
                lijnen.Add(fields)
            End While

            parser.Close()
            parser.Dispose()
        End Using

        Dim Tests As New List(Of Test)

        For Each L In lijnen
            Dim T As New Test
            T.Totaal = L(0)
            T.Waardes = New List(Of Integer)

            For i = 1 To L.Length - 1
                T.Waardes.Add(L(i))
            Next

            Tests.Add(T)
        Next

        Dim totaal As Long = 0

        For Each T In Tests
            If T.IsCorrect Then totaal += T.Totaal

        Next

        Console.WriteLine(totaal.ToString)

    End Sub

    'Private Function Iterate(index As Short, currentWaarde As Long, waardes As List(Of Integer)) As String

    '    If index = -1 Then
    '        If currentWaarde = 0 Then Return "END" Else Return ""
    '    End If

    '    If currentWaarde - waardes(index) >= 0 Then
    '        Dim str = Iterate(index - 1, currentWaarde - waardes(index), waardes)
    '        If str.Length > 0 Then
    '            Return str + "+" + waardes(index).ToString
    '        End If
    '    End If

    '    If currentWaarde > 0 AndAlso currentWaarde Mod waardes(index) = 0 Then
    '        Dim str = Iterate(index - 1, currentWaarde / waardes(index), waardes)
    '        If str.Length > 0 Then
    '            Return str + "*" + waardes(index).ToString
    '        End If
    '    End If

    '    If index > 0 AndAlso currentWaarde.ToString.EndsWith(waardes(index).ToString) Then
    '        Dim str = Iterate(index - 1, currentWaarde.ToString.Substring(0, currentWaarde.ToString.Length() - waardes(index).ToString.Length))

    '    End If

    '    Return ""

    'End Function


    Private Class Test
        Public Totaal As Long
        Public Waardes As List(Of Integer)

        Public Function IsCorrect() As Boolean
            Dim str = iterate(Waardes.Count - 1, Totaal)
            Console.WriteLine(Totaal.ToString + " - " + str)
            Return str.Length > 0
        End Function

        Public Function iterate(index As Short, currentWaarde As Long) As String

            If index = 0 Then
                If currentWaarde = Waardes(index) Then
                    Return "END+" + Waardes(index).ToString
                Else
                    Return ""
                End If
            End If

            If currentWaarde - Waardes(index) >= 0 Then
                Dim str = iterate(index - 1, currentWaarde - Waardes(index))
                If str.Length > 0 Then
                    Return str + "+" + Waardes(index).ToString
                End If
            End If

            If currentWaarde > 0 AndAlso currentWaarde Mod Waardes(index) = 0 Then
                Dim str = iterate(index - 1, currentWaarde / Waardes(index))
                If str.Length > 0 Then
                    Return str + "*" + Waardes(index).ToString
                End If
            End If

            If currentWaarde.ToString.EndsWith(Waardes(index).ToString) Then
                If currentWaarde = Waardes(index) Then Return ""
                Dim str = iterate(index - 1, currentWaarde.ToString.Substring(0, currentWaarde.ToString.Length() - Waardes(index).ToString.Length))
                If str.Length > 0 Then
                    Return str + "||" + Waardes(index).ToString
                End If
            End If

            Return ""

        End Function


    End Class


End Module
