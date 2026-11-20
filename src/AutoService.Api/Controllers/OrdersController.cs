using AutoService.Application.Interfaces;
using AutoService.Contracts.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace AutoService.Api.Controllers;

/// <summary>Заказы автосервиса.</summary>
/// <param name="service">Сервис заказов, состава работ и расчёта стоимости.</param>
[ApiController]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json")]
[Route("api/orders")]
public class OrdersController(IOrderService service) : ControllerBase
{
    /// <summary>Возвращает список заказов автосервиса.</summary>
    [HttpGet]
    [ProducesResponseType<IReadOnlyList<OrderDto>>(StatusCodes.Status200OK, "application/json")]
    public async Task<ActionResult<IReadOnlyList<OrderDto>>> GetAll() => Ok(await service.GetAllAsync());

    /// <summary>Возвращает заказ по его идентификатору.</summary>
    /// <param name="id">Идентификатор заказа.</param>
    [HttpGet("{id:int}")]
    [ProducesResponseType<OrderDto>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    public async Task<ActionResult<OrderDto>> Get(int id) => Ok(await service.GetAsync(id));

    /// <summary>Оформляет заказ на автомобиль клиента с выбранным механиком и видами работ.</summary>
    /// <remarks>Заголовок Location содержит URL чтения оформленного заказа: /api/orders/{id}.</remarks>
    /// <param name="request">Идентификаторы автомобиля, клиента, механика и работ, даты приёма и выдачи.</param>
    [HttpPost]
    [ProducesResponseType<OrderDto>(StatusCodes.Status201Created, "application/json")]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict, "application/problem+json")]
    public async Task<ActionResult<OrderDto>> Create(OrderRequest request)
    {
        var result = await service.CreateAsync(request);
        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    /// <summary>Обновляет автомобиль, клиента, механика, состав работ и даты заказа.</summary>
    /// <param name="id">Идентификатор заказа.</param>
    /// <param name="request">Идентификаторы автомобиля, клиента, механика и работ, даты приёма и выдачи.</param>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict, "application/problem+json")]
    public async Task<IActionResult> Update(int id, OrderRequest request)
    {
        await service.UpdateAsync(id, request);
        return NoContent();
    }

    /// <summary>Удаляет заказ и его связи с видами работ.</summary>
    /// <param name="id">Идентификатор заказа.</param>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict, "application/problem+json")]
    public async Task<IActionResult> Delete(int id)
    {
        await service.DeleteAsync(id);
        return NoContent();
    }

    /// <summary>Автомобиль заказа.</summary>
    /// <param name="id">Идентификатор заказа.</param>
    [HttpGet("{id:int}/car")]
    [ProducesResponseType<CarDto>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    public async Task<ActionResult<CarDto>> GetCar(int id) => Ok(await service.GetCarAsync(id));

    /// <summary>Клиент заказа.</summary>
    /// <param name="id">Идентификатор заказа.</param>
    [HttpGet("{id:int}/client")]
    [ProducesResponseType<ClientDto>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    public async Task<ActionResult<ClientDto>> GetClient(int id) => Ok(await service.GetClientAsync(id));

    /// <summary>Механик заказа.</summary>
    /// <param name="id">Идентификатор заказа.</param>
    [HttpGet("{id:int}/mechanic")]
    [ProducesResponseType<MechanicDto>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    public async Task<ActionResult<MechanicDto>> GetMechanic(int id) => Ok(await service.GetMechanicAsync(id));

    /// <summary>Виды работ заказа.</summary>
    /// <param name="id">Идентификатор заказа.</param>
    [HttpGet("{id:int}/work-types")]
    [ProducesResponseType<IReadOnlyList<WorkTypeDto>>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    public async Task<ActionResult<IReadOnlyList<WorkTypeDto>>> GetWorkTypes(int id) => Ok(await service.GetWorkTypesAsync(id));

    /// <summary>Суммарная стоимость работ заказа.</summary>
    /// <param name="id">Идентификатор заказа.</param>
    [HttpGet("{id:int}/total-cost")]
    [ProducesResponseType<OrderCostDto>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    public async Task<ActionResult<OrderCostDto>> GetTotalCost(int id) => Ok(await service.GetTotalCostAsync(id));
}
