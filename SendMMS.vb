Imports System
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Module SendMMS

    Sub Run()
        Dim imgResp As MessagingLib.Response = MessagingLib.UploadImage("testImage.jpg")
        Dim imageId As String = imgResp.Data.SelectToken("fileId").ToString()

        Console.WriteLine(imageId)

        Dim messages As MessagingLib.Messages = New MessagingLib.Messages()

        messages.Add(New MessagingLib.Message() With {
            .to = "01000000001",
            .from = "029302266",
            .imageId = imageId,
            .subject = "MMS 제목",
            .text = "이미지 아이디가 입력되면 MMS로 발송됩니다."
        })
        messages.Add(New MessagingLib.Message() With {
            .to = "01000000002",
            .from = "029302266",
            .imageId = imageId,
            .subject = "MMS 제목",
            .text = "동일한 이미지 아이디가 입력되면 동일한 이미지가 MMS로 발송됩니다."
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

