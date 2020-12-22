Imports System
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Module SendGroupMessage

    Sub Run()
        'Dim imgResp As TextingLib.Response = TextingLib.UploadImage("testImage.jpg")
        'Dim imageId As String = imgResp.Data.SelectToken("fileId").ToString()
        'Console.WriteLine("Image ID:" & imageId)

        Dim group As MessagingLib.Group = New MessagingLib.Group()
        Console.WriteLine(group.Create())
    End Sub
End Module

