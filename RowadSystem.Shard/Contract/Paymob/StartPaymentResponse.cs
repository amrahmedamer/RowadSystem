using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RowadSystem.Shard.Contract.Paymob;
public sealed class StartPaymentResponse
{
    public string IframeUrl { get; set; } = string.Empty;   // for card
    public string? RedirectUrl { get; set; }                // for wallet
    public long OrderId { get; set; }
}

