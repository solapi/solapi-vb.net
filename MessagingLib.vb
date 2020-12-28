Imports System
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Module MessagingLib
    Class Message
        Public type As String
        Public [to] As String
        Public [from] As String
        Public subject As String
        Public text As String
        Public imageId As String
        Public kakaoOptions As KakaoOptions
    End Class

    Class Messages
        Public messages As New List(Of Message)

        Sub Add(message As Message)
            messages.Add(message)
            ' Console.WriteLine(JsonConvert.SerializeObject(messages))
        End Sub
    End Class

    Class GroupInfo
        Public appId As String
        Public strict As Boolean
        Public sdkVersion As String
        Public osPlatform As String
    End Class

    Class Image
        Public type As String
        Public file As String
        Public name As String
        Public link As String
    End Class

    Class KakaoButton
        Public buttonType As String
        Public buttonName As String
        Public linkMo As String
        Public linkPc As String
        Public linkAnd As String
        Public linkIos As String
    End Class
    Class KakaoOptions
        Public pfId As String
        Public templateId As String
        Public disableSms As Boolean
        Public imageId As String
        Public buttons As KakaoButton()
    End Class

    Class Response
        Public StatusCode As Net.HttpStatusCode
        Public ErrorCode As String
        Public ErrorMessage As String
        Public Data As JObject
    End Class

    Dim JsonSettings As JsonSerializerSettings = New JsonSerializerSettings() With {
        .NullValueHandling = NullValueHandling.Ignore
    }

    Function GetSignature(apiKey As String, data As String, apiSecret As String)
        Dim sha256 As New Security.Cryptography.HMACSHA256(Text.Encoding.UTF8.GetBytes(apiSecret))
        Dim hashValue As Byte() = sha256.ComputeHash(Text.Encoding.UTF8.GetBytes(data))
        Dim hash As String = Replace(BitConverter.ToString(hashValue), "-", "")
        Return hash.ToLower
    End Function

    Function GetSalt(ByVal Optional len As Integer = 32)
        Dim s As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"
        Dim r As New Random
        Dim sb As New Text.StringBuilder
        For i As Integer = 1 To len
            Dim idx As Integer = r.Next(0, 35)
            sb.Append(s.Substring(idx, 1))
        Next
        Return sb.ToString()
    End Function
    Function GetAuth(apiKey As String, apiSecret As String)
        Dim salt As String = GetSalt()
        Dim dateStr As String = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
        Dim data As String = dateStr & salt

        Return "HMAC-SHA256 apiKey=" & apiKey & ", date=" & dateStr & ", salt=" & salt & ", signature=" & GetSignature(apiKey, data, apiSecret)
    End Function

    Function GetUrl(path)
        Return Config.protocol & "://" & Config.domain & path
    End Function

    Function Request(path As String, method As String, Optional data As String = vbNullString)
        Dim auth As String = GetAuth(Config.apiKey, Config.apiSecret)

        Try
            Dim req As Net.WebRequest = Net.WebRequest.Create(GetUrl(path))
            req.Method = method
            req.Headers.Add("Authorization", auth)
            req.Headers.Add("Content-type", "application/json; charset=utf-8")

            If Not String.IsNullOrEmpty(data) Then
                Using writer = New IO.StreamWriter(req.GetRequestStream())
                    writer.Write(data)
                    writer.Close()
                End Using
            End If

            Using response As Net.WebResponse = req.GetResponse()
                Using streamReader As IO.StreamReader = New System.IO.StreamReader(response.GetResponseStream())
                    Dim jsonResponseText = streamReader.ReadToEnd()
                    Dim jsonObj As JObject = JObject.Parse(jsonResponseText)
                    Return New Response() With {
                        .StatusCode = Net.HttpStatusCode.OK,
                        .Data = jsonObj,
                        .ErrorCode = vbNull,
                        .ErrorMessage = vbNull
                    }
                End Using
            End Using
        Catch ex As Net.WebException
            Using streamReader As IO.StreamReader = New System.IO.StreamReader(ex.Response.GetResponseStream())
                Dim jsonResponseText = streamReader.ReadToEnd()
                Dim jsonObj As JObject = JObject.Parse(jsonResponseText)
                Dim ErrorCode As String = jsonObj.SelectToken("errorCode")
                Dim ErrorMessage As String = jsonObj.SelectToken("errorMessage")
                Dim httpResp As Net.HttpWebResponse = DirectCast(ex.Response, Net.HttpWebResponse)
                Return New Response() With {
                        .StatusCode = httpResp.StatusCode,
                        .Data = jsonObj,
                        .ErrorCode = ErrorCode,
                        .ErrorMessage = ErrorMessage
                    }
            End Using
        Catch ex As Exception
            Dim ErrorCode As String = "Unknown Exception"
            Dim ErrorMessage As String = ex.Message

            Return New Response() With {
                        .StatusCode = vbNull,
                        .Data = vbNull,
                        .ErrorCode = ErrorCode,
                        .ErrorMessage = ErrorMessage
                    }
        End Try

    End Function

    Class Group
        Dim groupId As String

        Public Sub New()
            ' Nothing
        End Sub


        Public Sub New(groupId As String)
            Me.groupId = groupId
        End Sub

        Function AddMessages(msgs As Messages)
            If String.IsNullOrEmpty(groupId) Then
                Throw New System.Exception("그룹아이디가 설정되지 않았습니다.")
            End If
            Return Request("/messages/v4/groups/" & groupId & "/messages", "POST", JsonConvert.SerializeObject(msgs, Formatting.None, JsonSettings))
        End Function

        Function Create()
            Dim groupInfo As GroupInfo = New GroupInfo() With {
                .osPlatform = Environment.OSVersion.VersionString,
                .sdkVersion = "VB.NET v1"
            }
            Return Request("/messages/v4/groups", "POST", JsonConvert.SerializeObject(groupInfo, Formatting.None, JsonSettings))
        End Function

        Function GetList()
            Return Request("/messages/v4/groups", "GET")
        End Function
    End Class


    Function SendMessages(messages As Messages)
        Return Request("/messages/v4/send-many", "POST", JsonConvert.SerializeObject(messages, Formatting.None, JsonSettings))
    End Function

    Function UploadImage(path As String)
        Dim bytes As Byte() = IO.File.ReadAllBytes(path)
        Dim img As Image = New Image()
        img.type = "MMS"
        img.file = Convert.ToBase64String(bytes)
        Return Request("/storage/v1/files", "POST", JsonConvert.SerializeObject(img, Formatting.None, JsonSettings))
    End Function

    Function UploadKakaoImage(path As String, url As String)
        Dim bytes As Byte() = IO.File.ReadAllBytes(path)
        Dim img As Image = New Image()
        img.type = "KAKAO"
        img.file = Convert.ToBase64String(bytes)
        img.name = IO.Path.GetFileName(path)
        img.link = url
        Return Request("/storage/v1/files", "POST", JsonConvert.SerializeObject(img, Formatting.None, JsonSettings))
    End Function

    Function GetBalance()
        Return Request("/cash/v1/balance", "GET")
    End Function

    Sub GetGroupList()

        Request("/messages/v4/groups", "GET")

    End Sub
End Module
