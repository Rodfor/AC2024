Module HulpFuncties
	Public Function VindGgd(a As Integer, b As Integer) As Integer
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

	Public Function GCD(a As Long, b As Long) As Long
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

	Public Function LCM(a As Long, b As Long) As Long
		Return (a * b) \ GCD(a, b)
	End Function

End Module
