namespace GRPCExample.Services
{
    public class ProductService : ProductProtoService.ProductProtoServiceBase
    {
        private readonly AppDbContext _dbContext;
        public ProductService(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }

        public override async Task<CreateProductResponse> CreateProduct (CreateProductRequest request, ServerCallContext context)
        {
            
        }
    }
}