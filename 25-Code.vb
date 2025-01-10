Imports System.IO
Imports System.Threading

Module Code
    Public Sub Unlock()
        Dim fileName As String = "Code.txt"
        Dim projectPath As String = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName
        Dim Filepath As String = Path.Combine(projectPath, fileName)

        Dim Lijnen = File.ReadAllLines(Filepath)
        Dim Locks As New List(Of List(Of Integer))
        Dim Keys As New List(Of List(Of Integer))

        Dim newPattern As Boolean = True
        Dim Teken = Nothing
        Dim currentPattern As New List(Of Integer)

        For Each L In Lijnen
            If L = "" Then
                newPattern = True
                If Teken = "#" Then
                    Locks.Add(currentPattern)
                Else
                    Keys.Add(currentPattern)
                End If
                Continue For
            End If

            If newPattern Then
                currentPattern = New List(Of Integer)
                Teken = L.First
                currentPattern.AddRange(Enumerable.Repeat(-1, L.Length).ToArray())
                newPattern = False
            End If

            For i = 0 To L.Count - 1
                If L(i) = "#" Then currentPattern(i) += 1
            Next
        Next

        If Teken = "#" Then
            Locks.Add(currentPattern)
        Else
            Keys.Add(currentPattern)
        End If

        Dim count As Integer = Locks.Count * Keys.Count

        For Each L In Locks
            For Each k In Keys
                For i = 0 To k.Count - 1
                    If L(i) + k(i) > 5 Then
                        count -= 1
                        Exit For
                    End If
                Next
            Next
        Next

        Console.WriteLine(count)

    End Sub




End Module
