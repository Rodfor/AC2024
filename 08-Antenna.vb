Imports System.Data.SqlTypes
Imports System.IO
Imports System.Security.Cryptography

Module Antenna

    Public Sub Resonance()

        Dim fileName As String = "Antenna.txt"
        Dim projectPath As String = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName
        Dim pad As String = Path.Combine(projectPath, fileName)

        Dim lijnen() As String = File.ReadAllLines(pad)
        Dim Antennes As New Dictionary(Of Char, List(Of Vector2D))

        Dim UpperY = lijnen.First.Length() - 1
        Dim UpperX = lijnen.Length - 1

        For x = 0 To UpperX
            Dim lijn = lijnen(x)
            For y = 0 To UpperY
                If lijn(y) <> "." Then
                    Dim V As New Vector2D(x, y)
                    Dim Type As Char = lijn(y)
                    If Not Antennes.ContainsKey(Type) Then Antennes.Add(Type, New List(Of Vector2D))
                    Antennes(Type).Add(V)
                End If
            Next
        Next

        Dim Antinodes As New Dictionary(Of (Double, Double), Char)

        For Each T In Antennes
            Dim AntennesVanType = T.Value
            For i = 0 To AntennesVanType.Count() - 1
                For j = i + 1 To AntennesVanType.Count - 1
                    Dim offset As Vector2D = Vector2D.Subtract(AntennesVanType(i), AntennesVanType(j))
                    offset = offset.Normalize
                    Dim v1 As Vector2D = Vector2D.Add(AntennesVanType(i), offset)
                    Dim v2 As Vector2D = Vector2D.Subtract(AntennesVanType(j), offset)

                    Console.WriteLine(AntennesVanType(i).ToString + "," + AntennesVanType(j).ToString + " - " + v1.ToString + ", " + v2.ToString)

                    If Vector2D.InGrid(v1, UpperX, UpperY) Then Antinodes.TryAdd(v1.Key, T.Key)
                    If Vector2D.InGrid(v2, UpperX, UpperY) Then Antinodes.TryAdd(v2.Key, T.Key)
                Next
            Next
        Next

        Console.WriteLine(Antinodes.Count.ToString)


    End Sub



    Public Sub Resonance1()

        Dim fileName As String = "Antenna.txt"
        Dim projectPath As String = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName
        Dim pad As String = Path.Combine(projectPath, fileName)

        Dim lijnen() As String = File.ReadAllLines(pad)
        Dim Antennes As New Dictionary(Of Char, List(Of Vector2D))

        Dim UpperY = lijnen.First.Length() - 1
        Dim UpperX = lijnen.Length - 1

        For x = 0 To UpperX
            Dim lijn = lijnen(x)
            For y = 0 To UpperY
                If lijn(y) <> "." Then
                    Dim V As New Vector2D(x, y)
                    Dim Type As Char = lijn(y)
                    If Not Antennes.ContainsKey(Type) Then Antennes.Add(Type, New List(Of Vector2D))
                    Antennes(Type).Add(V)
                End If
            Next
        Next

        Dim Antinodes As New Dictionary(Of (Integer, Integer), Char)

        For Each T In Antennes
            Dim AntennesVanType = T.Value
            For i = 0 To AntennesVanType.Count() - 1
                For j = i + 1 To AntennesVanType.Count - 1
                    Dim offset As Vector2D = Vector2D.Subtract(AntennesVanType(i), AntennesVanType(j))
                    Dim v1 As Vector2D = Vector2D.Add(AntennesVanType(i), offset)
                    Dim v2 As Vector2D = Vector2D.Subtract(AntennesVanType(j), offset)

                    Console.WriteLine(AntennesVanType(i).ToString + "," + AntennesVanType(j).ToString + " - " + v1.ToString + ", " + v2.ToString)

                    If Vector2D.InGrid(v1, UpperX, UpperY) Then Antinodes.TryAdd(v1.Key, T.Key)
                    If Vector2D.InGrid(v2, UpperX, UpperY) Then Antinodes.TryAdd(v2.Key, T.Key)
                Next
            Next
        Next

        Console.WriteLine(Antinodes.Count.ToString)


    End Sub


End Module
