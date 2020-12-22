Imports System
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Module SendChingutalk

    Sub Run()
        ' 한번 요청으로 1만건의 친구톡 발송이 가능합니다.
        ' 카카오톡채널 친구로 추가되어 있어야 친구톡 발송이 가능합니다.
        ' 템플릿 등록없이 버튼을 포함하여 자유롭게 메시지 전송이 가능합니다.
        ' 버튼은 5개까지 입력 가능합니다.
        ' 버튼 종류(AL: 앱링크, WL: 웹링크, BK: 키워드, MD: 전달)

        Dim messages As MessagingLib.Messages = New MessagingLib.Messages()

        ' 텍스트 내용만 있는 간단한 알림톡
        messages.Add(New MessagingLib.Message() With {
            .to = "01000000001",
            .from = "029302266",
            .text = "광고를 포함하여 어떤 내용이든 입력 가능합니다.",
            .kakaoOptions = New MessagingLib.KakaoOptions() With {
                .pfId = "KA01PF190626020502205cl0mYSoplA2" ' 카카오톡채널 연동 후 발급받은 값을 사용해 주세요
            }
        })

        ' 친구톡 이미지 업로드
        Dim imgResp As MessagingLib.Response = MessagingLib.UploadKakaoImage("testImage.jpg", "https://example.com") ' 파일명, 이미지 클릭 시 이동할 링크 URL
        Dim imageId As String = imgResp.Data.SelectToken("fileId").ToString()

        ' 친구톡 이미지 발송
        messages.Add(New MessagingLib.Message() With {
            .to = "01000000002",
            .from = "029302266",
            .text = "광고를 포함하여 어떤 내용이든 입력 가능합니다.",
            .kakaoOptions = New MessagingLib.KakaoOptions() With {
                .pfId = "KA01PF190626020502205cl0mYSoplA2", ' 카카오톡채널 연동 후 발급받은 값을 사용해 주세요
                .imageId = imageId
            }
        })

        ' 모든 종류의 버튼 예시
        messages.Add(New MessagingLib.Message() With {
            .to = "01000000003",
            .from = "029302266",
            .text = "광고를 포함하여 어떤 내용이든 입력 가능합니다.",
            .kakaoOptions = New MessagingLib.KakaoOptions() With {
                .pfId = "KA01PF190626020502205cl0mYSoplA2",
                .buttons = {
                     New MessagingLib.KakaoButton() With {
                        .buttonType = "WL",
                        .buttonName = "시작하기",
                        .linkMo = "https://m.example.com",
                        .linkPc = "https://example.com"
                     },
                     New MessagingLib.KakaoButton() With {
                        .buttonType = "AL",
                        .buttonName = "앱 실행",
                        .linkAnd = "examplescheme://",
                        .linkIos = "examplescheme://"
                     },
                     New MessagingLib.KakaoButton() With {
                        .buttonType = "BK", ' 챗봇에게 키워드를 전달합니다. 버튼이름의 키워드가 그대로 전달됩니다.
                        .buttonName = "봇키워드"
                     },
                     New MessagingLib.KakaoButton() With {
                        .buttonType = "MD", ' 상담요청하기 버튼을 누르면 수신 받은 알림톡 메시지가 상담원에게 그대로 전달됩니다.
                        .buttonName = "상담요청하기"
                     }
                }
            }
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

