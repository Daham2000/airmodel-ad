﻿using airmodel_ad.Models;

namespace airmodel_ad.Business.Interface
{
    public interface IProductService
    {
        public List<ProductModel> GetAllProducts();
    }
}