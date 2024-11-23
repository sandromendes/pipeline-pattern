﻿namespace PipelinePatternSample.UseCases
{
    public interface IUseCase<TRequest, TResponse>
    {
        Task<TResponse> ExecuteAsync(TRequest request);
    }
}
