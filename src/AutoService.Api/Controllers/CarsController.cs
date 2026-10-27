using AutoService.Application.Interfaces;
using AutoService.Contracts.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace AutoService.Api.Controllers;

/// <summary>Автомобили автосервиса.</summary>
/// <param name="service">Сервис ресурса.</param>
[ApiController]
[Route("api/cars")]
public class CarsController(ICarService service) : ControllerBase
{
    /// <summary>Возвращает все записи.</summary>
    [HttpGet]
    [ProducesResponseType<IReadOnlyList<CarDto>>(StatusCodes.Status200OK, "application/json")]
    public ActionResult<IReadOnlyList<CarDto>> GetAll() => Ok(service.GetAll());

    /// <summary>Возвращает запись по идентификатору.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType<CarDto>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    public ActionResult<CarDto> Get(int id) => Ok(service.Get(id));

    /// <summary>Создаёт запись и возвращает ссылку на неё в заголовке Location.</summary>
    [HttpPost]
    [ProducesResponseType<CarDto>(StatusCodes.Status201Created, "application/json")]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict, "application/problem+json")]
    public ActionResult<CarDto> Create(CarRequest request)
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
    public IActionResult Update(int id, CarRequest request)
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

    /// <summary>Владелец автомобиля.</summary>
    [HttpGet("{id:int}/owner")]
    [ProducesResponseType<ClientDto>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    public ActionResult<ClientDto> GetOwner(int id) => Ok(service.GetOwner(id));

    /// <summary>Заказы автомобиля.</summary>
    [HttpGet("{id:int}/orders")]
    [ProducesResponseType<IReadOnlyList<OrderDto>>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    public ActionResult<IReadOnlyList<OrderDto>> GetOrders(int id) => Ok(service.GetOrders(id));
}
