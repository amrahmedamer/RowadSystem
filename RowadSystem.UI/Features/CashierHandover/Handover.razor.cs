using Microsoft.AspNetCore.Components;
using RowadSystem.Shard.Contract.Cashiers;

namespace RowadSystem.UI.Features.CashierHandover;

public partial class Handover
{
    [Inject]
    private ICashierHandoverService _cashierHandoverService { get; set; } = default!;
    private CashierHandoverDTO handoverDTO = new CashierHandoverDTO();

    private async Task HandleSubmit()
    {
        var response = await _cashierHandoverService.CashierHandoverAsync(handoverDTO);

        if (response.IsSuccess)
        {
            Console.WriteLine("تم تسليم الورديّة بنجاح");
        }
        else
        {
            Console.WriteLine("حدث خطأ أثناء تسليم الورديّة");
        }
    }
}
