//using BarcodeStandard;
//using SkiaSharp;

//namespace RowadSystem.Services;

//public class BarcodeService : IBarcodeService
//{
//    public async Task<string> GenerateBarcodeAsync(string input)
//    {
//        var barcode = new Barcode();

//        var foreground = new SKColorF(0, 0, 0);   // back
//        var background = new SKColorF(1, 1, 1);   // white
//        // توليد الباركود من نوع Code128
//        var image = barcode.Encode(BarcodeStandard.Type.Code39, input, foreground, background, 300, 150);


//        using var ms = new MemoryStream();
//        image.Save
//    }
//}


using BarcodeStandard;
using SkiaSharp;
using System.Drawing.Printing;
using System.Net.Sockets;
using System.Text;
using Type = BarcodeStandard.Type;

namespace RowadSystem.Services;

public class BarcodeService : IBarcodeService
{
    public async Task<byte[]> GenerateBarcodeAsync(string input)
    {
        var barcode = new Barcode();

        var foreground = new SKColorF(0, 0, 0);   // black
        var background = new SKColorF(1, 1, 1);   // white

        using var image = barcode.Encode(Type.Code39Extended, input, foreground, background, 250, 250);

        using var data = image.Encode(SKEncodedImageFormat.Png, 100);

        using var ms = new MemoryStream();
        data.SaveTo(ms);
        ms.Position = 0;

        return ms.ToArray();
    }


    //public async Task<byte[]> GenerateBarcodeAsync(string input, int quantity)
    //{
    //    var barcode = new Barcode();

    //    var foreground = new SKColorF(0, 0, 0);   // black
    //    var background = new SKColorF(1, 1, 1);   // white

    //    using var image = barcode.Encode(Type.Code39Extended, input, foreground, background, 250, 250);
    //    using var data = image.Encode(SKEncodedImageFormat.Png, 100);

    //    using var ms = new MemoryStream();
    //    data.SaveTo(ms);
    //    ms.Position = 0;

    //    var barcodeBytes = ms.ToArray();

    //    // تكرار الباركود بناءً على الكمية
    //    return RepeatBarcode(barcodeBytes, quantity);
    //}

    //private byte[] RepeatBarcode(byte[] barcodeBytes, int quantity)
    //{
    //    using var ms = new MemoryStream();
    //    for (int i = 0; i < quantity; i++)
    //    {
    //        ms.Write(barcodeBytes, 0, barcodeBytes.Length); // كتابة الباركود عدة مرات
    //    }

    //    return ms.ToArray(); // إرجاع البيانات المتكررة
    //}
}


//using BarcodeStandard;
//using SkiaSharp;
//using System;
//using System.Drawing.Printing;
//using System.IO;
//using System.Text;
//using Type = BarcodeStandard.Type;

//public class BarcodeService : IBarcodeService
//{
//    public async Task<bool> PrintBarcodeAsync(string input, int quantity)
//    {
//        // توليد الباركود وتكراره حسب الكمية
//        var barcodeBytes = await GenerateBarcodeAsync(input, quantity);

//        // الحصول على اسم الطابعة المتصلة
//        string printerName = GetConnectedPrinter();
//        if (string.IsNullOrEmpty(printerName))
//        {
//            Console.WriteLine("لم يتم العثور على طابعة متصلة.");
//            return false;
//        }

//        // إرسال البيانات للطابعة
//        return SendRawDataToPrinter(printerName, barcodeBytes);
//    }

//    public async Task<byte[]> GenerateBarcodeAsync(string input, int quantity)
//    {
//        var barcode = new Barcode();
//        var foreground = new SKColorF(0, 0, 0);   // black
//        var background = new SKColorF(1, 1, 1);   // white

//        using var image = barcode.Encode(Type.Code39Extended, input, foreground, background, 300, 150);
//        using var data = image.Encode(SKEncodedImageFormat.Png, 100);

//        using var ms = new MemoryStream();
//        data.SaveTo(ms);
//        ms.Position = 0;

//        var barcodeBytes = ms.ToArray();

//        // تكرار الباركود بناءً على الكمية
//        return RepeatBarcode(barcodeBytes, quantity);
//    }

//    private byte[] RepeatBarcode(byte[] barcodeBytes, int quantity)
//    {
//        using var ms = new MemoryStream();
//        for (int i = 0; i < quantity; i++)
//        {
//            ms.Write(barcodeBytes, 0, barcodeBytes.Length); // كتابة الباركود عدة مرات
//        }
//        return ms.ToArray(); // إرجاع البيانات المتكررة
//    }


//    private string GetConnectedPrinter()
//    {
//        // اكتشاف الطابعات المتصلة
//        foreach (string printer in PrinterSettings.InstalledPrinters)
//        {
//            // التحقق من الطابعة التي تحتوي على "POS" أو اسم الطابعة المطلوبة
//            if (printer.ToLower().Contains("xprinter".ToLower())) // تأكد من تغيير هذا إلى اسم الطابعة الفعلي
//            {
//                return printer;
//            }
//        }
//        return null;
//    }

//    private bool SendRawDataToPrinter(string printerName, byte[] barcodeData)
//    {
//        try
//        {
//            // إرسال البيانات للطابعة باستخدام RawPrinterHelper أو FileStream
//            using (var fs = new FileStream(@"\\.\\" + printerName, FileMode.Open))
//            using (var writer = new BinaryWriter(fs))
//            {
//                // توليد أوامر ESC/POS للطباعة
//                string escPosCommand = ESCPosBarcodeCommand(barcodeData);
//                writer.Write(Encoding.ASCII.GetBytes(escPosCommand));
//            }
//            return true;
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine("حدث خطأ أثناء إرسال البيانات للطابعة: " + ex.Message);
//            return false;
//        }
//    }

//    private string ESCPosBarcodeCommand(byte[] barcodeData)
//    {
//        // أمر ESC/POS للطباعة
//        StringBuilder escPosCommand = new StringBuilder();

//        // تهيئة الطابعة
//        escPosCommand.Append("\x1B\x40");

//        // طباعة الباركود باستخدام ESC/POS
//        escPosCommand.Append("\x1D\x6B\x49");  // طباعة الباركود
//        escPosCommand.Append((char)barcodeData.Length);
//        escPosCommand.Append(Encoding.ASCII.GetString(barcodeData));

//        // إنهاء الطباعة
//        escPosCommand.Append("\n");
//        escPosCommand.Append("\x1D\x56\x00");  // إنهاء الطباعة

//        return escPosCommand.ToString();
//    }
//}




