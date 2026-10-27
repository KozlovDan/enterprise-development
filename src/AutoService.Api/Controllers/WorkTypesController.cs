using AutoService.Application.Interfaces;
using AutoService.Contracts.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace AutoService.Api.Controllers;

/// <summary>Виды работ автосервиса.</summary>
/// <param name="service">Сервис ресурса.</param>
[ApiController]
[Route("api/work-types")]
public class WorkTypesController(IWorkTypeService service) : ControllerBase
{
    /// <summary>Возвращает все записи.</summary>
    [HttpGet]
    [ProducesResponseType<IReadOnlyList<WorkTypeDto>>(StatusCodes.Status200OK, "application/json")]
    public ActionResult<IReadOnlyList<WorkTypeDto>> GetAll() => Ok(service.GetAll());

    /// <summary>Возвращает запись по идентификатору.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType<WorkTypeDto>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    public ActionResult<WorkTypeDto> Get(int id) => Ok(service.Get(id));

    /// <summary>Создаёт запись и возвращает ссылку на неё в заголовке Location.</summary>
    [HttpPost]
    [ProducesResponseType<WorkTypeDto>(StatusCodes.Status201Created, "application/json")]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict, "application/problem+json")]
    public ActionResult<WorkTypeDto> Create(WorkTypeRequest request)
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
    public IActionResult Update(int id, WorkTypeRequest request)
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

    /// <summary>Заказы с выбранным видом работ.</summary>
    [HttpGet("{id:int}/orders")]
    [ProducesResponseType<IReadOnlyList<OrderDto>>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    public ActionResult<IReadOnlyList<OrderDto>> GetOrders(int id) => Ok(service.GetOrders(id));

    /// <summary>Механики с подходящей специализацией.</summary>
    [HttpGet("{id:int}/mechanics")]
    [ProducesResponseType<IReadOnlyList<MechanicDto>>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    public ActionResult<IReadOnlyList<MechanicDto>> GetMechanics(int id) => Ok(service.GetMechanics(id));
}
