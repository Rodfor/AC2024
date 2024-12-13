Imports System.IO
Public Module Hike
        Private upperX As Integer
        Private upperY As Integer
        Private Map As New Dictionary(Of (x As Integer, y As Integer), Integer)

      Public Sub Trails()
        Dim fileName As String = "Hike.txt"
        Dim projectPath As String = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName
        Dim pad As String = Path.Combine(projectPath, fileName)

        
        Dim lijnen() As String = File.ReadAllLines(pad)
        Dim TrailStarts As New List(Of (x As Integer, y As Integer))

        upperX = lijnen.Length() - 1
        upperY = lijnen.First.Length() - 1

        For x = 0 To upperX
            Dim lijn = lijnen(x)
            For y = 0 To upperY
                Map.Add((x, y), lijn(y).ToString)
                If lijn(y).ToString = 0 Then 
                    TrailStarts.Add((x,y))
                End If
            Next
        Next

        Dim totaal As Integer = 0

        For each Start In TrailStarts
           'totaal += FindNextUnique(Start).Distinct.Count()
           totaal += FindNext(Start)
            Console.WriteLine(Start.x.ToString + "," + Start.y.ToString + " - " + totaal.ToString)
        Next
       
        Console.WriteLine(totaal)
    End Sub

    Private Function FindNextUnique(Node As (x as Integer, y as integer)) As List(Of (x as Integer, y as integer))
        Dim totaal As New List(Of (x as Integer, y as integer))
        Dim current As Integer = Map(Node)

        If Current = 9 Then
            totaal.Add(Node)
            Return totaal
        End If

        If Node.x > 0 
          If Map((Node.x - 1, Node.y)) = current + 1 then
                totaal.addRange(FindNextUnique((Node.x - 1, Node.y)))
          End If
        End If
        If Node.x < upperX
          If Map((Node.x + 1, Node.y)) = current + 1 then
                totaal.AddRange( FindNextUnique((Node.x + 1, Node.y)))
          End If
        End If
        If Node.y > 0 
          If Map((Node.x, Node.y - 1)) = current + 1 then
                totaal.AddRange(FindNextUnique((Node.x, Node.y - 1)))
          End If
        End If
        If Node.y < upperY
          If Map((Node.x, Node.y+1)) = current + 1 then
                totaal.AddRange(FindNextUnique((Node.x, Node.y+1)))
          End If
        End If

        Return totaal
    End Function

     Private Function FindNext(Node As (x as Integer, y as integer)) As Integer
        Dim totaal As Integer = 0
        Dim current As Integer = Map(Node)

        If Current = 9 Then
            Return 1
        End If

        If Node.x > 0 
          If Map((Node.x - 1, Node.y)) = current + 1 then
                totaal += FindNext((Node.x - 1, Node.y))
          End If
        End If
        If Node.x < upperX
          If Map((Node.x + 1, Node.y)) = current + 1 then
                totaal +=FindNext((Node.x + 1, Node.y))
          End If
        End If
        If Node.y > 0 
          If Map((Node.x, Node.y - 1)) = current + 1 then
                totaal +=FindNext((Node.x, Node.y - 1))
          End If
        End If
        If Node.y < upperY
          If Map((Node.x, Node.y+1)) = current + 1 then
                totaal +=FindNext((Node.x, Node.y+1))
          End If
        End If

        Return totaal
    End Function


End Module
