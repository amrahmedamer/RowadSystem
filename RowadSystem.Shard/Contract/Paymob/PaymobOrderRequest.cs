using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RowadSystem.Shard.Contract.Paymob;
public class PaymobOrderRequest
{
    public int AmountCents { get; set; } // المبلغ بالـ سنت (100 = 1 جنيه مصري)
    public string Currency { get; set; } // العملة، مثلا "EGP" للجنيه المصري
    public string OrderId { get; set; }  // معرف الطلب (ممكن تستخدمه لتتبع الطلب)
    public string MerchantOrderId { get; set; } // معرف الطلب الخاص بالتاجر (اختياري)
    public List<PaymobOrderItem> Items { get; set; } // قائمة بالمنتجات اللي تم طلبها
    public string CallbackUrl { get; set; } // رابط الـ Webhook الخاص بالتاجر لتحديث حالة الدفع
    public string IntegrationId { get; set; } // الـ Integration ID الخاص بـ Paymob
}