using Microsoft.AspNetCore.Mvc;
using BookingApp.Places;
using System.Text.Json;
using BookingApp.Services.Results;
using BookingApp.Services.Requests;

namespace BookingApp.Places.V1;

[ApiController]
[Route("v1/places")]
public class PlaceController : ControllerBase
{
  private ILogger<PlaceController> Logger;
  private IPlaceService PlaceService;

  public PlaceController(ILogger<PlaceController> logger, IPlaceService placeService)
  {
    Logger = logger;
    PlaceService = placeService;
  }

  [HttpGet]
  public async Task<ActionResult<CollectionResult<PlaceDto>>> Index([FromQuery] SearchPlaceRequest input)
  {
    var result = await PlaceService.GetCollectionAsync(input);
    return Ok(result);
  }

  [HttpGet("{id:int}")]
  public async Task<ActionResult<PlaceDto>> GetById(int id)
  {
    var place = await PlaceService.GetByIdAsync(new GetByIdRequest { Id = id });
    return Ok(place);
  }

  [HttpPost]
  public async Task<ActionResult<PlaceDto>> Create([FromBody] CreateUpdatePlaceDto body)
  {
    var place = await PlaceService.CreateAsync(body);
    return CreatedAtAction(nameof(GetById), new { Id = place.Id }, place);
  }

  [HttpPut("{id:int}")]
  public async Task<ActionResult<PlaceDto>> Update(int id, [FromBody] CreateUpdatePlaceDto body)
  {
    var place = await PlaceService.UpdateAsync(new GetByIdRequest { Id = id }, body);
    return Ok(place);
  }

  [HttpDelete("{id:int}")]
  public async Task<ActionResult> Delete(int id)
  {
    await PlaceService.DeleteAsync(new GetByIdRequest { Id = id });
    return Ok();
  }
}