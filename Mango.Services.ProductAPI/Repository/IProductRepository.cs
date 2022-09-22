﻿using System;
using Mango.Services.ProductAPI.Models.DTO;

namespace Mango.Services.ProductAPI.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductDTO>> GetProducts();
        Task<ProductDTO> GetProductById(int id);
        Task<ProductDTO> CreateProduct(ProductDTO productDTO);
        Task<bool> DeleteProduct(int id);
        Task<ProductDTO> UpdateProduct(ProductDTO product);
    }
}
