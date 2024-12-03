Imports System.IO
Imports Microsoft.VisualBasic.FileIO

Module Computers
    Public Sub Multiply1()
        Dim fileName As String = "Computers.txt"
        Dim projectPath As String = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName
        Dim pad As String = Path.Combine(projectPath, fileName)

        Dim lijnen As new List(Of String)

        Using parser = New TextFieldParser(pad)
            parser.TextFieldType = FieldType.Delimited
            parser.SetDelimiters("mul")

            While Not parser.EndOfData
                Dim fields As String() = parser.ReadFields()
                lijnen.AddRange(fields)
            End While

            parser.Close()
            parser.Dispose()
        End Using

        Dim total As Integer = 0

        For each L In lijnen
            Console.Write(L + " - ")
            If L.StartsWith("(") Then 
                Dim KommaIndex = L.IndexOf(",")
                If not KommaIndex <= 1 Then 
                    Dim n1 =  L.Substring(1, KommaIndex - 1)
                    If IsNumeric(n1) Then 
                         Dim haakjesIndex = L.IndexOf(")")
                         If not haakjesIndex <= 3 Then 
                             Dim n2 =  L.Substring(KommaIndex + 1, haakjesIndex - KommaIndex - 1)
                            If IsNumeric(n2) Then 
                                Dim Res = n1*n2
                                Console.Write(Res.ToString + " -- " + n1 + "*" + n2)
                                total += Res
                            End If                 
                         End If                
                    End If                 
                End If       
            End If
             Console.Writeline("")
        Next

        Console.WriteLine(total)

    End Sub

      Public Sub Multiply()
        Dim fileName As String = "Computers.txt"
        Dim projectPath As String = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName
        Dim pad As String = Path.Combine(projectPath, fileName)

        Dim lijnen As new List(Of String)
        Dim enabled As Boolean = True

        Using parser = New TextFieldParser(pad)
            parser.TextFieldType = FieldType.Delimited
            parser.SetDelimiters("mul")

            While Not parser.EndOfData
                Dim fields As String() = parser.ReadFields()
                lijnen.AddRange(fields)
            End While

            parser.Close()
            parser.Dispose()
        End Using

        Dim total As Integer = 0

        For each L In lijnen
            Console.Write(L + " - ")
            If enabled Then
                If L.StartsWith("(") Then 
                    Dim KommaIndex = L.IndexOf(",")
                    If not KommaIndex <= 1 Then 
                        Dim n1 =  L.Substring(1, KommaIndex - 1)
                        If IsNumeric(n1) Then 
                             Dim haakjesIndex = L.IndexOf(")")
                             If not haakjesIndex <= 3 Then 
                                 Dim n2 =  L.Substring(KommaIndex + 1, haakjesIndex - KommaIndex - 1)
                                If IsNumeric(n2) Then 
                                    Dim Res = n1*n2
                                    Console.Write(Res.ToString + " -- " + n1 + "*" + n2)
                                    total += Res
                                End If                 
                             End If                
                        End If                 
                    End If       
                End If
                 if L.Contains("don't()") then enabled = False
            Else 
                if L.Contains("do()") then  enabled = True
                Console.Write("disabled")
            End If 
            
            Console.WriteLine("")
        Next

        Console.WriteLine(total)

    End Sub
End Module
