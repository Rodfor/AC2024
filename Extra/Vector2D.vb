Public Class Vector2D
    ' Eigenschappen voor de X- en Y-coördinaten
    Public Property X As Double
    Public Property Y As Double

    Public ReadOnly Property Key As (Double, Double)
        Get
            Return (X, Y)
        End Get
    End Property

    ' Constructor
    Public Sub New(x As Double, y As Double)
        Me.X = x
        Me.Y = y
    End Sub

    ' ToString-methode voor nette weergave
    Public Overrides Function ToString() As String
        Return $"({X}, {Y})"
    End Function

    ' Vectoroptelling
    Public Shared Function Add(v1 As Vector2D, v2 As Vector2D) As Vector2D
        Return New Vector2D(v1.X + v2.X, v1.Y + v2.Y)
    End Function

    ' Vectoraftrekking
    Public Shared Function Subtract(v1 As Vector2D, v2 As Vector2D) As Vector2D
        Return New Vector2D(v1.X - v2.X, v1.Y - v2.Y)
    End Function

    ' Schalen met een scalar
    Public Shared Function Multiply(v As Vector2D, scalar As Double) As Vector2D
        Return New Vector2D(v.X * scalar, v.Y * scalar)
    End Function

    ' Berekening van de lengte (magnitude) van de vector
    Public Function Length() As Double
        Return Math.Sqrt(X * X + Y * Y)
    End Function

    ' Normaliseren van de vector
    Public Function Normalize() As Vector2D
        Dim len As Double = Me.Length()
        If len = 0 Then
            Throw New InvalidOperationException("Kan een nulvector niet normaliseren.")
        End If

        Return New Vector2D(X / len, Y / len)
    End Function

    ' Berekening van het dot-product
    Public Shared Function Dot(v1 As Vector2D, v2 As Vector2D) As Double
        Return v1.X * v2.X + v1.Y * v2.Y
    End Function

    Public Shared Function InBounds(v1 As Vector2D, UpperX As Double, UpperY As Double) As Boolean
        Return v1.X <= UpperX AndAlso v1.X >= 0 AndAlso v1.Y <= UpperY AndAlso v1.Y >= 0
    End Function
End Class
