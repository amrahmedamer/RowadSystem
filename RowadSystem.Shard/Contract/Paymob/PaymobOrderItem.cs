using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RowadSystem.Shard.Contract.Paymob;
public class PaymobOrderItem
{
    public string ItemId { get; set; } // معرف المنتج
    public string ItemName { get; set; } // اسم المنتج
    public int Quantity { get; set; } // الكمية المطلوبة
    public int PriceCents { get; set; } = 0; // سعر المنتج بالـ سنت
}

