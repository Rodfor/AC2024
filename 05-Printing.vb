Imports System.Data
Imports System.IO
Imports System.Net.Security
Imports System.Text
Imports System.Text.Json
Imports System.Xml.XPath
Imports Microsoft.VisualBasic.FileIO


Module Printing
    Private rules As New Dictionary(Of Integer, List(Of Integer))

    Public Sub Print()
        Dim fileName As String = "Printing.txt"
        Dim projectPath As String = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName
        Dim pad As String = Path.Combine(projectPath, fileName)

        Dim lijnen As New List(Of String())

        Dim updates As New List(Of String())

        Using parser = New TextFieldParser(pad)
            parser.TextFieldType = FieldType.Delimited
            parser.SetDelimiters("|", ",")

            While Not parser.EndOfData
                Dim fields As String() = parser.ReadFields()
                lijnen.Add(fields)
            End While

            parser.Close()
            parser.Dispose()
        End Using

        Dim EndOfRules As Boolean = False

        For Each L In lijnen
            If L.Length > 2 Then
                EndOfRules = True
            End If

            If Not EndOfRules Then
                rules.TryAdd(L(0), New List(Of Integer))
                rules(L(0)).Add(L(1))
            Else
                updates.Add(L)
            End If
        Next


        Console.WriteLine(FixUpdate(updates, True))

    End Sub

    Private Function FixUpdate(Updates As List(Of String()), first As Boolean) As Integer
        Dim total As Integer = 0
        Dim IncorrectUpdates As New List(Of String())

        For Each U In Updates
            Dim correct As Boolean = True
            Dim i = U.Length - 1
            While i > 0
                Dim j = i - 1
                While j >= 0
                    Dim values As List(Of Integer)
                    If rules.TryGetValue(U(i), values) Then
                        If values.Contains(U(j)) Then
                            correct = False
                            Dim newarray(U.Length - 1) As String
                            U.CopyTo(newarray, 0)
                            newarray(i) = U(j)
                            newarray(j) = U(i)
                            IncorrectUpdates.Add(newarray)
                            Exit While
                        End If
                    End If
                    j -= 1
                End While
                If Not correct Then Exit While
                i -= 1
            End While

            If Not first AndAlso correct Then total += U((U.Length - 1) / 2)
        Next

        Console.WriteLine("incorrect - " + IncorrectUpdates.Count.ToString)

        If IncorrectUpdates.Count > 0 Then total += FixUpdate(IncorrectUpdates, False)

        Return total
    End Function


    Public Sub Print1()
        Dim fileName As String = "Printing.txt"
        Dim projectPath As String = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName
        Dim pad As String = Path.Combine(projectPath, fileName)

        Dim lijnen As New List(Of String())
        Dim rules As New Dictionary(Of Integer, List(Of Integer))
        Dim updates As New List(Of String())

        Using parser = New TextFieldParser(pad)
            parser.TextFieldType = FieldType.Delimited
            parser.SetDelimiters("|", ",")

            While Not parser.EndOfData
                Dim fields As String() = parser.ReadFields()
                lijnen.Add(fields)
            End While

            parser.Close()
            parser.Dispose()
        End Using

        Dim EndOfRules As Boolean = False

        For Each L In lijnen
            If L.Length > 2 Then
                EndOfRules = True
            End If

            If Not EndOfRules Then
                rules.TryAdd(L(0), New List(Of Integer))
                rules(L(0)).Add(L(1))
            Else
                updates.Add(L)
            End If
        Next

        Dim total As Integer = 0

        For Each U In updates
            Dim correct As Boolean = True
            Dim i = U.Length - 1
            While i > 0
                Dim j = i - 1
                While j >= 0
                    Dim values As List(Of Integer)
                    If rules.TryGetValue(U(i), values) Then
                        If values.Contains(U(j)) Then
                            correct = False
                            Console.WriteLine(U(i) + "|" + U(j) + " - " + String.Join(",", U))
                            Exit While
                        End If
                    End If
                    j -= 1
                End While
                If Not correct Then Exit While
                i -= 1
            End While

            If correct Then total += U((U.Length - 1) / 2)
        Next

        Console.WriteLine(total)

    End Sub


End Module
