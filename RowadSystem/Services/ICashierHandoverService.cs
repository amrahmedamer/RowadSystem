namespace RowadSystem.API.Services;

public interface ICashierHandoverService
{
    //Task<bool> HandoverCashierAsync(string cashierId, decimal amountInDrawer);
        Task<Result> HandoverCashierAsync(string cashierId, decimal amountInDrawer);
}
