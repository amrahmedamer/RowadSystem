namespace RowadSystem.Helpers;

public static class Ean13Generator
{
    private const string Prefix = "116"; // Prefix

    public static string Generate(int productId)
    {
        string body = productId.ToString().PadLeft(9, '0');
        string codeWithoutCheck = Prefix + body;
        int checkDigit = CalculateCheckDigit(codeWithoutCheck);
        return codeWithoutCheck + checkDigit;
    }

    private static int CalculateCheckDigit(string code12)
    {
        int sum = 0;
        for (int i = 0; i < code12.Length; i++)
        {
            int digit = code12[i] - '0';
            sum += (i % 2 == 0) ? digit : digit * 3;
        }

        int remainder = sum % 10;
        return remainder == 0 ? 0 : 10 - remainder;
    }
}
