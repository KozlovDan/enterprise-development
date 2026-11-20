using AutoService.Application.Interfaces;
using AutoService.Contracts.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace AutoService.Api.Controllers;

/// <summary>Автомобили автосервиса.</summary>
/// <param name="service">Сервис автомобилей, их владельцев и заказов.</param>
[ApiController]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json")]
[Route("api/cars")]
public class CarsController(ICarService service) : ControllerBase
{
    /// <summary>Возвращает список автомобилей автосервиса.</summary>
    [HttpGet]
    [ProducesResponseType<IReadOnlyList<CarDto>>(StatusCodes.Status200OK, "application/json")]
    public async Task<ActionResult<IReadOnlyList<CarDto>>> GetAll() => Ok(await service.GetAllAsync());

    /// <summary>Возвращает автомобиль по его идентификатору.</summary>
    /// <param name="id">Идентификатор автомобиля.</param>
    [HttpGet("{id:int}")]
    [ProducesResponseType<CarDto>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    public async Task<ActionResult<CarDto>> Get(int id) => Ok(await service.GetAsync(id));

    /// <summary>Добавляет автомобиль с указанным владельцем.</summary>
    /// <remarks>Заголовок Location содержит URL чтения добавленного автомобиля: /api/cars/{id}.</remarks>
    /// <param name="request">Госномер, марка, модель, год выпуска и идентификатор владельца автомобиля.</param>
    [HttpPost]
    [ProducesResponseType<CarDto>(StatusCodes.Status201Created, "application/json")]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict, "application/problem+json")]
    public async Task<ActionResult<CarDto>> Create(CarRequest request)
    {
        var result = await service.CreateAsync(request);
        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    /// <summary>Обновляет госномер, марку, модель, год выпуска и владельца автомобиля.</summary>
    /// <param name="id">Идентификатор автомобиля.</param>
    /// <param name="request">Госномер, марка, модель, год выпуска и идентификатор владельца автомобиля.</param>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict, "application/problem+json")]
    public async Task<IActionResult> Update(int id, CarRequest request)
    {
        await service.UpdateAsync(id, request);
        return NoContent();
    }

    /// <summary>Удаляет автомобиль, если для него нет заказов.</summary>
    /// <param name="id">Идентификатор автомобиля.</param>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict, "application/problem+json")]
    public async Task<IActionResult> Delete(int id)
    {
        await service.DeleteAsync(id);
        return NoContent();
    }

    /// <summary>Владелец автомобиля.</summary>
    /// <param name="id">Идентификатор автомобиля.</param>
    [HttpGet("{id:int}/owner")]
    [ProducesResponseType<ClientDto>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    public async Task<ActionResult<ClientDto>> GetOwner(int id) => Ok(await service.GetOwnerAsync(id));

    /// <summary>Заказы автомобиля.</summary>
    /// <param name="id">Идентификатор автомобиля.</param>
    [HttpGet("{id:int}/orders")]
    [ProducesResponseType<IReadOnlyList<OrderDto>>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    public async Task<ActionResult<IReadOnlyList<OrderDto>>> GetOrders(int id) => Ok(await service.GetOrdersAsync(id));
}
