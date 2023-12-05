﻿using airmodel_ad.Business.Interface;
using airmodel_ad.Data;
using airmodel_ad.Models;

namespace airmodel_ad.Business.Services
{
    public class ProductService : IProductService
    {
        AppDbContext appDbContext;
        public ProductService(AppDbContext context) { 
            appDbContext = context;
        }
        public List<ProductModel> GetAllProducts()
        {
            List<ProductModel> products = new List<ProductModel>();
            try {
                products = appDbContext.products.ToList();
                return products;
            } catch (Exception ex) {
                return products;
            }
        }
    }
}
