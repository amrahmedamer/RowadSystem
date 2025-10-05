namespace RowadSystem.Shard.consts.Enums;

public enum OrderStatus
{
    Pending,  // في انتظار الدفع
    Paid,     // تم الدفع بنجاح
    Shipped,  // تم شحنه
    Delivered, // تم تسليمه
    Canceled  // تم إلغاء الطلب
}
