Imports System.IO
Imports System.Reflection

Module Toilet
    Private upperX As Integer
    Private upperY As Integer

    Public Sub RobotPatrol()
        Dim fileName As String = "Toilet.txt"
        Dim projectPath As String = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName
        Dim pad As String = Path.Combine(projectPath, fileName)

        Dim Robots As New List(Of Robot)

        upperX = 100
        upperY = 102

        Dim lijnen() As String = File.ReadAllLines(pad)

        For Each L In lijnen
            Dim R As New Robot
            Dim vars = L.Split(" ")
            Dim plaatsvars = vars(0).Split("=")(1).Split(",")
            R.Positie = New Vector2D(plaatsvars(0), plaatsvars(1))
            Dim snelheidvars = vars(1).Split("=")(1).Split(",")
            R.Snelheid = New Vector2D(snelheidvars(0), snelheidvars(1))
            Robots.Add(R)
        Next

        Dim tijd As Integer = 0

        For Each R In Robots
            R.Move(1614)
        Next


        While True

            For Each R In Robots
                Dim Eindpositie As Vector2D = R.Move(1)
            Next

            tijd += 1

            If tijd Mod 101 = 0 Then
                For y = 0 To upperY
                    Console.WriteLine()
                    For x = 0 To upperX
                        Dim xcoord = x
                        Dim ycoord = y
                        If Robots.Where(Function(f) f.Positie.X = xcoord AndAlso f.Positie.Y = ycoord).Any Then
                            Console.Write("x")
                        Else
                            Console.Write(".")
                        End If
                    Next
                Next
                Console.WriteLine(tijd.ToString)

            End If
        End While

    End Sub

    Private Class Robot
        Public Positie As Vector2D
        Public Snelheid As Vector2D

        Public Function Move(tijd As Integer) As Vector2D
            Dim Eindpositie As Vector2D = Vector2D.Add(Positie, Vector2D.Multiply(Snelheid, tijd))

            Dim eindX = Eindpositie.X Mod (upperX + 1)
            Dim eindY = Eindpositie.Y Mod (upperY + 1)

            If eindX < 0 Then eindX += upperX + 1
            If eindY < 0 Then eindY += upperY + 1

            Dim corrigeerde = New Vector2D(Math.Abs(eindX), Math.Abs(eindY))

            'Console.WriteLine(Positie.ToString + " - " + Snelheid.ToString + " -> " + Eindpositie.ToString + " -> " + corrigeerde.ToString)

            Positie = corrigeerde

            Return corrigeerde
        End Function

    End Class
End Module
