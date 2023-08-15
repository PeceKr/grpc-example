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

        public override async Task<GetProductResponse> GetProduct(GetProductRequest request, ServerCallContext context)
        {
            if (request.ProductId <= 0)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Id must be grater than 0"));

            var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.ProductId == request.ProductId);

            if (product == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Product with this id not found"));

            return new GetProductResponse
            {
                Name = product.Name,
                Price = (double)product.Price,
                ProductId = product.ProductId,
                Quantity = product.Quantity
            };
        }

        public override async Task<GetAllProductsResponse> GetAllProducts(GetAllProductsRequest request, ServerCallContext context)
        {
            var response = new GetAllProductsResponse();
            // TODO : add pagination
            var products = await _dbContext.Products.ToListAsync();

            if (products.Any())

                foreach (var product in products)
                {
                    response.Products.Add(new GetProductResponse
                    {
                        Name = product.Name,
                        Price = (double)product.Price,
                        Quantity = product.Quantity,
                        ProductId = product.ProductId,
                    });
                }

            return response;
        }

        public override async Task<UpdateProductResponse> UpdateProduct(UpdateProductRequest request, ServerCallContext context)
        {
            if (request.ProductId <= 0)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Id must be grater than 0"));

            var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.ProductId == request.ProductId);
            if (product == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Product with this id not found"));

            if (request.Name == string.Empty || request.Quantity == 0 || request.Price == 0)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Bad request"));

            product.Name = request.Name;
            product.Price = (decimal)request.Price;
            product.Quantity = request.Quantity;

            _dbContext.Update(product);
            await _dbContext.SaveChangesAsync();

            return new UpdateProductResponse
            {
                ProductId = product.ProductId
            };
        }

        public override async Task<DeleteProductResponse> DeleteProduct(DeleteProductRequest request, ServerCallContext context)
        {
            if (request.ProductId <= 0)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Id must be grater than 0"));

            var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.ProductId == request.ProductId);
            if (product == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Product with this id not found"));

            _dbContext.Remove(product);
            await _dbContext.SaveChangesAsync();

            return new DeleteProductResponse
            {
                ProductId = request.ProductId
            };
        }
    }
}