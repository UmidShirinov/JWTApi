using AuthServer.Core.DTOS;
using AuthServer.Core.Model;
using AuthServer.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWTApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : BaseeController
    {
        private readonly IServices<Product, ProductDto> _productServices;

        public ProductController(IServices<Product, ProductDto> _productServices)
        {
            this._productServices = _productServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetProduct()
        {
            return ActionResultInstance(await _productServices.GetAllAsync());
        }

        [HttpPost]
        public async Task<IActionResult> SaveProduct(ProductDto productDto )
        {
            return ActionResultInstance(await _productServices.AddAsync(productDto));
        }


        [HttpPut]
        public async Task<IActionResult> UpdateProduct(ProductDto productDto)
        {
            return ActionResultInstance(await _productServices.Update(productDto, productDto.Id));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveProduct(int id)
        {
            return ActionResultInstance(await _productServices.Remove(id));

        }
    }
}
