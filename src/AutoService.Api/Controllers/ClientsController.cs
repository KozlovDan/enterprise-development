using AutoService.Application.Interfaces;
using AutoService.Contracts.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace AutoService.Api.Controllers;

/// <summary>Клиенты автосервиса.</summary>
/// <param name="service">Сервис ресурса.</param>
[ApiController]
[Route("api/clients")]
public class ClientsController(IClientService service) : ControllerBase
{
    /// <summary>Возвращает все записи.</summary>
    [HttpGet]
    [ProducesResponseType<IReadOnlyList<ClientDto>>(StatusCodes.Status200OK, "application/json")]
    public ActionResult<IReadOnlyList<ClientDto>> GetAll() => Ok(service.GetAll());

    /// <summary>Возвращает запись по идентификатору.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType<ClientDto>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    public ActionResult<ClientDto> Get(int id) => Ok(service.Get(id));

    /// <summary>Создаёт запись и возвращает ссылку на неё в заголовке Location.</summary>
    [HttpPost]
    [ProducesResponseType<ClientDto>(StatusCodes.Status201Created, "application/json")]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict, "application/problem+json")]
    public ActionResult<ClientDto> Create(ClientRequest request)
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
    public IActionResult Update(int id, ClientRequest request)
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

    /// <summary>Автомобили клиента.</summary>
    [HttpGet("{id:int}/cars")]
    [ProducesResponseType<IReadOnlyList<CarDto>>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    public ActionResult<IReadOnlyList<CarDto>> GetCars(int id) => Ok(service.GetCars(id));

    /// <summary>Заказы клиента.</summary>
    [HttpGet("{id:int}/orders")]
    [ProducesResponseType<IReadOnlyList<OrderDto>>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    public ActionResult<IReadOnlyList<OrderDto>> GetOrders(int id) => Ok(service.GetOrders(id));
}
