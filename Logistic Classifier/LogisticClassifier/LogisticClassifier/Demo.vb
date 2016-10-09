Module Demo

    'This serves as the UI for the demo

    Sub Main()

        Console.WriteLine("                                    Peter's Late Night Work Club Presents")
        Console.WriteLine("
                 _                 _     _   _        _____ _               _  __ _            *
                | |               (_)   | | (_)      /  __ \ |             (_)/ _(_)           
                | |     ___   __ _ _ ___| |_ _  ___  | /  \/ | __ _ ___ ___ _| |_ _  ___ _ __  
                | |    / _ \ / _` | / __| __| |/ __| | |   | |/ _` / __/ __| |  _| |/ _ \ '__| 
                | |___| (_) | (_| | \__ \ |_| | (__  | \__/\ | (_| \__ \__ \ | | | |  __/ |    
                \_____/\___/ \__, |_|___/\__|_|\___|  \____/_|\__,_|___/___/_|_| |_|\___|_|    
                              __/ |                                                            
                             |___/                                                             ")

        Console.WriteLine()
        Console.WriteLine("*Binary bc I'm lazy")
        Console.WriteLine()
        Console.WriteLine("Type a path or pick a demo (press 1,2,3...)")
        Console.WriteLine()
        Console.WriteLine("Demo List:
[1] Predicts sex based upon physical characteristics.
[2] Predicts whether a student passes their exams based upon revision load and whether they're in a couple.
[3] Predicts whether your car will crash based upon age, tyres and weather.
[4] Predicts whether you're attracted to guys based upon your sex (please don't get triggered I needed to demo a very basic thing)
[5] A random sales dataset I downloaded off the internet. I have no idea what it does but it's something to do with buying magazines.")


        'Load and initialise stuff

        'Let the user choose what to load
        Dim demoChoice As String = ""

        While demoChoice = ""

            demoChoice = Console.ReadLine()
            If demoChoice = "1" Then
                Load(AppDomain.CurrentDomain.BaseDirectory + "/data_sex.csv")
            ElseIf demoChoice = "2" Then
                Load(AppDomain.CurrentDomain.BaseDirectory + "/data_exams.csv")
            ElseIf demoChoice = "3" Then
                Load(AppDomain.CurrentDomain.BaseDirectory + "/data_crashes.csv")
            ElseIf demoChoice = "4" Then
                Load(AppDomain.CurrentDomain.BaseDirectory + "/data_dating.csv")
            ElseIf demoChoice = "5" Then
                Load(AppDomain.CurrentDomain.BaseDirectory + "/data_sales.csv")
            ElseIf demoChoice.Contains(".csv") Then
                'We are probably dealing with a path???
                Load(demoChoice)
            End If

        End While

        InitialiseTheta()


        'Ask the user how accurate they want it
        While desiredDelta = 0
            Try
                Console.WriteLine("Delta Cost Threshold (the smaller, the more accurate - try 0.0001ish): ")
                desiredDelta = Console.ReadLine()
            Catch ex As Exception
                Continue While
            End Try
        End While

        'Get rid of the intro screen
        Console.Clear()


        'Minimise the cost by messing with theta
        Minimise()

        'Display the final cost
        Console.WriteLine("Final Cost: " + LogisticCost().ToString)
        Console.Write("Press any key to continue")
        Console.ReadKey()
        Console.Clear()

        'Test the hypothesis against the training set. You could implement something to get use a cross-validation set too.
        Check()
        Console.Write("Press any key to continue")
        Console.ReadKey()
        Console.Clear()

        'Predict stuff (user inputs data, regression works out what the output is likely to be)
        While True

            Dim inputs As New List(Of Double)

            'Loop through all the features (except for the y)
            For i = 0 To featureCount - 1
                Try
                    Console.WriteLine(exampleHeadings(i) + ": ")
                    Dim rawInput As Double = Convert.ToDouble(Console.ReadLine())
                    Dim normalisedInput As Double = ((rawInput - standardisedFeatureData(i).average) / standardisedFeatureData(i).standardDeviation)
                    inputs.Add(normalisedInput)
                Catch ex As FormatException
                    i -= 1
                    Continue For
                End Try
            Next

            'Give a prediction
            If Predict(inputs) > 0.5 Then
                Console.WriteLine("Positive")
            Else
                Console.WriteLine("Negative")
            End If

            Console.WriteLine("Press any key to continue or hit ctrl + c to exit")
            Console.ReadKey()
            Console.Clear()

        End While


    End Sub

End Module
