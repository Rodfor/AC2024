Imports Microsoft.VisualBasic.FileIO

Module Rendier

    Public Sub Reports()
        Dim pad As String = "C:\Users\mle.SERVER\source\repos\AC2024\Rendier.txt"
        Dim lijnen As New List(Of String())

        Using parser = New TextFieldParser(pad)
            parser.SetDelimiters(" ")
            While Not parser.EndOfData
                Dim fields As String() = parser.ReadFields()
                lijnen.Add(fields)
            End While

            parser.Close()
            parser.Dispose()
        End Using

        Dim AantalSave As Integer = 0
        Dim indexCount As Int32 = 1

        Dim UnsafeLijnen As New Dictionary(Of String(), Short)

        For Each L In lijnen
            Dim FouteIndex = IsSafe(L)
            If FouteIndex >= 0 Then
                UnsafeLijnen.Add(L, FouteIndex)
            Else
                Console.WriteLine(indexCount.ToString + "  -  Safe")
                AantalSave += 1
            End If

            indexCount += 1
        Next

        For Each D In UnsafeLijnen
            Dim safe As Boolean = False

            If IsSafe(D.Key.Where(Function(value, index) index <> D.Value).ToArray()) < 0 Then
                safe = True
            End If

            If D.Value - 1 >= 0 Then
                If IsSafe(D.Key.Where(Function(value, index) index <> D.Value - 1).ToArray()) < 0 Then
                    safe = True
                End If
            End If
            If D.Value + 1 < D.Key.Length Then
                If IsSafe(D.Key.Where(Function(value, index) index <> D.Value + 1).ToArray()) < 0 Then
                    safe = True
                End If
            End If

            If safe Then
                AantalSave += 1
                Console.WriteLine("Recheck -  Safe")
            Else
                Console.WriteLine("Recheck -  slecht")
            End If
        Next


        Console.WriteLine(AantalSave)

    End Sub

    Private Function IsSafe(L As String()) As Short
        Dim decrease As Boolean = CInt(L(0)) > CInt(L(1))

        For i = 0 To L.Length - 2
            If decrease Then
                If CInt(L(i)) <= CInt(L(i + 1)) Then
                    Return i
                    Exit For
                End If
            Else
                If CInt(L(i)) >= CInt(L(i + 1)) Then
                    Return i
                End If
            End If

            Dim verschil = Math.Abs(CInt(L(i)) - CInt(L(i + 1)))

            If verschil < 1 OrElse verschil > 3 Then
                Return i
            End If
        Next

        Return -1
    End Function

End Module
