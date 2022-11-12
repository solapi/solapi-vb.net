Module Program
    Sub Main(args As String())
        If args.Length <= 0 Then
            PrintHelp()
            Return
        End If
        Select Case args(0)
            Case "sms"
                SendSMS.Run()
                Return
            Case "lms"
                SendLMS.Run()
                Return
            Case "mms"
                SendMMS.Run()
                Return
            Case "alimtalk"
                SendAlimtalk.Run()
                Return
            Case "chingutalk"
                SendChingutalk.Run()
                Return
            Case "balance"
                GetBalance.Run()
                Return
            Case "scheduled"
                SendScheduledMessages.Run()
                Return
        End Select
        PrintHelp()
    End Sub

    Sub PrintHelp()
        Console.WriteLine("Nurigo [sms, lms, mms, alimtalk, chingutalk, balance] 형식으로 실행하세요(소문자 주의)")
    End Sub
End Module

