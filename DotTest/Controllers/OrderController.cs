using DotTest.DTOs;
using DotTest.Services;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
    {
        try
        {
            return Ok(await _orderService.GetAllOrders());
        }
        catch (Exception ex) {
            return BadRequest(ex.Message);
        }
        
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderDto>> GetOrder(int id)
    {
        try
        {
            var order = await _orderService.GetOrderById(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult> PostOrder(OrderDto orderDto)
    {
        try
        {
            await _orderService.AddOrder(orderDto);
            return CreatedAtAction(nameof(GetOrder), new { id = orderDto.OrderId }, orderDto);
        }
        catch (Exception ex)
        {
            return Ok(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutOrder(int id, OrderDto orderDto)
    {
        try
        {
            if (id != orderDto.OrderId)
            {
                return Ok(new { message = "Wrong Id !" });
            }

            await _orderService.UpdateOrder(orderDto);
            return Ok(new { message = "Success Updated Data!" });
        }
        catch (Exception ex)
        {
            return Ok(new { message = ex.Message });
        }


    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        try
        {
            await _orderService.DeleteOrder(id);
            return Ok(new { message = "Success Deleted Data!" });

        }
        catch (Exception ex)
        {
            return Ok(new { message = ex.Message });
        }
    }
}
