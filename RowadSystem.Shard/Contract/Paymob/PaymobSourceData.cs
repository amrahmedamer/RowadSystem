using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RowadSystem.Shard.Contract.Paymob;
public sealed class PaymobSourceData { 
    public string type { get; set; } = "";
    public string sub_type { get; set; } = ""; 
    public string pan { get; set; } = ""; 
}
