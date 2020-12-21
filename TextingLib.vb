Imports System
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Module TextingLib
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

    Function GetAuth(apiKey As String, apiSecret As String)
        Dim salt As String = "abcdefghijklmn01234"
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
                    Console.WriteLine(data)
                    writer.Write(data)
                    writer.Close()
                End Using
            End If

            Using response As Net.WebResponse = req.GetResponse()
                Using streamReader As IO.StreamReader = New System.IO.StreamReader(response.GetResponseStream())
                    Dim jsonResponseText = streamReader.ReadToEnd()
                    Dim jsonObj As JObject = JObject.Parse(jsonResponseText)
                    ' Console.WriteLine(jsonObj.SelectToken("groupList").ToString)
                    Return New Response() With {
                        .StatusCode = Net.HttpStatusCode.OK,
                        .Data = jsonObj,
                        .ErrorCode = vbNull,
                        .ErrorMessage = vbNull
                    }
                End Using
            End Using
        Catch ex As Net.WebException
            Console.WriteLine("WebException:" & ex.Message)
            Using streamReader As IO.StreamReader = New System.IO.StreamReader(ex.Response.GetResponseStream())
                Dim jsonResponseText = streamReader.ReadToEnd()
                Console.WriteLine(jsonResponseText)
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
            Console.WriteLine(ex.Message)
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
        Console.WriteLine("SendMessages")
        ' Console.WriteLine(JsonConvert.SerializeObject(messages))
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


    Sub GetGroupList()

        Request("/messages/v4/groups", "GET")

    End Sub

    Public Function GetOSVersion() As String
        Select Case Environment.OSVersion.Platform
            Case PlatformID.Win32S
                Return "Win 3.1"
            Case PlatformID.Win32Windows
                Select Case Environment.OSVersion.Version.Minor
                    Case 0
                        Return "Win95"
                    Case 10
                        Return "Win98"
                    Case 90
                        Return "WinME"
                    Case Else
                        Return "Unknown"
                End Select
            Case PlatformID.Win32NT
                Select Case Environment.OSVersion.Version.Major
                    Case 3
                        Return "NT 3.51"
                    Case 4
                        Return "NT 4.0"
                    Case 5
                        Select Case _
                            Environment.OSVersion.Version.Minor
                            Case 0
                                Return "Win2000"
                            Case 1
                                Return "WinXP"
                            Case 2
                                Return "Win2003"
                        End Select
                    Case 6
                        Select Case _
                            Environment.OSVersion.Version.Minor
                            Case 0
                                Return "Vista/Win2008Server"
                            Case 1
                                Return "Win7/Win2008Server R2"
                            Case 2
                                Return "Win8/Win2012Server"
                            Case 3
                                Return "Win8.1/Win2012Server R2"
                        End Select
                    Case 10 ' this will only show up If the application has a manifest file allowing W10, otherwise a 6.2 version will be used
                        Return "Windows 10"
                    Case Else
                        Return "Unknown"
                End Select
            Case PlatformID.WinCE
                Return "Win CE"
        End Select
        Return "Unknown"
    End Function
End Module
