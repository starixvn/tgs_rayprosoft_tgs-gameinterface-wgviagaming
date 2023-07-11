namespace Tgs.GameInterface.WGViaGaming.Core.Exceptions;

public class ReturnCodeException : Exception
{
    public ReturnCodeException(string returnCode) : base(ConvertErrorMesaage(returnCode))
    {
    }

    private static string ConvertErrorMesaage(string returnCode) => returnCode switch
    {
        "101" => "請求過期，原因是當前時間與解密 Hash 後得到的時間間隔太久。",
        "103" => "貨幣未正確，只能是 HKD 或者 RMB",
        "104" => "錯誤的合作公司標誌,即無效的 PartnerCode",
        "105" => "無效的 hash, 注意：hash 當 url 參數時,需要加 urlencode()",
        "106" => "用戶唯一標誌為空",
        "198" => "用戶唯一標誌為空",
        "199" => "內部錯誤：創建用戶失敗",
        "201" => "Token 解密異常",
        "202" => "Token 內部數據異常",
        "203" => "無效的 Lang 類型",
        "298" => "內部錯誤：未分配代理",
        "299" => "內部錯誤：創建用戶失敗",
        "301" => "無效 Hash",
        "302" => "無效用戶",
        "303" => "無效金額",
        "305" => "訂單號重複",
        "398" => "內部錯誤：代理非現金帳戶",
        "399" => "內部錯誤：代理現金不足",
        "397" => "內部錯誤：會員現金不足",
        "401" => "無效 Hash",
        "402" => "平台向站內存取款的訂單未生效",
        "501" => "無效 Hash",
        "502" => "無效用戶",
        "503" => "無效日期",
        "601" => "無效 Hash",
        "602" => "無效用戶",
        _ => $"遊戲供應商未提供錯誤代碼{returnCode}"
    };
}
