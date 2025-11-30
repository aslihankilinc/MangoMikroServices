using AutoMapper;
using Mango.Service.Product.Utility;
using Mango.Services.ProductApi.Data;
using Mango.Services.ProductApi.Models;
using Mango.Services.ProductApi.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.ProductApi.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductApiController : Controller
    {
        private readonly AppDbContext _db;
        private ResponseDto _response;
        private IMapper mapper;
        public ProductApiController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            this.mapper = mapper;
            this._response = new ResponseDto();
        }
        [HttpGet("getProductList")]
        public ResponseDto GetProductList()
        {
            try
            {
                IEnumerable<ProductDto> productDtos = mapper.Map<IEnumerable<ProductDto>>(_db.Product.ToList());
                _response.Result = productDtos;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;

        }
        [HttpGet("getProduct/{id:int}")]
        public ResponseDto GetProduct(int id)
        {
            try
            {
                ProductDto productDto = mapper.Map<ProductDto>(_db.Product.FirstOrDefault(u => u.ProductId == id));
                _response.Result = productDto;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpPost("created")]
        [Authorize(Roles =Const.Admin )]
        public ResponseDto CreatedProduct([FromBody] ProductDto productDto)
        {
            try
            {
                var product = mapper.Map<Models.Product>(productDto);
                if (product.ProductId==0)
                    _db.Product.Add(product);
                _db.SaveChanges();
                if (productDto.Image is not null)
                {
                    string folder = "wwwroot/images/";
                    folder += Guid.NewGuid().ToString() + "_" + productDto.Image.FileName;
                    productDto.ImageLocalPath = folder;
                    product.ImageLocalPath = folder;
                    using (var fileStream = new FileStream(folder, FileMode.Create))
                    {
                        productDto.Image.CopyTo(fileStream);
                    }
                    string serverUrl = $"{Request.Scheme}://{Request.Host.Value}/";
                    product.ImageUrl = serverUrl + folder.Substring("wwwroot/".Length).Replace("\\", "/");
                    product.ImageLocalPath = product.ImageUrl;
                }
                else
                {
                    product.ImageUrl = "https://placehold.co/600x400";
                }
                _db.Product.Update(product);
                _db.SaveChanges();
                _response.Result = mapper.Map<ProductDto>(product);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }


        [HttpDelete("deleted/{id:int}")]
        [Authorize(Roles = Const.Admin)]
        public ResponseDto Deleted(int id)
        {
            try
            {
                Product obj = _db.Product.First(u => u.ProductId == id);
                if (!string.IsNullOrEmpty(obj.ImageLocalPath))
                {
                    var oldFilePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), obj.ImageLocalPath);
                    FileInfo file = new FileInfo(oldFilePathDirectory);
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                }
                _db.Product.Remove(obj);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }



    }
}
