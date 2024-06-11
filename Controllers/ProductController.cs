using Microsoft.AspNetCore.Mvc;

namespace duka;
[Route("api/[controller]")]
[ApiController]

public class ProductController:ControllerBase
{
    private readonly Iproduct _productServices;
    private readonly ResponseDto _response;
    public ProductController(Iproduct iproduct)
    {
        _productServices=iproduct;
        _response=new ResponseDto();
    }

    [HttpGet]
    public async Task<ActionResult<ResponseDto>> GetProducts()
    {
        try
        {
            var products=await _productServices.GetProducts();
            if (products == null)
            {
                _response.Error="Products not found";
                return NotFound(_response);
            }
            
            _response.Result=products;
            return Ok(_response);

        }
        catch (Exception ex)
        {
            _response.Error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            return StatusCode(500, _response);
        }
    }
    [HttpGet("{Id}")]
    public async Task<ActionResult<ResponseDto>> GetProduct(Guid Id)
    {
        try
        {
            var product= await _productServices.GetProduct(Id);
            if( product == null)
            {
                _response.Error="Product not found";
                return NotFound(_response);
            }
            _response.Result=product;
            return Ok(_response);

        }
        catch (Exception ex)
        {
            _response.Error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            return StatusCode(500, _response);
        }
    }
    
    [HttpPost("{categoryId}")]
    public async Task<ActionResult<ResponseDto>> AddProduct(Guid categoryId,AddProduct newProduct)
    {
        try
        {
            var new_product=new Product()
            {
                Id=new Guid(),
                Name=newProduct.Name,
                Price=newProduct.Price,
                ImageURL=newProduct.ImageURL,
                CategoryId=categoryId
            };
            var product= await _productServices.AddProduct(new_product);
            _response.Result=product;
            return Created("",_response);

        }
        catch (Exception ex)
        {
            _response.Error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            return StatusCode(500, _response);
        }
    }

    [HttpPut("{Id}")]
    public async Task<ActionResult<ResponseDto>> UpdateProduct(Guid Id, AddProduct updateProduct)
    {
        try
        {
            var product= await _productServices.GetProduct(Id);
            if (product == null)
            {
                _response.Error="Product not found";
                return Ok(_response);
            }
            

            var updated_product=new Product()
            {
                Id=product.Id,
                Name=updateProduct.Name ?? product.Name,
                Price=updateProduct.Price,
                ImageURL=updateProduct.ImageURL ?? product.ImageURL,
                CategoryId=product.CategoryId

            };

            var updatedResult= await _productServices.UpdateProduct(product.Id,updated_product);
            _response.Result=updatedResult;

            if(updatedResult == "Product updated!!")
            {
                return Ok(_response);
            }
             else
             {
                return BadRequest(_response);
             }

        }
        catch (Exception ex)
        {
            _response.Error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            return StatusCode(500, _response);
        }
    }

     [HttpPatch("{Id}")]
    public async Task<ActionResult<ResponseDto>> UpdateProductfield(Guid Id, AddProduct updateProduct)
    {
        try
        {
            var product= await _productServices.GetProduct(Id);
            if (product == null)
            {
                _response.Error="Product not found";
                return Ok(_response);
            }
            

            var updated_product=new Product()
            {
                Id=product.Id,
                Name=updateProduct.Name ?? product.Name,
                Price=updateProduct.Price,
                ImageURL=updateProduct.ImageURL ?? product.ImageURL,
                CategoryId=product.CategoryId

            };

            var updatedResult= await _productServices.UpdateProduct(product.Id,updated_product);
            _response.Result=updatedResult;

            if(updatedResult == "Product updated!!")
            {
                return Ok(_response);
            }
             else
             {
                return BadRequest(_response);
             }

        }
        catch (Exception ex)
        {
            _response.Error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            return StatusCode(500, _response);
        }
    }


    [HttpDelete("{Id}")]
    public async Task<ActionResult<ResponseDto>> DeleteProduct (Guid Id)
    {
        try
        {
            var product= await _productServices.GetProduct(Id);
            if (product == null)
            {
                _response.Error="Product not found";
            return NotFound(_response);
            }
            var deleted= await _productServices.DeleteProduct(product);
                _response.Result=deleted;

            if (deleted == "Product deleted")
            {
                return Ok(_response);

            }   
            else
            {
                return BadRequest(_response);
            } 

            
        }
        catch (Exception ex)
        {
            _response.Error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            return StatusCode(500, _response);
        }
    }

}
