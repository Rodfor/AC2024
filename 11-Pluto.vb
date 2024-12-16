Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Threading
Imports Microsoft.VisualBasic.FileIO


Public Module Pluto

    Dim MemoTable As New Dictionary(Of Long, (Long, Long))

    Public Sub StenenTellen()
        Dim fileName As String = "Pluto.txt"
        Dim projectPath As String = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName
        Dim pad As String = Path.Combine(projectPath, fileName)

        Dim Stenen As New Dictionary(Of Long, Long)

        Using parser = New TextFieldParser(pad)
            parser.SetDelimiters(" ")

            While Not parser.EndOfData
                Dim fields As String() = parser.ReadFields()
                For Each F In fields
                    Stenen.Add(F, 1)
                Next
            End While

            parser.Close()
            parser.Dispose()
        End Using


        MemoTable.Add(0, (1, -1))

        Console.WriteLine(BlinkSneller(Stenen, 75).ToString)


    End Sub

    Private Function BlinkSneller(Stenen As Dictionary(Of Long, Long), Blinks As Integer) As Long
        Dim stopwatch As Stopwatch = Stopwatch.StartNew()

        For i = 1 To Blinks
            Dim NieuweStenen As New Dictionary(Of Long, Long)

            For Each KVP In Stenen
                Dim steen = KVP.Key
                Dim value As (Long, Long)
                If MemoTable.TryGetValue(steen, value) Then
                    If value.Item2 <> -1 Then
                        If Not NieuweStenen.TryAdd(value.Item2, KVP.Value) Then
                            NieuweStenen(value.Item2) += KVP.Value
                        End If
                    End If
                    If Not NieuweStenen.TryAdd(value.Item1, KVP.Value) Then
                        NieuweStenen(value.Item1) += KVP.Value
                    End If
                Else
                    Dim str = CStr(steen)
                    Dim lengte = str.Length
                    If lengte Mod 2 = 0 Then
                        Dim midden = lengte / 2
                        Dim deel1 As Long = str.Substring(0, midden)
                        Dim deel2 As Long = str.Substring(midden, midden)
                        MemoTable.Add(steen, (deel1, deel2))
                        If Not NieuweStenen.TryAdd(deel1, KVP.Value) Then
                            NieuweStenen(deel1) += KVP.Value
                        End If
                        If Not NieuweStenen.TryAdd(deel2, KVP.Value) Then
                            NieuweStenen(deel2) += KVP.Value
                        End If
                    Else
                        Dim nieuwnr = steen * 2024
                        MemoTable.Add(steen, (nieuwnr, -1))
                        If Not NieuweStenen.TryAdd(nieuwnr, KVP.Value) Then
                            NieuweStenen(nieuwnr) += KVP.Value
                        End If
                    End If
                End If
            Next

            Stenen = NieuweStenen
        Next
        Dim totaal As Long = 0

        For Each N In Stenen.Values
            totaal += N
        Next
        stopwatch.Stop()

        Console.WriteLine("In " + stopwatch.ElapsedMilliseconds.ToString + "ms")
        Return totaal
    End Function


    Private Function Blink(Stenen As List(Of String), Blinks As Integer) As Integer

        For i = 1 To Blinks
            Dim NieuweStenen As New List(Of String)

            For Each S In Stenen
                Dim nr As Long = Convert.ToInt64(S)
                If nr = 0 Then
                    NieuweStenen.Add("1")
                ElseIf S.Length Mod 2 = 0 Then
                    Dim midden As Integer = S.Length / 2
                    NieuweStenen.Add(Convert.ToInt64(S.Substring(0, midden)))
                    NieuweStenen.Add(Convert.ToInt64(S.Substring(midden, midden)))
                Else
                    NieuweStenen.Add(nr * 2024)
                End If
            Next

            If i < 10 Then
                For Each N In NieuweStenen
                    Console.Write(N + " ")
                Next

            End If

            Stenen = NieuweStenen
            Console.WriteLine(i.ToString + " Blink(s) - " + Stenen.Count.ToString)
        Next

        Return Stenen.Count
    End Function
End Module
