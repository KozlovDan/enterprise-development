using AutoService.Application.Interfaces;
using AutoService.Contracts.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace AutoService.Api.Controllers;

/// <summary>Виды работ автосервиса.</summary>
/// <param name="service">Сервис прейскуранта, заказов по видам работ и подбора механиков.</param>
[ApiController]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json")]
[Route("api/work-types")]
public class WorkTypesController(IWorkTypeService service) : ControllerBase
{
    /// <summary>Возвращает список видов работ автосервиса.</summary>
    [HttpGet]
    [ProducesResponseType<IReadOnlyList<WorkTypeDto>>(StatusCodes.Status200OK, "application/json")]
    public async Task<ActionResult<IReadOnlyList<WorkTypeDto>>> GetAll() => Ok(await service.GetAllAsync());

    /// <summary>Возвращает вид работ по его идентификатору.</summary>
    /// <param name="id">Идентификатор вида работ.</param>
    [HttpGet("{id:int}")]
    [ProducesResponseType<WorkTypeDto>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    public async Task<ActionResult<WorkTypeDto>> Get(int id) => Ok(await service.GetAsync(id));

    /// <summary>Добавляет вид работ в прейскурант автосервиса.</summary>
    /// <remarks>Заголовок Location содержит URL чтения добавленного вида работ: /api/work-types/{id}.</remarks>
    /// <param name="request">Название, категория, стоимость и нормативная продолжительность вида работ.</param>
    [HttpPost]
    [ProducesResponseType<WorkTypeDto>(StatusCodes.Status201Created, "application/json")]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict, "application/problem+json")]
    public async Task<ActionResult<WorkTypeDto>> Create(WorkTypeRequest request)
    {
        var result = await service.CreateAsync(request);
        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    /// <summary>Обновляет название, категорию, стоимость и продолжительность вида работ.</summary>
    /// <param name="id">Идентификатор вида работ.</param>
    /// <param name="request">Название, категория, стоимость и нормативная продолжительность вида работ.</param>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict, "application/problem+json")]
    public async Task<IActionResult> Update(int id, WorkTypeRequest request)
    {
        await service.UpdateAsync(id, request);
        return NoContent();
    }

    /// <summary>Удаляет вид работ, если он не входит ни в один заказ.</summary>
    /// <param name="id">Идентификатор вида работ.</param>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict, "application/problem+json")]
    public async Task<IActionResult> Delete(int id)
    {
        await service.DeleteAsync(id);
        return NoContent();
    }

    /// <summary>Заказы с выбранным видом работ.</summary>
    /// <param name="id">Идентификатор вида работ.</param>
    [HttpGet("{id:int}/orders")]
    [ProducesResponseType<IReadOnlyList<OrderDto>>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    public async Task<ActionResult<IReadOnlyList<OrderDto>>> GetOrders(int id) => Ok(await service.GetOrdersAsync(id));

    /// <summary>Механики с подходящей специализацией.</summary>
    /// <param name="id">Идентификатор вида работ.</param>
    [HttpGet("{id:int}/mechanics")]
    [ProducesResponseType<IReadOnlyList<MechanicDto>>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    public async Task<ActionResult<IReadOnlyList<MechanicDto>>> GetMechanics(int id) => Ok(await service.GetMechanicsAsync(id));
}
