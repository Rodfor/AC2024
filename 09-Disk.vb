Imports System.IO

Module Disk  

     Public Sub MoveFiles()
        Dim fileName As String = "Disk.txt"
        Dim projectPath As String = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName
        Dim pad As String = Path.Combine(projectPath, fileName)
        
        Dim input As String = File.ReadAllText(pad)

        Dim Output As New List(Of (aantal as Integer, c as String))

        For i = 0 To input.Length() - 1
            Dim nummer As Integer = input(i).ToString
            Dim c As String 
            If i Mod 2 = 0 Then
               c = i/2
            Else
                c = "."
            End If
            Output.Add((nummer, c))
        Next

        Dim k As Integer = Output.Count - 1

        While k > 0
            'Dim outputstring As String = ""
            'For each O In Output
            '    For l = 1 To O.aantal
            '        outputstring += O.c
            '    Next
            'Next      

            ' Console.WriteLine(outputstring)

            If not Output(k).c = "." Then
                Dim lastpunt As Integer = 1
                While lastPunt < k
                    If Output(lastpunt).c = "." Then
                        If Output(lastPunt).aantal = Output(k).aantal Then
                            Output(lastPunt) = Output(k)
                            Output(k) = (Output(k).aantal, ".")
                            Exit While
                        ElseIf output(lastPunt).aantal > Output(k).aantal Then
                            Dim offset As Integer = (output(lastPunt).aantal - Output(k).aantal)
                            Output(lastPunt) = Output(k)
                            Output(k) = (Output(k).aantal, ".") 
                            Output.Insert(lastpunt + 1,(offset , "."))
                            k += 1
                            Exit While
                        End If
                    End If          
                    lastPunt += 1
                End While
            End If
                
           
            k -= 1        
        End While 
        
        Dim totaal As long = 0
        Dim teller As Integer = 0
        For each O In Output
                For l = 1 To O.aantal
                    If O.c <> "." then totaal+= O.c * teller
                    teller += 1
                Next         
        Next           
        Console.WriteLine(totaal)
    End Sub
      
    Public Sub MoveFiles1()
        Dim fileName As String = "Disk.txt"
        Dim projectPath As String = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName
        Dim pad As String = Path.Combine(projectPath, fileName)
        
        Dim input As String = File.ReadAllText(pad)

        Dim Output As New List(Of String)

        For i = 0 To input.Length() - 1
            Dim nummer As Integer = input(i).ToString
            Dim c As String 
            If i Mod 2 = 0 Then
               c = i/2
            Else
                c = "."
            End If
             For j = 1 To nummer
                Output.add(c)
            Next
        Next

        Dim lastPunt As Integer = input(0).ToString
        Dim k As Integer = Output.Count - 1

        While k > lastPunt
            If not Output(k) = "." Then
                
                While lastPunt < k
                    If Output(lastPunt) = "." Then
                        Output(lastPunt) = Output(k)
                        Output.RemoveAt(k)
                        
                        ' For l = 0 To Output.Count() - 1
                        '    Console.Write(Output(l))
                        'Next
                        ' Console.WriteLine()
                        Exit While
                    End If
                    lastPunt += 1
                End While
            Else
                Output.RemoveAt(k)
            End If
            k -= 1        
        End While 
        
        Dim totaal As long = 0
       

        For m = 0 To Output.Count() - 1
            totaal += (Output(m) * m)
        Next

            
        Console.WriteLine(totaal)
    End Sub
      
End Module
