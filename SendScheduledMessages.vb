Imports System
Imports System.Globalization
Imports System.Threading
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Module SendScheduledMessages
    Sub Run()
        Dim messages = New MessagesDetail()
        ' 중복 수신번호를 허용하시려면 아래 주석을 해제 해주세요!
        ' messages.allowDuplicates = true

        ' 문자 발송 시..
'        messages.Add(New Message() With {
'                        .to = "보낼 전화번호(수신번호) 입력",
'                        .from = "계정에서 등록한 발신번호 입력",
'                        .text = "한글 45자, 영자 90자 이상 입력되면 자동으로 LMS타입의 문자메시자가 발송됩니다. 0123456789 ABCDEFGHIJKLMNOPQRSTUVWXYZ"
'                        })

        ' 알림톡 발송 시..
        Dim variables = New Dictionary(Of String, String)

        ' 템플릿 안에 치환문구(#{변수명} 형식)가 있다면 아래 주석을 해제해주세요!
        ' 반드시 키 값은 템플릿 내에 등록한 치환문구를 그대로 적어주셔야 합니다, 치환문구 수 만큼 variables.Add를 추가해주세요! 
        ' variables.Add("#{변수명}", "테스트123")

        messages.Add(New Message() With {
                        .to = "보낼 전화번호(수신번호) 입력",
                        .kakaoOptions = New KakaoOptions() With {
                        .pfId = "연동한 비즈니스 채널의 pfId(당사 관리콘솔 '카카오채널(플러스친구)'에서 확인 가능)",
                        .templateId = "등록한 알림톡 템플릿의 ID(당사 관리콘솔 '카카오 알림톡 템플릿'에서 확인 가능)",
                        .variables = variables
                        }
                        })

        ' 1만건까지 추가 가능
        ' 예약발송을 희망하는 경우 아래 주석을 해제 해보세요. 단, 과거시간으로 예약 발송을 진행하면 즉시 발송처리 됩니다!
        ' messages.scheduledDate = DateTime.parse("2022-11-12 19:00:00").AddHours(- 9).ToString("o")

        Dim response As Response = SendMessagesDetail(messages)
        If response.StatusCode = Net.HttpStatusCode.OK Then
            Console.WriteLine("전송 결과")
            Console.WriteLine("접수 정보:" & response.Data.SelectToken("groupInfo").ToString)
            Console.WriteLine("접수에 실패한 메시지 정보:" & response.Data.SelectToken("failedMessageList").ToString)
        Else
            Console.WriteLine("Error Code:" & response.ErrorCode)
            Console.WriteLine("Error Message:" & response.ErrorMessage)
        End If
    End Sub
End Module

