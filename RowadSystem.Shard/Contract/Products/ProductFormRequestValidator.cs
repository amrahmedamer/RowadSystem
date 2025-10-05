namespace RowadSystem.Shard.Contract.Products;

public class ProductFormRequestValidator<TFile> : AbstractValidator<ProductFormRequest<TFile>> where TFile : class
{
    private const int MaxFileSize = 5 * 1024 * 1024; // 5MB
    private const int MaxImageCount = 5;
    private static readonly string[] AllowedContentTypes = ["image/jpeg", "image/png"];
    public ProductFormRequestValidator()
    {
        RuleFor(x => x.data)
            .NotEmpty();

        RuleFor(x => x.images)
            .NotNull();


        RuleFor(x => x.images)
                .NotEmpty()
                .Must(images => images.Count <= MaxImageCount);

        //RuleForEach(x => x.images).ChildRules(files =>
        //{
        //    files.RuleFor(file => file.Size)
        //        .LessThanOrEqualTo(MaxFileSize);

        //    files.RuleFor(file => file.ContentType)
        //        .Must(type => AllowedContentTypes.Contains(type));
        //});
    }
}



