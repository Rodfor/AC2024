Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Security.Cryptography.X509Certificates
Imports System.Windows

Module Robot
    Private NumPad As New Dictionary(Of Char, (x As Integer, y As Integer))
    Private Keypad As New Dictionary(Of Char, (x As Integer, y As Integer))
    Private MemorySingle As New Dictionary(Of String, List(Of String))
    Private MemoryMulti As New Dictionary(Of String, List(Of String))


    Private Memory5 As New Dictionary(Of String, List(Of String))
    Private Memory25 As New Dictionary(Of String, List(Of String))

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

        Dim robots As Integer = 25
        Dim totaal As Long = 0

        For Each C In Codes
            Console.WriteLine(C + ":")

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

            Dim minInput As Integer = Inputs.Min(Function(x) x.Length)
            Inputs = Inputs.Where(Function(x) x.Length = minInput).ToList()

            For i = 0 To 2
                Dim newInputs As New List(Of String)
                For Each inp In Inputs
                    newInputs.AddRange(GetInputs(inp))
                Next
                Inputs = newInputs
            Next


            'For R = 1 To robots - 1
            '    Inputs = GetInputs(Inputs)
            'Next

            Dim Numeric = CInt(C.Substring(1, C.Length - 2))
            Dim complexity As Long = output.First.Length * Numeric

            Console.WriteLine(C + " - " + output.First.Length.ToString + " * " + Numeric.ToString + " = " + complexity.ToString)
            totaal += complexity

        Next


        Console.WriteLine(totaal)
    End Sub


    Private Function GetInputs25(input As String)
        Dim opties As New List(Of String) From {
             ""
         }

        For i = 0 To input.Length - 2
            Dim newOpties As New List(Of String)
            Dim key = input(i) + input(i + 1)
            Dim memoryValues As List(Of String)

            If Not Memory25.TryGetValue(key, memoryValues) Then
                memoryValues = New List(Of String)
                Memory25.Add(key, memoryValues)

                Dim outputs As New List(Of String) From {
                        key
                    }

                For k = 0 To 4
                    Dim newoutputs As New List(Of String)
                    For Each output In outputs
                        newoutputs.AddRange(GetInputs5(output))
                    Next
                    outputs = newoutputs
                Next

                For Each O In outputs
                    memoryValues.Add(O)
                Next
            End If

            For Each S In memoryValues
                For Each K In opties
                    newOpties.Add(K + S)
                Next
            Next

            opties = newOpties
            Dim minOutput As Integer = opties.Min(Function(x) x.Length)
            opties = opties.Where(Function(x) x.Length = minOutput).ToList()

        Next

        Return opties

    End Function


    Private Function GetInputs5(input As String)
        Dim opties As New List(Of String) From {
            ""
        }

        For i = 0 To input.Length - 2
            Dim newOpties As New List(Of String)
            Dim key = input(i) + input(i + 1)
            Dim memoryValues As List(Of String)

            If Not Memory5.TryGetValue(key, memoryValues) Then
                memoryValues = New List(Of String)
                Memory5.Add(key, memoryValues)

                Dim outputs As New List(Of String) From {
                        key
                    }

                For k = 0 To 4
                    Dim newoutputs As New List(Of String)
                    For Each output In outputs

                        newoutputs.AddRange(GetInputs(output))
                    Next
                    outputs = newoutputs
                Next

                For Each O In outputs
                    memoryValues.Add(O)
                Next
            End If

            For Each S In memoryValues
                For Each K In opties
                    newOpties.Add(K + S)
                Next
            Next

            opties = newOpties
            Dim minOutput As Integer = opties.Min(Function(x) x.Length)
            opties = opties.Where(Function(x) x.Length = minOutput).ToList()
        Next

        Return opties

    End Function



    Private Function GetInputs(Outputs As List(Of String))

        Dim Inputs As New List(Of String)

        For Each O In Outputs
            O = "A" + O

            Dim opties As List(Of String)

            If MemoryMulti.TryGetValue(O, opties) Then
                Inputs.AddRange(opties)
            Else
                opties = New List(Of String) From {
                    ""
                }

                For i = 0 To O.Length - 2
                    Dim newOpties As New List(Of String)
                    Dim key = O(i) + O(i + 1)
                    Dim memoryValues As List(Of String)

                    If Not MemorySingle.TryGetValue(key, memoryValues) Then
                        memoryValues = New List(Of String)
                        MemorySingle.Add(key, memoryValues)
                        For Each S In GetSequencesKeyPad(key)
                            memoryValues.Add(S)
                        Next
                    End If

                    For Each S In memoryValues
                        For Each K In opties
                            newOpties.Add(K + S)
                        Next
                    Next

                    opties = newOpties
                Next

                MemoryMulti.Add(O, opties)
                Inputs.AddRange(opties)
            End If
        Next


        Dim minInput As Integer = Inputs.Min(Function(x) x.Length)
        Inputs = Inputs.Where(Function(x) x.Length = minInput).ToList()

        Return Inputs
    End Function


    Private Function GetSequencesNumberPad(S As Char, E As Char) As List(Of String)
        Dim opties = New List(Of String)

        Dim difx As Short = NumPad(S).x - NumPad(E).x
        Dim difY As Short = NumPad(S).y - NumPad(E).y

        Dim listx As String = ""

        If difx > 0 Then
            For i = 1 To difx
                listx += "<"
            Next
        Else
            For i = 1 To Math.Abs(difx)
                listx += ">"
            Next
        End If

        Dim listy As String = ""


        If difY > 0 Then
            For i = 1 To difY
                listy += "^"
            Next
        Else
            For i = 1 To Math.Abs(difY)
                listy += "v"
            Next
        End If

        If difx <> 0 AndAlso difY <> 0 AndAlso (NumPad(S).x = 0 OrElse NumPad(E).x = 0) AndAlso (NumPad(S).y = 3 OrElse NumPad(E).y = 3) Then
            If difY > 0 Then
                opties.Add(listy + listx + "A")
            Else
                opties.Add(listx + listy + "A")
            End If
        Else
            If listx.Length = 0 OrElse listy.Length = 0 Then
                opties.Add(listx + listy + "A")
            Else
                opties.Add(listx + listy + "A")
                opties.Add(listy + listx + "A")
            End If
        End If

        Return opties

    End Function

    Private Function GetSequencesKeyPad(SE As String) As List(Of String)
        Dim opties = New List(Of String)

        Dim S = SE(0)
        Dim E = SE(1)

        Dim difx As Short = Keypad(S).x - Keypad(E).x
        Dim difY As Short = Keypad(S).y - Keypad(E).y

        Dim listx As String = ""

        If difx > 0 Then
            For i = 1 To difx
                listx += "<"
            Next
        Else
            For i = 1 To Math.Abs(difx)
                listx += ">"
            Next
        End If

        Dim listy As String = ""


        If difY > 0 Then
            For i = 1 To difY
                listy += "^"
            Next
        Else
            For i = 1 To Math.Abs(difY)
                listy += "v"
            Next
        End If

        If difx <> 0 AndAlso difY <> 0 AndAlso (Keypad(S).x = 0 OrElse Keypad(E).x = 0) AndAlso (Keypad(S).y = 0 OrElse Keypad(E).y = 0) Then
            If difY > 0 Then
                opties.Add(listx + listy + "A")
            Else
                opties.Add(listy + listx + "A")
            End If
        Else
            If listx.Length = 0 OrElse listy.Length = 0 Then
                opties.Add(listx + listy + "A")
            Else
                opties.Add(listx + listy + "A")
                opties.Add(listy + listx + "A")
            End If
        End If


        Return opties

    End Function


End Module
