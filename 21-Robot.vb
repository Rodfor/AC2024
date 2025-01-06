Imports System.IO
Imports System.Reflection.Metadata.Ecma335
Imports System.Runtime.InteropServices
Imports System.Security.Cryptography.X509Certificates
Imports System.Windows

Module Robot
    Private NumPad As New Dictionary(Of Char, (x As Integer, y As Integer))
    Private Keypad As New Dictionary(Of Char, (x As Integer, y As Integer))

    Private Cache As New Dictionary(Of (c1 As Char, c2 As Char, diepte As Integer), Long)

    Public Sub Type()
        Dim fileName As String = "Robot.txt"
        Dim projectPath As String = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName
        Dim Filepath As String = Path.Combine(projectPath, fileName)

        Dim Codes = File.ReadAllLines(Filepath).ToList()

        NumPad.Add("7", (0, 0))
        NumPad.Add("8", (1, 0))
        NumPad.Add("9", (2, 0))
        NumPad.Add("4", (0, 1))
        NumPad.Add("5", (1, 1))
        NumPad.Add("6", (2, 1))
        NumPad.Add("1", (0, 2))
        NumPad.Add("2", (1, 2))
        NumPad.Add("3", (2, 2))
        NumPad.Add("X", (0, 3))
        NumPad.Add("0", (1, 3))
        NumPad.Add("A", (2, 3))

        Keypad.Add("X", (0, 0))
        Keypad.Add("^", (1, 0))
        Keypad.Add("A", (2, 0))
        Keypad.Add("<", (0, 1))
        Keypad.Add("v", (1, 1))
        Keypad.Add(">", (2, 1))

        Console.WriteLine("Deel 1 : " + Solve(Codes, 3).ToString)
        Console.WriteLine("Deel 2 : " + Solve(Codes, 26).ToString)

    End Sub

    Private Function Solve(Codes As List(Of String), robots As Integer) As Long
        Dim totaal As Long = 0

        For Each C In Codes
            '  Console.WriteLine(C + ":")

            Dim Inputs As New List(Of String) From {
                ""
            }

            C = "A" + C

            For i = 0 To C.Length - 2
                Dim newOpties As New List(Of String)

                For Each S In GetSequencesNumberPad(C(i), C(i + 1))
                    For Each O In Inputs
                        newOpties.Add(O + S)
                    Next
                Next

                Inputs = newOpties
            Next

            Dim lengtes As New List(Of Long)

            For Each inp In Inputs
                Dim lengte As Long = 0

                inp = "A" + inp

                For i = 0 To inp.Length - 2
                    lengte += GetLengte(inp(i), inp(i + 1), robots - 1)
                Next

                lengtes.Add(lengte)
            Next

            Dim Numeric = CInt(C.Substring(1, C.Length - 2))
            Dim complexity As Long = lengtes.Min * Numeric

            ' Console.WriteLine(C + " - " + lengtes.Min.ToString + " * " + Numeric.ToString + " = " + complexity.ToString)
            totaal += complexity

        Next

        Return totaal

    End Function

    Private Function GetLengte(c1 As Char, c2 As Char, diepte As Integer) As Long
        Dim lengte As Long = 0

        If diepte = 0 Then
            lengte = 1
        Else
            If Not Cache.TryGetValue((c1, c2, diepte), lengte) Then
                Dim Mogelijkheden = GetSequencesKeyPad(c1, c2)
                Dim lengtes As New List(Of Long)

                For Each S In Mogelijkheden
                    Dim LengteA As Long = 0
                    If S.Length = 1 Then
                        LengteA = 1
                    Else
                        S = "A" + S
                        For i = 0 To S.Length - 2
                            LengteA += GetLengte(S(i), S(i + 1), diepte - 1)
                        Next
                    End If

                    lengtes.Add(LengteA)
                Next

                lengte = lengtes.Min
                Cache.Add((c1, c2, diepte), lengte)
            End If
        End If

        Return lengte

    End Function



    Private Function GetSequencesNumberPad(S As Char, E As Char) As List(Of String)
        Dim opties = New List(Of String)

        Dim difx As Short = NumPad(S).x - NumPad(E).x
        Dim difY As Short = NumPad(S).y - NumPad(E).y

        MaakString("", Math.Abs(difY), If(difY > 0, "^", "v"), Math.Abs(difx), If(difx > 0, "<", ">"), opties)

        If difx <> 0 AndAlso difY <> 0 AndAlso (NumPad(S).x = 0 OrElse NumPad(E).x = 0) AndAlso (NumPad(S).y = 3 OrElse NumPad(E).y = 3) Then
            Select Case S
                Case "7"
                    opties.RemoveAll(Function(x) x.StartsWith("vvv"))
                Case "4"
                    opties.RemoveAll(Function(x) x.StartsWith("vv"))
                Case "1"
                    opties.RemoveAll(Function(x) x.StartsWith("v"))
                Case "0"
                    opties.RemoveAll(Function(x) x.StartsWith("<"))
                Case "A"
                    opties.RemoveAll(Function(x) x.StartsWith("<<"))
            End Select
        End If

        Return opties

    End Function

    Private Function GetSequencesKeyPad(S As Char, E As Char) As List(Of String)
        Dim opties = New List(Of String)

        Dim difx As Short = Keypad(S).x - Keypad(E).x
        Dim difY As Short = Keypad(S).y - Keypad(E).y

        MaakString("", Math.Abs(difY), If(difY > 0, "^", "v"), Math.Abs(difx), If(difx > 0, "<", ">"), opties)

        If difx <> 0 AndAlso difY <> 0 AndAlso (Keypad(S).x = 0 OrElse Keypad(E).x = 0) AndAlso (Keypad(S).y = 0 OrElse Keypad(E).y = 0) Then
            Select Case S
                Case "A"
                    opties.RemoveAll(Function(x) x.StartsWith("<<"))
                Case "^"
                    opties.RemoveAll(Function(x) x.StartsWith("<"))
                Case "<"
                    opties.RemoveAll(Function(x) x.StartsWith("^"))
            End Select
        End If

        Return opties
    End Function

    Private Sub MaakString(current As String, aantalA As Integer, A As Char, aantalB As Integer, B As Char, combinaties As List(Of String))
        If aantalA = 0 AndAlso aantalB = 0 Then
            combinaties.Add(current + "A")
        Else
            If aantalA > 0 Then
                MaakString(current + A, aantalA - 1, A, aantalB, B, combinaties)
            End If

            If aantalB > 0 Then
                MaakString(current + B, aantalA, A, aantalB - 1, B, combinaties)
            End If
        End If
    End Sub



End Module
