Module Classifier

    'This is where all the learning stuff happens

    'This class is used to store standardised feature lists (we subtract the mean and divide by the standard deviation)
    Class StandardisedData
        Public list As New List(Of Double)
        Public average As Double
        Public standardDeviation As Double
    End Class

    Public thetas As New List(Of Double) 'Parameters 
    Public exampleInputs As New List(Of List(Of Double)) 'Training Examples (each row is an example, each column is a feature)
    Public exampleOutputs As New List(Of Double) ' The "Y" Matrix
    Public exampleHeadings As New List(Of String) 'The headings for the dataset (eg. "Age" "Hours")

    Public exampleCount As Double 'How many training examples do we have?
    Public featureCount As Double 'How many features do we have?
    Public desiredDelta As Double 'The change in the cost function required to end minimisation

    Dim lastCost As Double 'The last value of the cost function
    Dim deltaCost As Double

    Public standardisedFeatureData As List(Of StandardisedData) = New List(Of StandardisedData) 'A list of standardised feature lists

    'Predict what the output will be given example x
    Function Predict(x)

        Return LogisticFunction(Hypothesis(x))

    End Function

    'Check if the hypothesis works
    Sub Check()

        Dim correctlyClassified As Double = 0

        'Loop through results and see how many were classified correctly
        For i = 0 To exampleInputs.Count - 1

            Dim result As String
            If Predict(exampleInputs(i)) > 0.5 Then
                result = "Positive"
                If exampleOutputs(i) = 1 Then
                    correctlyClassified += 1
                End If
            Else
                result = "Negative"
                If exampleOutputs(i) = 0 Then
                    correctlyClassified += 1
                End If
            End If

            Dim trueClassification As String
            If exampleOutputs(i) = 1 Then
                trueClassification = "Positive"
            Else
                trueClassification = "Negative"
            End If

            Console.WriteLine(result + " and was actually " + trueClassification)

        Next

        Console.WriteLine("Correctly Classified " + correctlyClassified.ToString + " out of " + exampleInputs.Count.ToString)

    End Sub

    'Take in input features from specific training example and spew forth the hypothesis
    Function Hypothesis(x)

        Dim result As Double

        'Matrix multiplcation at its finest
        For i = 0 To thetas.Count - 1
            result += thetas(i) * x(i)
        Next

        Return result

    End Function


    Function LogisticFunction(x)
        'Compute the value of the logistic function given input x
        'a sigmoid function has an S shape and is given by 1/(e^(-t))
        'e = 2.71... (Euler's number)
        'Reference: https://en.wikipedia.org/wiki/Logistic_function

        Return (1 / (1 + Math.Exp(-x)))

    End Function

    Function LogisticCost()

        'Reference: http://openclassroom.stanford.edu/MainFolder/DocumentPage.php?course=MachineLearning&doc=exercises/ex5/ex5.html
        'TODO: Regularlisation

        Dim cost As Double

        'Loop through all the training examples and calculate the cost
        For i = 0 To exampleCount - 1

            Dim exampleCost As Double

            If exampleOutputs(i) = 0 Then
                exampleCost = Math.Log(1 - LogisticFunction(Hypothesis(exampleInputs(i))))
            Else
                exampleCost = Math.Log(LogisticFunction(Hypothesis(exampleInputs(i))))
            End If

            cost += exampleCost

        Next

        'Take an average
        cost *= -1 / exampleCount

        '(we multiply it by -1 to make sure that the function has a u shape rather than an n shape. this helps with minimisation)

        Return Math.Abs(cost)

    End Function

    Sub Minimise()

        'Simple implementation of adagrad bc no way in hell am I implementing bfgs or anything that requires the hessan
        'gradient = X'(h(x) - y)

        Dim learningRate As New List(Of Double)
        Dim derivative As New List(Of Double)
        Dim pastGradientSquares As New List(Of List(Of Double))
        Dim sumOfPastGradientSquares As New List(Of Double)

        'Initialise all the variables
        For i = 0 To featureCount
            derivative.Add(1)
            learningRate.Add(0.01)
            sumOfPastGradientSquares.Add(0)
            pastGradientSquares.Add(New List(Of Double))
        Next

        lastCost = LogisticCost()
        deltaCost = 100

        'Do this until we converge (the derivative = 0ish)
        While (Math.Abs(deltaCost) > desiredDelta)

            Dim difference As New List(Of Double)
            Dim h As New List(Of Double)

            'Reset the derivative
            For i = 0 To thetas.Count
                derivative(i) = 0
            Next

            'Loop through the hypotheses and populate the list h
            For i = 0 To exampleCount - 1
                h.Add(LogisticFunction(Hypothesis(exampleInputs(i))))
            Next

            'Get the difference between the hypothesised value and the acutal value
            For i = 0 To exampleCount - 1
                difference.Add(h(i) - exampleOutputs(i))
            Next

            'Multiply by the features 
            For i = 0 To exampleCount - 1
                For j = 0 To featureCount - 1
                    derivative(j) += difference(i) * exampleInputs(i)(j)

                    'Update the list of previous squared derivatives
                    pastGradientSquares(j).Add(Math.Pow(derivative(j), 2))

                    'If we exceed 10 things, just remove the oldest entry 
                    If pastGradientSquares(j).Count > 10 Then
                        pastGradientSquares(j).RemoveAt(0)
                    End If

                    'Update the sums
                    sumOfPastGradientSquares(j) = Sum(pastGradientSquares(j))

                Next
            Next

            'Multiply by Learning Rate for this specific feature and get new thetas
            For i = 0 To featureCount - 1
                'We are taking some stuff (the dividey bit) from adadelta bc it's pretty neat (faster???) 
                thetas(i) -= (learningRate(i) / (Math.Sqrt(sumOfPastGradientSquares(i) + 0.00000001))) * derivative(i)
            Next

            Dim currentCost = LogisticCost()
            deltaCost = currentCost - lastCost

            'We need to look like we're doing something so here's something to keep users occupied
            Console.WriteLine("Training...  " + currentCost.ToString + " " + derivative(0).ToString) ' + " " + derivative(1).ToString + " " + derivative(2).ToString)

            lastCost = currentCost

        End While

    End Sub

    'Set all the thetas to 0
    Sub InitialiseTheta()

        thetas = New List(Of Double)

        For i = 0 To featureCount - 1
            thetas.Add(0)
        Next

    End Sub

    Sub Load(path As String)

        Dim fileData As String 'To read files
        Dim rows As List(Of String)
        Dim values As List(Of Double)

        'We use Comma Separated Value files which basically means the data looks like
        ',1,2,3,4
        ',5,6,7,8
        ',9,0,1,2
        'and that we can parse using delimiter ','

        'Read the file and split it into rows
        Try

            fileData = My.Computer.FileSystem.ReadAllText(path)

        Catch ex As IO.IOException

            Console.WriteLine("Error opening file - you probably have it open somewhere else")
            Return

        End Try

        rows = fileData.Split(vbNewLine).ToList

        'Save Heading Data (the first row)
        Dim headingRow = rows(0).Split(",").ToList
        exampleHeadings.AddRange(headingRow)

        'Remove the first row (because we're done with it) as well as any blank rows
        rows.RemoveAt(0)
        rows.RemoveAll(Function(str) String.IsNullOrWhiteSpace(str))

        'The number of examples is now the number of rows
        exampleCount = rows.Count

        'The number of features is the number of columns minus one (since one column is output)
        featureCount = headingRow.Count - 1

        'Initialise the input list
        exampleInputs = New List(Of List(Of Double))(exampleCount)

        'We can therefore now loop over all the rows and add them to our example list
        For i = 0 To rows.Count - 1

            Try

                'Convert the array of strings given by splitting the row with a comma to a list of doubles
                values = New List(Of Double)(rows(i).Split(",").ToList.ConvertAll(AddressOf Double.Parse))

                exampleInputs.Add(values.GetRange(0, values.Count - 1))

                'The last column is the output
                exampleOutputs.Add(values(values.Count - 1))

            Catch

                'Just plough on if we have an error (yes. I'm soppy and depressed)
                Continue For

            End Try

        Next

        StandardiseFeatures()

    End Sub

    Sub StandardiseFeatures()

        'We want to scale all the features
        'Create feature vectors (with difficulty - there is an infinitely more efficient way to do this but it's midnight and school's tomorrow) 
        Dim features As New List(Of List(Of Double))

        For i = 0 To featureCount - 1
            Dim featureList As New List(Of Double)
            For j = 0 To exampleCount - 1
                featureList.Add(exampleInputs(j)(i))
            Next
            features.Add(featureList)
        Next

        'scale them
        For i = 0 To featureCount - 1
            'The Standardise function takes in a list and spurts out a StandardisedData object. Why? Because we want to be able to access the averages and standard deviations for prediction-making.
            Dim standardised As StandardisedData = Standardise(features(i))
            features(i) = standardised.list
            standardisedFeatureData.Add(standardised)
        Next

        'Send them back to the input list
        For i = 0 To featureCount - 1
            For j = 0 To exampleCount - 1
                exampleInputs(j)(i) = features(i)(j)
            Next
        Next

    End Sub

End Module
