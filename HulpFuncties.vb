Module HulpFuncties
     Public Function VindGgd(a As Integer, b as integer) As Integer
		a = Math.Abs(a)
		b = Math.Abs(b)

         If a = 0 Then
		    Return b
	    End If

	    While b <> 0
		    If a > b Then
			    a -= b
		    Else
			    b -= a
		    End If
	    End While

	    Return a
    End Function
End Module
