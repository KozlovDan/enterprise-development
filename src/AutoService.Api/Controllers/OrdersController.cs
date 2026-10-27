using AutoService.Application.Interfaces;
using AutoService.Contracts.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace AutoService.Api.Controllers;

/// <summary>Заказы автосервиса.</summary>
/// <param name="service">Сервис ресурса.</param>
[ApiController]
[Route("api/orders")]
public class OrdersController(IOrderService service) : ControllerBase
{
    /// <summary>Возвращает все записи.</summary>
    [HttpGet]
    [ProducesResponseType<IReadOnlyList<OrderDto>>(StatusCodes.Status200OK, "application/json")]
    public ActionResult<IReadOnlyList<OrderDto>> GetAll() => Ok(service.GetAll());

    /// <summary>Возвращает запись по идентификатору.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType<OrderDto>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    public ActionResult<OrderDto> Get(int id) => Ok(service.Get(id));

    /// <summary>Создаёт запись и возвращает ссылку на неё в заголовке Location.</summary>
    [HttpPost]
    [ProducesResponseType<OrderDto>(StatusCodes.Status201Created, "application/json")]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict, "application/problem+json")]
    public ActionResult<OrderDto> Create(OrderRequest request)
    {
        var result = service.Create(request);
        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    /// <summary>Полностью обновляет существующую запись.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict, "application/problem+json")]
    public IActionResult Update(int id, OrderRequest request)
    {
        service.Update(id, request);
        return NoContent();
    }

    /// <summary>Удаляет запись, если её не используют связанные ресурсы.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict, "application/problem+json")]
    public IActionResult Delete(int id)
    {
        service.Delete(id);
        return NoContent();
    }

    /// <summary>Автомобиль заказа.</summary>
    [HttpGet("{id:int}/car")]
    [ProducesResponseType<CarDto>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    public ActionResult<CarDto> GetCar(int id) => Ok(service.GetCar(id));

    /// <summary>Клиент заказа.</summary>
    [HttpGet("{id:int}/client")]
    [ProducesResponseType<ClientDto>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    public ActionResult<ClientDto> GetClient(int id) => Ok(service.GetClient(id));

    /// <summary>Механик заказа.</summary>
    [HttpGet("{id:int}/mechanic")]
    [ProducesResponseType<MechanicDto>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    public ActionResult<MechanicDto> GetMechanic(int id) => Ok(service.GetMechanic(id));

    /// <summary>Виды работ заказа.</summary>
    [HttpGet("{id:int}/work-types")]
    [ProducesResponseType<IReadOnlyList<WorkTypeDto>>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    public ActionResult<IReadOnlyList<WorkTypeDto>> GetWorkTypes(int id) => Ok(service.GetWorkTypes(id));

    /// <summary>Суммарная стоимость работ заказа.</summary>
    [HttpGet("{id:int}/total-cost")]
    [ProducesResponseType<OrderCostDto>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    public ActionResult<OrderCostDto> GetTotalCost(int id) => Ok(service.GetTotalCost(id));
}
