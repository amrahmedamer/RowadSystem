namespace RowadSystem.Shard.Contract.Products;

//public record ProductFormRequest(
//    string data,
//  List<IBrowserFile> images

//    );

public class ProductFormRequest<TFile>
{
    public string data { get; set; } = string.Empty;
    public List<TFile> images { get; set; } = new List<TFile>();

}