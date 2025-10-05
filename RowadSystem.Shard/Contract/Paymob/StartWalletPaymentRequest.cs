using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RowadSystem.Shard.Contract.Paymob;
public sealed class StartWalletPaymentRequest : StartPaymentRequest
{
    public string WalletMsisdn { get; set; } = "01000000000";
}
