﻿using server.Dto;
using server.Models;

namespace server.Interfaces
{
    public interface IProductRepository
    {
        ICollection<ProductCatalogDto> GetProducts();
        Task<Product> GetProductById(int id);
        Task AddProduct(Product product);
        Task RemoveProduct(Product product);
        Task UpdateProduct(Product product, ProductUpdateDto productDto);
    }
}
