using System;
using AutoMapper;
using Mango.Services.ProductAPI.DbContexts;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ProductAPI.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        public ProductRepository(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<bool> DeleteProduct(int id)
        {
            try
            {
                var product = await _dbContext.products.FirstOrDefaultAsync(item => item.productid == id);
                if(product is null)
                {
                    return false;
                }
                _dbContext.products.Remove(product);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }

        public async Task<ProductDTO> GetProductById(int id)
        {
            var product = await _dbContext.products.Where(item => item.productid == id).FirstOrDefaultAsync();
            return _mapper.Map<ProductDTO>(product);
        }

        public async Task<IEnumerable<ProductDTO>> GetProducts()
        {
            var productList = await _dbContext.products.ToListAsync();
            return _mapper.Map<List<ProductDTO>>(productList);
        }

        public async Task<ProductDTO> CreateProduct(ProductDTO productDTO)
        {
            var product = _mapper.Map<ProductDTO, Product>(productDTO);

            if (product.productid > 0)
            {
                _dbContext.products.Update(product);
            }
            else
            {
                _dbContext.products.Add(product);
            }

            await _dbContext.SaveChangesAsync();
            return _mapper.Map<Product, ProductDTO>(product);
        }

        public async Task<ProductDTO> UpdateProduct(ProductDTO product)
        {
            if (product.productid==0)
            {
                var pro = _mapper.Map<ProductDTO, Product>(product);
                _dbContext.products.Update(pro);
                await _dbContext.SaveChangesAsync();
            }

            return product;
        }
    }
}

