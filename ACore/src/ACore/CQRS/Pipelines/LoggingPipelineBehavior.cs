using System.Diagnostics;
using System.Text.Json;
using ACore.CQRS.Pipelines.Helpers;
using ACore.Extensions;
using ACore.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ACore.CQRS.Pipelines;

public class LoggingPipelineBehavior<TRequest, TResponse>(ILogger<LoggingPipelineBehavior<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse>
  where TRequest : IRequest<TResponse>
  where TResponse : Result
{
  public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
  {
    var id = Guid.NewGuid();
    Stopwatch? duration = null;
    TResponse? response;

    // Serialization could be expensive for all requests. Enable only for debug.
    if (logger.IsEnabled(LogLevel.Debug))
    {
      logger.LogDebug("Request '{request}'.Id:{id};Data:{data}", typeof(TRequest).Name, id, JsonSerializer.Serialize(request));
      duration = Stopwatch.StartNew();
    }

    try
    {
      response = await next();
    }
    catch (Exception e)
    {
      response = PipelineBehaviorHelper<TResponse>.CreateErrorExceptionResult<TResponse>(e);
    }

    // Analyzes errors for logging
    if (response.IsFailure)
    {
      // It is only validation error, it may not be logged.
      var isSeriousError = (response is ValidationResult || (response.GetType().IsGenericType && response.GetType().GetGenericTypeDefinition() == typeof(ValidationResult<>)));

      // This is a serious exception error.
      if (!isSeriousError && (response is ExceptionResult || (response.GetType().IsGenericType && response.GetType().GetGenericTypeDefinition() == typeof(ExceptionResult<>))))
      {
        var exception = (Exception)(response.PropertyValue(nameof(ExceptionResult.Exception)) ?? throw new Exception($"{nameof(ExceptionResult.Exception)} doesn't exist."));
        LogError(request, response, exception.MessageRecursive(true));
        isSeriousError = true;
      }

      // This is s serious error e.g. configuration.
      if (!isSeriousError)
        LogError(request, response);
    }

    if (!logger.IsEnabled(LogLevel.Debug))
      return response;

    duration?.Stop();
    logger.LogDebug("Response '{request}'.Id:{id};Duration:{duration};Data:{data}", typeof(TRequest).Name, id, duration, JsonSerializer.Serialize(response));

    return response;
  }

  private void LogError(TRequest request, TResponse response, string exception = "")
  {
    var dataRequest = string.Empty;
    var dataResponse = string.Empty;
    try
    {
      dataRequest = JsonSerializer.Serialize(request);
      dataResponse = JsonSerializer.Serialize(response);
    }
    catch (Exception e)
    {
      exception += e.MessageRecursive(true);
    }

    logger.LogError("ErrorId:'{errorId}'; Request:'{requestName}'; ErrorCode:{errorCode}; Error:{error}; DataRequest{dataRequest}; DataResponse{dataResponse}",
      response.Id, typeof(TRequest).Name, response.ResultErrorItem.Code, response.ResultErrorItem.Message + exception, dataRequest, dataResponse);
  }
}