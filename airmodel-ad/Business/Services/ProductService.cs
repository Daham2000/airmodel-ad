﻿using airmodel_ad.Business.Interface;
using airmodel_ad.Data;
using airmodel_ad.Models;
using System.Diagnostics;

namespace airmodel_ad.Business.Services
{
    public class ProductService : IProductService
    {
        AppDbContext appDbContext;
        public ProductService(AppDbContext context) { 
            appDbContext = context;
        }

        public List<ProductModel> GetAllAvailableProducts()
        {
            List<ProductModel> products = new List<ProductModel>();
            try
            {
                products = appDbContext.products.Where((p) => p.productQty > 0).ToList();
                return products;
            }
            catch (Exception ex)
            {
                return products;
            }
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

        bool IProductService.AddProduct(ProductModel product)
        {
            try { 
                appDbContext.Add(product);
                appDbContext.SaveChanges();
                return true;
            } catch (Exception ex)
            {
                return false;
            }
        }

        bool IProductService.AddProductVarient(VarientModel varientModel)
        {
            try
            {
                appDbContext.Add(varientModel);
                appDbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        bool IProductService.AddProductVarientItem(VarientOptionModel varientOption)
        {
            try
            {
                appDbContext.Add(varientOption);
                appDbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
