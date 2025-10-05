using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RowadSystem.Shard.Contract.Paymob;
public sealed class PaymobWebhookPayload
{
    public string hmac { get; set; } = "";
    public PaymobOrder order { get; set; } = new();
    public bool success { get; set; }
    public int id { get; set; }
    public int integration_id { get; set; }
    public string currency { get; set; } = "";
    public string amount_cents { get; set; } = "";
    public PaymobSourceData source_data { get; set; } = new();
    public bool pending { get; set; }
    // Add other fields you need
}
