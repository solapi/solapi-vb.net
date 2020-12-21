﻿Imports System
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Module SendLMS

    Sub Run()
        ' TextingLib.GetGroupList()
        ' Dim group = New TextingLib.Group()
        ' Console.WriteLine(group.GetList())

        Dim messages As TextingLib.Messages = New TextingLib.Messages()
        messages.Add(New TextingLib.Message() With {
            .to = "01000000001",
            .from = "029302266",
            .text = "한글 45자, 영자 90자 이상 입력되면 자동으로 LMS타입의 문자메시자가 발송됩니다. 0123456789 ABCDEFGHIJKLMNOPQRSTUVWXYZ"
        })
        messages.Add(New TextingLib.Message() With {
            .to = "01000000002",
            .from = "029302266",
            .subject = "LMS 제목", ' 제목을 지정할 수 있습니다.
            .text = "한글 45자, 영자 90자 이상 입력되면 자동으로 LMS타입의 문자메시자가 발송됩니다. 0123456789 ABCDEFGHIJKLMNOPQRSTUVWXYZ"
        })
        messages.Add(New TextingLib.Message() With {
            .type = "LMS", ' 타입을 명시할 수 있습니다.
            .to = "01000000003",
            .from = "029302266",
            .text = "내용이 짧아도 LMS로 발송됩니다."
        })
        messages.Add(New TextingLib.Message() With {
            .to = "01000000004",
            .from = "029302266",
            .text = "한글 45자, 영자 90자 이하는 자동으로 SMS타입의 문자가 발송됩니다."
        })

        ' 1만건까지 추가 가능

        Dim response As TextingLib.Response = TextingLib.SendMessages(messages)
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

