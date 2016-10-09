Module Utilities

    Public Function ContainsLetters(ByVal str As String)

        For i = 0 To str.Length - 1
            If Char.IsLetter(str.Chars(i)) Then
                Return True
            End If
        Next

        Return False

    End Function

    Public Function Average(x As List(Of Double))

        'Takes an average in absolute terms

        Dim result As Double

        For i = 0 To x.Count - 1
            result += x(i)
        Next

        Return result / x.Count

    End Function

    Public Function Sum(x As List(Of Double))

        'Takes an average in absolute terms

        Dim result As Double

        For i = 0 To x.Count - 1
            result += x(i)
        Next

        Return result

    End Function

    Public Function StandardDeviation(x As List(Of Double))

        Dim result As Double
        Dim avg As Double = Average(x)

        For i = 0 To x.Count - 1
            result += Math.Pow((x(i) - avg), 2)
        Next

        result /= x.Count

        Return result

    End Function

    Public Function Standardise(x As List(Of Double))

        Dim result As New StandardisedData()

        Dim list As New List(Of Double)
        Dim avg As Double = Average(x)
        Dim std As Double = StandardDeviation(x)


        For i = 0 To x.Count - 1
            list.Add((x(i) - avg) / std)
        Next

        result.average = avg
        result.standardDeviation = std
        result.list = list

        Return result

    End Function

End Module
