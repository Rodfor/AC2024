Imports System

Public Module Program

    Sub Main(args As String())
        Dim stopwatch As Stopwatch = Stopwatch.StartNew()
        Patrol()
        stopwatch.Stop()
        Console.WriteLine("In " + stopwatch.ElapsedMilliseconds.ToString + "ms")
        Dim x = Console.ReadLine()
    End Sub
End Module
