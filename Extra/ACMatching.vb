Imports System.Collections.Generic
Imports System.Globalization

Module ACMatching

    Private out() As Integer
    Private f() As Integer
    Private g(,) As Integer

    Private MAXC As Integer
    Private MAXS As Integer

    Private arr As String()
    Private k As Integer

    Public Sub InitACMatching(MaxChar As Integer, MaxStates As Integer, keywords As String())
        MAXC = MaxChar - 1
        MAXS = MaxStates - 1

        ReDim out(MAXS)
        ReDim f(MAXS)
        ReDim g(MAXS, MAXC)

        arr = keywords
        k = keywords.Length

        ' Preprocess patterns. Build machine with goto, failure and output functions
        buildMatchingMachine()

    End Sub

    ' Builds the String matching machine.
    ' arr - array of words. The index of each keyword is important:
    ' "out[state] & (1 << i)" is > 0 if we just found word[i] in the text.
    ' Returns the number of states that the built machine has.
    ' States are numbered 0 up to the return value - 1, inclusive.
    Public Function buildMatchingMachine() As Integer
        ' Initialize all values in output function as 0.
        For i As Integer = 0 To out.Length - 1
            out(i) = 0
        Next

        ' Initialize all values in goto function as -1.
        For i As Integer = 0 To MAXS
            For j As Integer = 0 To MAXC
                g(i, j) = -1
            Next
        Next

        ' Initially, we just have the 0 state
        Dim states As Integer = 1

        ' Construct values for goto function, i.e., fill g[,]
        ' This is same as building a Trie for arr
        For i As Integer = 0 To k - 1
            Dim word As String = arr(i)
            Dim currentState As Integer = 0

            ' Insert all characters of current word in arr
            For j As Integer = 0 To word.Length - 1
                Dim ch As Integer = Asc(word(j)) - Asc("a")

                ' Allocate a new node (create a new state) if a node for ch doesn't exist.
                If g(currentState, ch) = -1 Then
                    g(currentState, ch) = states
                    states += 1
                End If

                currentState = g(currentState, ch)
            Next

            ' Add current word in output function
            out(currentState) = out(currentState) Or (1 << i)
        Next

        ' For all characters which don't have an edge from root (or state 0) in Trie,
        ' add a goto edge to state 0 itself
        For ch As Integer = 0 To MAXC
            If g(0, ch) = -1 Then
                g(0, ch) = 0
            End If
        Next

        ' Now, let's build the failure function
        ' Initialize values in fail function for all states
        For i As Integer = 0 To MAXC
            f(i) = 0
        Next

        ' Failure function is computed in breadth first order using a queue
        Dim q As New Queue(Of Integer)()

        ' Iterate over every possible input
        For ch As Integer = 0 To MAXC
            ' All nodes of depth 1 have failure function value as 0.
            If g(0, ch) <> 0 Then
                f(g(0, ch)) = 0
                q.Enqueue(g(0, ch))
            End If
        Next

        ' Now queue has states 1 and 3
        While q.Count <> 0
            ' Remove the front state from queue
            Dim state As Integer = q.Dequeue()

            ' For the removed state, find failure function for all those characters
            ' for which goto function is not defined.
            For ch As Integer = 0 To MAXC
                ' If goto function is defined for character 'ch' and 'state'
                If g(state, ch) <> -1 Then
                    ' Find failure state of removed state
                    Dim failure As Integer = f(state)

                    ' Find the deepest node labeled by proper suffix of String from root to current state.
                    While g(failure, ch) = -1
                        failure = f(failure)
                    End While

                    failure = g(failure, ch)
                    f(g(state, ch)) = failure

                    ' Merge output values
                    out(g(state, ch)) = out(g(state, ch)) Or out(failure)

                    ' Insert the next level node (of Trie) in Queue
                    q.Enqueue(g(state, ch))
                End If
            Next
        End While

        Return states
    End Function

    ' Returns the next state the machine will transition to using goto and failure functions.
    ' currentState - The current state of the machine. Must be between 0 and the number of states - 1, inclusive.
    ' nextInput - The next character that enters into the machine.
    Public Function findNextState(currentState As Integer, nextInput As Char) As Integer
        Dim answer As Integer = currentState
        Dim ch As Integer = Asc(nextInput) - Asc("a")

        ' If goto is not defined, use failure function
        While g(answer, ch) = -1
            answer = f(answer)
        End While

        Return g(answer, ch)
    End Function


    ' This function finds all occurrences of all array words in text.
    Public Sub searchWords(text As String)
        ' Initialize current state
        buildMatchingMachine()
        Dim currentState As Integer = 0

        ' Traverse the text through the built machine to find all occurrences of words in arr
        For i As Integer = 0 To text.Length - 1
            currentState = findNextState(currentState, text(i))

            ' If match not found, move to next state
            If out(currentState) = 0 Then
                Continue For
            End If

            ' Match found, print all matching words of arr using output function.
            For j As Integer = 0 To k - 1
                If (out(currentState) And (1 << j)) > 0 Then
                    Dim startIndex As Integer = i - arr(j).Length + 1
                    If startIndex >= 0 Then
                        Console.WriteLine("Word " & arr(j) & " appears from " & startIndex & " to " & i)
                    End If
                End If
            Next
        Next
    End Sub

    Public Function MatchAll(text As String, currentindex As Integer, visited As HashSet(Of (Integer, Integer))) As Boolean
        ' Initialize current state
        Dim currentState As Integer = 0

        ' Base case: if the current index reaches the end of the text, return true
        If currentindex = text.Length Then
            Return True
        End If

        ' Check if the current state and index have been visited before
        If visited.Contains((currentState, currentindex)) Then
            Return False
        End If

        ' Add the current state and index to visited set
        visited.Add((currentState, currentindex))

        For i As Integer = currentindex To MATH.Min(text.Length - 1, currentindex + 10)
            ' Traverse the text through the built machine to find all occurrences of words in arr
            currentState = findNextState(currentState, text(i))

            ' If match not found, move to next state
            If out(currentState) = 0 Then
                Continue For
            End If

            ' Match found, check all matching words of arr using output function.
            For j As Integer = 0 To k - 1
                If (out(currentState) And (1 << j)) > 0 Then
                    If currentindex + arr(j).Length - 1 = i Then
                        If MatchAll(text, i + 1, visited) Then
                            Return True
                        End If
                    End If
                End If
            Next
        Next

        Return False
    End Function


End Module