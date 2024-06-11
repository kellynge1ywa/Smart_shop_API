using Microsoft.AspNetCore.Mvc;

namespace duka;
[Route("api/[controller]")]
[ApiController]

public class CategoryController : ControllerBase
{
    private readonly Icategory _categoryServices;
    private readonly ResponseDto _responseDto;
    public CategoryController(Icategory icategory)
    {
        _categoryServices = icategory;
        _responseDto = new ResponseDto();
    }

    [HttpGet]
    public async Task<ActionResult<ResponseDto>> GetCategories()
    {
        try
        {
            var categories = await _categoryServices.GetCategories();
            if (categories != null)
            {
                _responseDto.Result = categories;
                return Ok(_responseDto);
            }
            _responseDto.Error = "Categories not found";
            return NotFound(_responseDto);

        }
        catch (Exception ex)
        {
            _responseDto.Error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            return StatusCode(500, _responseDto);
        }
    }

    [HttpGet("{Id}")]
    public async Task<ActionResult<ResponseDto>> GetCategory(Guid Id)
    {
        try
        {
            var category = await _categoryServices.GetCategory(Id);
            if (category != null)
            {
                _responseDto.Result = category;
                return Ok(_responseDto);
            }
            _responseDto.Error = "Category not found";
            return NotFound(_responseDto);


        }
        catch (Exception ex)
        {
            _responseDto.Error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            return StatusCode(500, _responseDto);
        }
    }

    [HttpPost]
    public async Task<ActionResult<ResponseDto>> AddCategory(AddCategory newCategory)
    {
        try
        {
            var new_category = new Category()
            {
                Id = new Guid(),
                Name = newCategory.Name,
                ImageURL = newCategory.ImageURL
            };
            var addedCategory = await _categoryServices.AddCategory(new_category);
            _responseDto.Result = addedCategory;
            return Created("", _responseDto);

        }
        catch (Exception ex)
        {
            _responseDto.Error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            return StatusCode(500, _responseDto);
        }
    }

    [HttpPut("{Id}")]
    public async Task<ActionResult<ResponseDto>> UpdateCategory(Guid Id, AddCategory updateCategory)
    {
        try
        {
            var category = await _categoryServices.GetCategory(Id);

            if (category == null)
            {
                _responseDto.Error = "Category not found";
                return NotFound(_responseDto);
            }

            var updatedCategory = new Category()
            {
                Id = category.Id,
                Name = updateCategory.Name ?? category.Name,
                ImageURL = updateCategory.ImageURL ?? category.ImageURL

            };

            var updateResult = await _categoryServices.UpdateCategory(category.Id, updatedCategory);
            _responseDto.Result = updateResult;

            if (updateResult == "Category updated!!")
            {
                return Ok(_responseDto);
            }
            else
            {
                return NotFound(_responseDto);
            }

        }
        catch (Exception ex)
        {
            _responseDto.Error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            return StatusCode(500, _responseDto);
        }
    }
    [HttpDelete("{Id}")]
    public async Task<ActionResult<ResponseDto>> DeleteCategory(Guid Id)
    {
        try
        {
            var category = await _categoryServices.GetCategory(Id);
            if (category == null)
            {
                _responseDto.Error = "Category not found";
                return NotFound(_responseDto);
            }

            var deleted = await _categoryServices.DeleteCategory(category);
            _responseDto.Result = deleted;
            return Ok(_responseDto);

        }
        catch (Exception ex)
        {
            _responseDto.Error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            return StatusCode(500, _responseDto);
        }
    }

}
