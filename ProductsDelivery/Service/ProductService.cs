﻿using Microsoft.EntityFrameworkCore;
using ProductsDelivery.Models;
using ProductsDelivery.ViewModels;
using System.Collections.Generic;

namespace ProductsDelivery.Service
{
    public class ProductService
    {
        private readonly ApplicationContext _db;
        private readonly ILogger<ProductService> _logger;
        public ProductService(ApplicationContext dbContext, ILogger<ProductService> logger)
        {
            _db = dbContext;
            _logger = logger;
        }
        public async Task<List<Product>> AllProductsAsync()
        {
            List<Product> availableProducts = await _db.Products
                .Where(p => p.OrderId == null).ToListAsync();
            List<int> uniqueSerialCode = await _db.Products
                .Where(p => p.OrderId == null)
                .GroupBy(p => p.SerialCode)
                .Select(p => p.Key).ToListAsync();
            List<Product> uniqueProducts = new List<Product>();
            foreach(var serialCode in uniqueSerialCode)
            {
                List<Product> products = availableProducts.Where(p => p.SerialCode == serialCode).ToList();
                Product product = products.First();
                product.Count = products.Count;
                uniqueProducts.Add(product);
            }
            return uniqueProducts;
        }
        public async Task<Product> ProductFindByIdAsync(int id)
        {
            return await _db.Products.SingleOrDefaultAsync(p => p.Id == id);
        }

        internal async Task UpdateOrderIdAsync(int orderId, int serialCode, int count)
        {
            var products = _db.Products.Where(p => p.SerialCode == serialCode).ToList();
            for(int i = 0; i < count; i++)
            {
                products[i].OrderId = orderId;
                _db.Products.Update(products[i]);
            }
            await _db.SaveChangesAsync();
        }

        public async Task UpdateCountProductAsync(Product product, int count)
        {
            /*product.Count = product.Count - count;*/
            _db.Products.Update(product);
            await _db.SaveChangesAsync();
        }
    }
}