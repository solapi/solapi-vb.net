Module GetBalance
    Sub Run()
        Dim response As MessagingLib.Response = MessagingLib.GetBalance()

        If response.StatusCode = Net.HttpStatusCode.OK Then
            Console.WriteLine("조회 결과")
            Console.WriteLine("현재 잔액: " & response.Data.SelectToken("balance").ToString)
            Console.WriteLine("현재 포인트: " & response.Data.SelectToken("point").ToString)

            ' Console.WriteLine(response.Data)
        Else
            Console.WriteLine("Error Code:" & response.ErrorCode)
            Console.WriteLine("Error Message:" & Response.ErrorMessage)
        End If
    End Sub
End Module
