# Visual Basic .NET용 메시지 발송 SDK

## 사용방법

* Config.vb 와 MessagingLib.vb 파일을 프로젝트로 복사하여 사용합니다.
* Config.vb에 API Key와 API Secret이 정상적으로 입력되어 있는지 확인해 주세요.
* Program.vb 예제 메인 모듈을 참고하세요.

## 예제

```
SendSMS.vb                  SMS 발송 예제
SendLMS.vb                  LMS 발송 예제
SendMMS.vb                  MMS 발송 예제
SendAlimtalk.vb             알림톡 발송 예제
SendChingutalk.vb           친구톡 발송 예제
GetBalance.vb               잔액 조회 예제
SendScheduledMessages.vb    예약 발송 예제(예약 발송은 모든 발송 유형에 대응됩니다.)
```

## 예제 실행 방법

아래 형식으로 실행할 수 있으며, 소문자에 주의해 주세요.  
또한, 실행 시 [] 등의 괄호는 제거한 목록의 유형 중 한 가지 만으로 실행해야 합니다.

```
Messaging.exe [sms, lms, mms, alimtalk, chingutalk, balance]

실제 실행시 -> Messaging.exe sms
```
