using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using RowadSystem.API.Dto;
using RowadSystem.API.Services;
using RowadSystem.Shard.Contract.Barcode;
using RowadSystem.Shard.Contract.Helpers;
using RowadSystem.Shard.Contract.Products;
using System.IO.Compression;

namespace RowadSystem.Controllers;
[Route("api/[controller]")]
[ApiController]
//[Authorize]
public class ProductsController(IProductService productService,
    IValidator<ProductRequest> validator,
    ILogger<ProductsController> logger,
    IBarcodeService barcodeService,
    IExcelService excelExportService
    ) : ControllerBase
{
    private readonly IProductService _productService = productService;
    private readonly IValidator<ProductRequest> _validator = validator;
    private readonly ILogger<ProductsController> _logger = logger;
    private readonly IBarcodeService _barcodeService = barcodeService;
    private readonly IExcelService _excelExportService = excelExportService;

    [HttpPost("")]
    public async Task<IActionResult> Add([FromBody] ProductRequest request)
    {
        //var (product, parseError) = request.data.TryParseJson<ProductRequest>(_logger);

        //if (parseError is not null)
        //    return parseError;

        //var validationResult = await _validator.ValidateWithModelStateAsync(product!, ModelState);

        //if (validationResult is not null)
        //    return validationResult;




        var result = await _productService.AddProductAsync(User.GetUserId(), request);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProductRequest request)
    {


        var result = await _productService.UpdateProductAsync(id, request!);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
    //[HttpPut("{id}")]
    //public async Task<IActionResult> Update(int id, [FromForm] ProductFormRequest<IFormFile> request)
    //{
    //    var (product, parseError) = request.data.TryParseJson<ProductRequest>(_logger);

    //    if (parseError is not null)
    //        return parseError;

    //    var validationResult = await _validator.ValidateWithModelStateAsync(product!, ModelState);

    //    if (validationResult is not null)
    //        return validationResult;

    //    var result = await _productService.UpdateProductAsync(id, product!, request.images);
    //    return result.IsSuccess ? NoContent() : result.ToProblem();
    //}

    //[HttpGet("{id}/barcode")]
    //public async Task<IActionResult> GetBarcode([FromRoute] int id)
    //{
    //    var result = await _productService.GetBarcodeAsync(id);
    //    return result.IsSuccess ? File(result.Value, "image/png") : result.ToProblem();
    //}


    [HttpGet("{id}/barcode")]
    public async Task<IActionResult> GetBarcode([FromRoute] int id)
    {
        var result = await _productService.GetBarcodeAsync(id);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    //[HttpGet("{id}/barcode/{quantity}")]
    //public async Task<IActionResult> GetBarcode([FromRoute] int id, [FromRoute] int quantity)
    //{
    //    var result = await _productService.PrintBarcode(id, quantity);
    //    return result.IsSuccess ?Ok() : result.ToProblem();
    //}



    [HttpGet("barcode/{barcode}")]
    public async Task<IActionResult> GetProductByBarcode([FromRoute] BarcodeRequest request)
    {
        var result = await _productService.GetProductByBarcodeAsync(request.barcode);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
   


    [HttpGet("barcode-invoice-sales/{barcode}")]
    public async Task<IActionResult> GetProductByBarcodeForInvoiceSales([FromRoute] BarcodeRequest request)
    {
        var result = await _productService.GetProductByBarcodeForInvoiceSalesAsync(request.barcode);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("{id}/details")]
    public async Task<IActionResult> GetProductDetails([FromRoute] int id)
    {
        var result = await _productService.GetProductDetailsAsync(id);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("{id}/update")]
    public async Task<IActionResult> GetProductForUpdate([FromRoute] int id)
    {
        var result = await _productService.GetProductForUpdateAsync(id);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll([FromQuery] RequestFilters request)
    {
        var result = await _productService.GetAll(request);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }


    [HttpGet("export")]
    [AllowAnonymous]
    public IActionResult ExportProducts()
    {
        // Sample data (or get from database)
        var products = new List<Product>
    {
        new Product { Id = 1, Name = "Laptop" },
        new Product { Id = 2, Name = "Smartphone"},
        new Product { Id = 3, Name = "Tablet" }
    };

        // Generate the Excel file
        var excelFile = _excelExportService.ExportProductsToExcel(products);

        // Ensure the file is returned as a valid Excel file
        if (excelFile == null || excelFile.Length == 0)
        {
            return BadRequest("Failed to generate the Excel file.");
        }

        // Return the file with the correct MIME type and file name
        return File(excelFile,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "Products.xlsx");
    }


}
