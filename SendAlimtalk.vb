Imports System
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Module SendAlimtalk

    Sub Run()
        ' 한번 요청으로 1만건의 알림톡 발송이 가능합니다.
        ' 등록되어 있는 템플릿의 변수 부분을 제외한 나머지 부분(상수)은 100% 일치해야 합니다.
        ' 템플릿 내용이 "#{이름}님 가입을 환영합니다."으로 등록되어 있는 경우 변수 #{이름}을 홍길동으로 치환하여 "홍길동님 가입을 환영합니다."로 입력해 주세요.
        ' 버튼은 5개까지 입력 가능합니다.

        Dim messages As MessagingLib.Messages = New MessagingLib.Messages()

        ' 텍스트 내용만 있는 간단한 알림톡
        messages.Add(New MessagingLib.Message() With {
            .to = "01000000001",
            .from = "029302266",
            .text = "안녕하세요." & Environment.NewLine & "홍길동님 회원가입을 환영합니다.",
            .kakaoOptions = New MessagingLib.KakaoOptions() With {
                .pfId = "KA01PF190626020502205cl0mYSoplA2", ' 카카오톡채널 연동 후 발급받은 값을 사용해 주세요
                .templateId = "KA01TP190626032036196g86q1RGN7D1" ' 템플릿 등록 후 발급받은 값을 사용해 주세요
            }
        })

        ' 웹 링크 버튼이 하나 있는 알림톡
        messages.Add(New MessagingLib.Message() With {
            .to = "01000000002",
            .from = "029302266",
            .text = "안녕하세요." & Environment.NewLine & "홍길동님 회원가입을 환영합니다." & Environment.NewLine & "아래 '시작하기' 버튼을 통해 간단하게 사용방법을 익히실 수 있습니다.",
            .kakaoOptions = New MessagingLib.KakaoOptions() With {
                .pfId = "KA01PF190626020502205cl0mYSoplA2",
                .templateId = "KA01TP190626032036196g86q1RGN7D2",
                .buttons = {
                     New MessagingLib.KakaoButton() With {
                        .buttonType = "WL",
                        .buttonName = "시작하기",
                        .linkMo = "https://m.example.com"
                     }
                }
            }
        })
        ' 모든 종류의 버튼 예시
        messages.Add(New MessagingLib.Message() With {
            .to = "01000000003",
            .from = "029302266",
            .text = "안녕하세요." & Environment.NewLine & "홍길동님 회원가입을 환영합니다." & Environment.NewLine & "아래 다양한 형식의 버튼을 통해 사용방법을 익히실 수 있습니다.",
            .kakaoOptions = New MessagingLib.KakaoOptions() With {
                .pfId = "KA01PF190626020502205cl0mYSoplA2",
                .templateId = "KA01TP190626032036196g86q1RGN7D3",
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
                        .buttonType = "DS",
                        .buttonName = "배송조회"
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

