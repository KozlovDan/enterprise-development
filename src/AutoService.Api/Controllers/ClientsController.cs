using AutoService.Application.Interfaces;
using AutoService.Contracts.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace AutoService.Api.Controllers;

/// <summary>Клиенты автосервиса.</summary>
/// <param name="service">Сервис клиентов, их автомобилей и истории заказов.</param>
[ApiController]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json")]
[Route("api/clients")]
public class ClientsController(IClientService service) : ControllerBase
{
    /// <summary>Возвращает список клиентов автосервиса.</summary>
    [HttpGet]
    [ProducesResponseType<IReadOnlyList<ClientDto>>(StatusCodes.Status200OK, "application/json")]
    public async Task<ActionResult<IReadOnlyList<ClientDto>>> GetAll() => Ok(await service.GetAllAsync());

    /// <summary>Возвращает клиента по его идентификатору.</summary>
    /// <param name="id">Идентификатор клиента.</param>
    [HttpGet("{id:int}")]
    [ProducesResponseType<ClientDto>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    public async Task<ActionResult<ClientDto>> Get(int id) => Ok(await service.GetAsync(id));

    /// <summary>Добавляет клиента автосервиса.</summary>
    /// <remarks>Заголовок Location содержит URL чтения добавленного клиента: /api/clients/{id}.</remarks>
    /// <param name="request">ФИО и контактный телефон клиента.</param>
    [HttpPost]
    [ProducesResponseType<ClientDto>(StatusCodes.Status201Created, "application/json")]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict, "application/problem+json")]
    public async Task<ActionResult<ClientDto>> Create(ClientRequest request)
    {
        var result = await service.CreateAsync(request);
        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    /// <summary>Обновляет ФИО и контактный телефон клиента.</summary>
    /// <param name="id">Идентификатор клиента.</param>
    /// <param name="request">ФИО и контактный телефон клиента.</param>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict, "application/problem+json")]
    public async Task<IActionResult> Update(int id, ClientRequest request)
    {
        await service.UpdateAsync(id, request);
        return NoContent();
    }

    /// <summary>Удаляет клиента, если у него нет автомобилей и заказов.</summary>
    /// <param name="id">Идентификатор клиента.</param>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict, "application/problem+json")]
    public async Task<IActionResult> Delete(int id)
    {
        await service.DeleteAsync(id);
        return NoContent();
    }

    /// <summary>Автомобили клиента.</summary>
    /// <param name="id">Идентификатор клиента.</param>
    [HttpGet("{id:int}/cars")]
    [ProducesResponseType<IReadOnlyList<CarDto>>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    public async Task<ActionResult<IReadOnlyList<CarDto>>> GetCars(int id) => Ok(await service.GetCarsAsync(id));

    /// <summary>Заказы клиента.</summary>
    /// <param name="id">Идентификатор клиента.</param>
    [HttpGet("{id:int}/orders")]
    [ProducesResponseType<IReadOnlyList<OrderDto>>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    public async Task<ActionResult<IReadOnlyList<OrderDto>>> GetOrders(int id) => Ok(await service.GetOrdersAsync(id));
}
