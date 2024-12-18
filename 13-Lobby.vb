Imports System.IO
Imports System.Numerics
Imports System.Security.AccessControl
Imports Microsoft.VisualBasic.FileIO

Module Lobby
    Public Sub Gamba()
        Dim fileName As String = "Lobby.txt"
        Dim projectPath As String = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName
        Dim pad As String = Path.Combine(projectPath, fileName)

        Dim lijnen() As String = File.ReadAllLines(pad)

        Dim Machines As New List(Of Machine)

        For i = 0 To lijnen.Count()
            If i = 0 OrElse i Mod 4 = 0 Then
                Dim M As New Machine
                Dim XY As String() = lijnen(i).Split(",")
                M.A = (XY(0).Split("+")(1), XY(1).Split("+")(1))
                Machines.Add(M)
            ElseIf i Mod 4 = 1 Then
                Dim M As Machine = Machines.Last()
                Dim XY As String() = lijnen(i).Split(",")
                M.B = (XY(0).Split("+")(1), XY(1).Split("+")(1))
            ElseIf i Mod 4 = 2 Then
                Dim M As Machine = Machines.Last()
                Dim XY As String() = lijnen(i).Split(",")
                M.Prijs = (10000000000000 + XY(0).Split("=")(1), 10000000000000 + XY(1).Split("=")(1))
            End If
        Next

        Dim totaal As Long = 0

        For Each M In Machines
            totaal += M.BerekenTokens()
        Next


        Console.WriteLine(totaal)


    End Sub

    Private Class Machine
        Public A As (x As Integer, y As Integer)
        Public B As (x As Integer, y As Integer)

        Public TokensA As Integer = 3
        Public TokensB As Integer = 1

        Public Prijs As (x As Long, y As Long)

        Public Function BerekenTokens() As Long

            If Not Prijs.x Mod GCD(A.x, B.x) = 0 Then
                Console.WriteLine("GCD x")
                Return 0
            End If

            If Not Prijs.y Mod GCD(A.y, B.y) = 0 Then
                Console.WriteLine("GCD y")
                Return 0
            End If

            Dim aantalB As Double = (A.x * Prijs.y - A.y * Prijs.x) / (A.x * B.y - B.x * A.y)

            If aantalB < 0 Then
                Console.WriteLine(aantalB.ToString + " < 0 B")
                Return 0
            ElseIf aantalB Mod 1 <> 0 Then
                Console.WriteLine(aantalB.ToString + " geen int B")
                Return 0
            End If

            Dim aantalA As Double = (Prijs.x - aantalB * B.x) / A.x
            If aantalA < 0 Then
                Console.WriteLine(aantalA.ToString + " < 0 A")
                Return 0
            ElseIf aantalA Mod 1 <> 0 Then
                Console.WriteLine(aantalA.ToString + " geen int A")
                Return 0
            End If

            Console.WriteLine(aantalA.ToString + " - " + aantalB.ToString + " = " + (aantalA * TokensA + aantalB * TokensB).ToString)

            Return aantalA * TokensA + aantalB * TokensB

        End Function
    End Class

End Module
