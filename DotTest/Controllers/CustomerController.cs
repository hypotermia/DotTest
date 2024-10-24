using DotTest.DTOs;
using DotTest.Services;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomers()
    {
        try
        {
            return Ok(await _customerService.GetAllCustomers());
        }
        catch (Exception ex) { 
            return BadRequest(ex.Message);  
        }
        
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerDto>> GetCustomer(int id)
    {
        try
        {
            var customer = await _customerService.GetCustomerById(id);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer);

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult> PostCustomer(CustomerDto customerDto)
    {
        try
        {
            await _customerService.AddCustomer(customerDto);
            return CreatedAtAction(nameof(GetCustomer), new { id = customerDto.CustomerId }, customerDto);
        }
        catch (Exception ex) {
            return Ok(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutCustomer(int id, CustomerDto customerDto)
    {
        try
        {
            if (id != customerDto.CustomerId)
            {
                return BadRequest();
            }

            await _customerService.UpdateCustomer(customerDto);
            return Ok(new { message = "Success Edit Data" });
        }
        catch (Exception ex)
        {
            return Ok(new { message = ex.Message });
        }

    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomer(int id)
    {
        try
        {
            await _customerService.DeleteCustomer(id);
            return Ok(new { message = "Success Deleted Data" });

        }
        catch (Exception ex)
        {
            return Ok(new { message = ex.Message });
        }
    }
}
