Imports System
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Module SendSMS

    Sub Run()
        ' TextingLib.GetGroupList()
        ' Dim group = New TextingLib.Group()
        ' Console.WriteLine(group.GetList())

        Dim messages As MessagingLib.Messages = New MessagingLib.Messages()
        messages.Add(New MessagingLib.Message() With {
            .to = "01000000001",
            .from = "029302266",
            .text = "한글 45자, 영자 90자 이하 입력되면 자동으로 SMS타입의 메시지가 추가됩니다."
        })
        messages.Add(New MessagingLib.Message() With {
            .to = "01000000002",
            .from = "029302266",
            .text = "한글 45자, 영자 90자 이상 입력되면 자동으로 LMS타입의 문자메시자가 발송됩니다. 0123456789 ABCDEFGHIJKLMNOPQRSTUVWXYZ"
        })
        ' 타입을 명시할 경우 text 길이가 한글 45 혹은 영자 90자를 넘을 경우 오류가 발생합니다.
        messages.Add(New MessagingLib.Message() With {
            .to = "01000000003",
            .from = "029302266",
            .text = "SMS 타입에 한글 45자, 영자 90자 이상 입력되면 오류가 발생합니다. 0123456789 ABCDEFGHIJKLMNOPQRSTUVWXYZ"
        })

        ' 1만건까지 추가 가능

        Dim response As MessagingLib.Response = MessagingLib.SendMessages(messages)
        If response.StatusCode = Net.HttpStatusCode.OK Then
            Console.WriteLine("전송 결과")
            Console.WriteLine("Group ID:" & response.Data.SelectToken("groupId").ToString)
            Console.WriteLine("Status:" & response.Data.SelectToken("status").ToString)
            Console.WriteLine("Count:" & response.Data.SelectToken("count").ToString)
            ' Console.WriteLine(response.Data)
        Else
            Console.WriteLine("Error Code:" & response.ErrorCode)
            Console.WriteLine("Error Message:" & response.ErrorMessage)
        End If
    End Sub
End Module