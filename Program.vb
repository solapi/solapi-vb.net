Module Program

    Sub Main(args As String())
        ReDim args(1)
        args(0) = "group"
        If args.Length <= 0 Then
            Console.WriteLine("sms: SMS 예제 실행, lms: LMS 예제 실행, ...")
            Return
        End If
        Select Case args(0)
            Case "sms"
                SendSMS.Run()
            Case "lms"
                SendLMS.Run()
            Case "mms"
                SendMMS.Run()
            Case "alimtalk"
                SendAlimtalk.Run()
            Case "chingutalk"
                SendChingutalk.Run()
            Case "group"
                SendGroupMessage.Run()
        End Select
    End Sub
End Module

