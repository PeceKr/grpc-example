using GRPCExample.Models;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using GRPCExample.Data;

namespace GRPCExample.Services
{
    public class ProductService : ProductProtoService.ProductProtoServiceBase
    {
        private readonly AppDbContext _dbContext;
        public ProductService(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }

        public override async Task<CreateProductResponse> CreateProduct(CreateProductRequest request, ServerCallContext context)
        {
            if (request.Name == string.Empty || request.Quantity == 0 || request.Price == 0)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Bad request"));

            var product = new Product
            {
                Name = request.Name,
                Quantity = request.Quantity,
                Price = (decimal)request.Price
            };

            await _dbContext.AddAsync(product);
            await _dbContext.SaveChangesAsync();

            return await Task.FromResult(new CreateProductResponse
            {
                ProductId = product.ProductId
            });
        }
    }
}