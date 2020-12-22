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
            .text = "�ѱ� 45��, ���� 90�� ���� �ԷµǸ� �ڵ����� SMSŸ���� �޽����� �߰��˴ϴ�."
        })
        messages.Add(New MessagingLib.Message() With {
            .to = "01000000002",
            .from = "029302266",
            .text = "�ѱ� 45��, ���� 90�� �̻� �ԷµǸ� �ڵ����� LMSŸ���� ���ڸ޽��ڰ� �߼۵˴ϴ�. 0123456789 ABCDEFGHIJKLMNOPQRSTUVWXYZ"
        })
        ' Ÿ���� ����� ��� text ���̰� �ѱ� 45 Ȥ�� ���� 90�ڸ� ���� ��� ������ �߻��մϴ�.
        messages.Add(New MessagingLib.Message() With {
            .to = "01000000003",
            .from = "029302266",
            .text = "SMS Ÿ�Կ� �ѱ� 45��, ���� 90�� �̻� �ԷµǸ� ������ �߻��մϴ�. 0123456789 ABCDEFGHIJKLMNOPQRSTUVWXYZ"
        })

        ' 1���Ǳ��� �߰� ����

        Dim response As MessagingLib.Response = MessagingLib.SendMessages(messages)
        If response.StatusCode = Net.HttpStatusCode.OK Then
            Console.WriteLine("���� ���")
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