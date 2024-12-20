Public Class Heap
    Public items As New List(Of Node)
    Public Count As Integer

    Public Sub Add(item As Node)
        item.HeapIndex = Count
        items.Add(item)
        SortUp(item)
        Count += 1
    End Sub

    Public Function RemoveFirst() As Node
        Dim N = items.First
        Count -= 1
        items(0) = items(Count)
        items(0).HeapIndex = 0
        items.RemoveAt(Count)
        If Count > 0 Then SortDown(items(0))
        Return N
    End Function

    Public Sub Update(N As Node)
        SortUp(N)
    End Sub


    Public Sub SortUp(item As Node)
        Dim parentIndex = (item.HeapIndex - 1) / 2
        While parentIndex >= 0
            Dim parentItem = items(parentIndex)
            If item.CompareTo(parentItem) < 0 Then
                Swap(item, parentItem)
                parentIndex = (item.HeapIndex - 1) / 2
            Else
                Exit While
            End If
        End While
    End Sub

    Public Sub SortDown(item As Node)
        While True
            Dim IndexLeft = item.HeapIndex * 2 + 1
            Dim IndexRight = item.HeapIndex * 2 + 2
            Dim IndexSwap = 0

            If IndexLeft < Count Then
                IndexSwap = IndexLeft
                If IndexRight < Count Then
                    If items(IndexLeft).CompareTo(items(IndexRight)) > 0 Then
                        IndexSwap = IndexRight
                    End If
                End If

                If item.CompareTo(items(IndexSwap)) > 0 Then
                    Swap(item, items(IndexSwap))
                Else
                    Exit While
                End If
            Else
                Exit While
            End If

        End While


    End Sub

    Private Sub Swap(itemA As Node, itemB As Node)
        items(itemA.HeapIndex) = itemB
        items(itemB.HeapIndex) = itemA
        Dim index = itemA.HeapIndex
        itemA.HeapIndex = itemB.HeapIndex
        itemB.HeapIndex = index
    End Sub

    Public Shared Function GetDistance(N1 As Node, N2 As Node) As Integer
        Dim dstX = Math.Abs(N1.x - N2.x)
        Dim dstY = Math.Abs(N1.y - N2.y)

        Return dstX + dstY
    End Function


End Class

Public Class Node
    Public x As Integer
    Public y As Integer

    Public gCost As Integer
    Public hCost As Integer

    Public parent As Node

    Public Visited As Boolean = False

    Public Waarde As Char

    Public Property HeapIndex As Integer

    Public ReadOnly Property Name As String
        Get
            Return x.ToString + "," + y.ToString
        End Get
    End Property

    Public ReadOnly Property fCost As Integer
        Get
            Return gCost + hCost
        End Get
    End Property

    Public Sub New(x As Integer, y As Integer, waarde As Char)
        Me.x = x
        Me.y = y
        Me.Waarde = waarde
    End Sub

    Public Function CompareTo(N As Node) As Integer
        If fCost = N.fCost Then
            Return hCost - N.hCost
        Else
            Return fCost - N.fCost
        End If
    End Function
End Class



