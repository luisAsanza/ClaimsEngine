using Microsoft.AspNetCore.Mvc;
using ClaimsEngine.Ingestion.Api.Features.Claims.Contracts;
using ClaimsEngine.Ingestion.Api.Features.Claims;
using MediatR;
using ClaimsEngine.Application.Features.Claims.Commands;
using ClaimsEngine.Application.Features.Claims.DTOs;
using ClaimsEngine.Domain.Aggregates.ClaimAggregate;
using System.Collections.Generic;
using System.Linq;

namespace ClaimsEngine.Ingestion.Api.Features.Claims.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ClaimsController : ControllerBase
{
    private readonly ISender _mediator;
    private readonly ILogger<ClaimsController> _logger;

    public ClaimsController(ISender mediator, ILogger<ClaimsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    // POST api/v1/claims
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SubmitClaim(
        [FromBody] SubmitClaimRequest claim,
        [FromHeader(Name = "X-Correlation-ID")] string? correlationIdHeader,
        CancellationToken cancellationToken)
    {
        // 1. Edge telemetry: Extract or Mint the Correlation ID
        var correlationId = !string.IsNullOrEmpty(correlationIdHeader) && Guid.TryParse(correlationIdHeader, out var parsedCorrelationId)
            ? parsedCorrelationId
            : Guid.NewGuid();

        // 2. Attach metadata to the logging scope
        using var scope = _logger.BeginScope(new Dictionary<string, object>
        {
            ["CorrelationId"] = correlationId,
            ["ClaimNumber"] = claim.ClaimNumber
        });

        _logger.LogInformation("Received claim {ClaimNumber} (CorrelationId: {CorrelationId})", claim.ClaimNumber, correlationId);

        // 3. Map the HTTP contract to the Application Command
        var command = new SubmitClaimCommand(
            correlationId,
            claim.ClaimNumber,
            claim.SubscriberId,
            claim.PayerId,
            claim.ProviderNpi,
            new PatientDto(
                claim.Patient.Name,
                claim.Patient.DateOfBirth,
                Enum.Parse<RelationshipToInsured>(claim.Patient.RelationshipToInsured)),
            new InsuredDto(
                claim.Insured.Name,
                claim.Insured.DateOfBirth),
            claim.LineItems.Select(li => new LineItemDto(
                li.Description,
                li.Amount)).ToList()
        );

        // 4. Dispatch the command to the Application layer for processing
        var claimId = await _mediator.Send(command, cancellationToken);

        // Return 202 Accepted with a ClaimAcceptedResponse for asynchronous processing
        var response = new ClaimAcceptedResponse(claimId, correlationId, "Ingested");
        return AcceptedAtAction(
            ClaimsRouteNames.GetClaimStatus, 
            new { id = claimId }, 
            response);
    }

    // GET api/v1/claims/{id}
    [HttpGet("{id:guid}", Name = ClaimsRouteNames.GetClaimStatus)]
    public IActionResult GetClaimStatus(Guid id)
    {
        // TODO: Retrieve claim status
        return Ok();
    }
}
