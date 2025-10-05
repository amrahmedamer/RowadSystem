namespace RowadSystem.Shard.consts;
public static class Permissions
{
    public static string Type { get; } = "Permissions";

    public const string ViewUser = "User.View";
    public const string CreateUser = "User.Create";
    public const string EditUser = "User.Edit";
    public const string DeletteUser = "User.Delete";

    public const string ViewProduct = "Product.View";
    public const string CreateProduct = "Product.Create";
    public const string EditProduct = "Product.Edit";
    public const string DeletteProduct = "Product.Delete";

    public const string ViewInvoice = "Invoice.View";
    public const string CreateInvoice = "Invoice.Create";
    public const string EditInvoice = "Invoice.Edit";
    public const string DeletteInvoice = "Invoice.Delete";


    public const string ViewRole = "Role.View";
    public const string CreateRole = "Role.Create";
    public const string EditRole = "Role.Edit";
    public const string DeletteRole = "Role.Delete";

    public const string ViewReport = "Report.View";
    public const string CreateReport = "Report.Create";
    public const string EditReport = "Report.Edit";
    public const string DeletteReport = "Report.Delete";

    public const string ViewBrand = "Brand.View";
    public const string CreateBrand = "Brand.Create";
    public const string EditBrand = "Brand.Edit";
    public const string DeletteBrand = "Brand.Delete";

    public const string ViewCategory = "Category.View";
    public const string CreateCategory = "Category.Create";
    public const string EditCategory = "Category.Edit";
    public const string DeletteCategory = "Category.Delete";


  

    public static IList<string> GetAllPermissions()
       => typeof(Permissions)
        .GetFields()
        .Select(f => f.GetValue(null)?.ToString()).ToList()!;

}
