using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RowadSystem.Shard.Contract.Paymob;
public class PaymobOrderResponse
{
    public string OrderId { get; set; }    // ID الخاص بالطلب
    public int AmountCents { get; set; }   // المبلغ بالـ سنت (يمكنك تحويله إلى قيمة منطقية)
    public string Currency { get; set; }   // العملة (مثل "EGP" للجنيه المصري)
    public string Status { get; set; }     // حالة الطلب (مثال: "pending", "completed")
    public string PaymentKey { get; set; } // Payment key لو كنت هتستخدم iframe أو redirect للدفع
    public string ErrorMessage { get; set; } // لو في خطأ رجع من Paymob، هتكون هنا
    public string ErrorCode { get; set; }   // الكود بتاع الخطأ لو موجود
    public string IframeUrl { get; set; }   // الكود بتاع الخطأ لو موجود
}
