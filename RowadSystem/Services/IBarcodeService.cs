namespace RowadSystem.Services;

public interface IBarcodeService
{
    Task<byte[]> GenerateBarcodeAsync(string input);
    //Task<byte[]> GenerateBarcodeAsync(string input, int quantity);
    //Task<bool> PrintBarcodeAsync(string input, int quantity);
    

    }
