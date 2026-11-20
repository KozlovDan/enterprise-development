using AutoService.Application.Interfaces;
using AutoService.Contracts.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace AutoService.Api.Controllers;

/// <summary>Механики автосервиса.</summary>
/// <param name="service">Сервис механиков, их заказов и обслуженных клиентов.</param>
[ApiController]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json")]
[Route("api/mechanics")]
public class MechanicsController(IMechanicService service) : ControllerBase
{
    /// <summary>Возвращает список механиков автосервиса.</summary>
    [HttpGet]
    [ProducesResponseType<IReadOnlyList<MechanicDto>>(StatusCodes.Status200OK, "application/json")]
    public async Task<ActionResult<IReadOnlyList<MechanicDto>>> GetAll() => Ok(await service.GetAllAsync());

    /// <summary>Возвращает механика по его идентификатору.</summary>
    /// <param name="id">Идентификатор механика.</param>
    [HttpGet("{id:int}")]
    [ProducesResponseType<MechanicDto>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    public async Task<ActionResult<MechanicDto>> Get(int id) => Ok(await service.GetAsync(id));

    /// <summary>Добавляет механика с паспортом, специализацией и стажем.</summary>
    /// <remarks>Заголовок Location содержит URL чтения добавленного механика: /api/mechanics/{id}.</remarks>
    /// <param name="request">ФИО, паспорт, специализация и стаж механика.</param>
    [HttpPost]
    [ProducesResponseType<MechanicDto>(StatusCodes.Status201Created, "application/json")]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict, "application/problem+json")]
    public async Task<ActionResult<MechanicDto>> Create(MechanicRequest request)
    {
        var result = await service.CreateAsync(request);
        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    /// <summary>Обновляет ФИО, паспорт, специализацию и стаж механика.</summary>
    /// <param name="id">Идентификатор механика.</param>
    /// <param name="request">ФИО, паспорт, специализация и стаж механика.</param>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict, "application/problem+json")]
    public async Task<IActionResult> Update(int id, MechanicRequest request)
    {
        await service.UpdateAsync(id, request);
        return NoContent();
    }

    /// <summary>Удаляет механика, если он не назначен ни на один заказ.</summary>
    /// <param name="id">Идентификатор механика.</param>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict, "application/problem+json")]
    public async Task<IActionResult> Delete(int id)
    {
        await service.DeleteAsync(id);
        return NoContent();
    }

    /// <summary>Заказы механика.</summary>
    /// <param name="id">Идентификатор механика.</param>
    [HttpGet("{id:int}/orders")]
    [ProducesResponseType<IReadOnlyList<OrderDto>>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    public async Task<ActionResult<IReadOnlyList<OrderDto>>> GetOrders(int id) => Ok(await service.GetOrdersAsync(id));

    /// <summary>Клиенты механика без повторов, в порядке ФИО.</summary>
    /// <param name="id">Идентификатор механика.</param>
    [HttpGet("{id:int}/clients")]
    [ProducesResponseType<IReadOnlyList<ClientDto>>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    public async Task<ActionResult<IReadOnlyList<ClientDto>>> GetClients(int id) => Ok(await service.GetClientsAsync(id));
}
