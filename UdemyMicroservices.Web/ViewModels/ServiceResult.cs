﻿using System.Text.Json;
using Refit;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace UdemyMicroservices.Web.ViewModels;

public class ServiceResult
{
    public bool IsSuccess => ProblemDetails is null;

    public bool IsFail => !IsSuccess;


    public ProblemDetails? ProblemDetails { get; set; }


    // Static factory method for success
    public static ServiceResult Success()
    {
        return new ServiceResult();
    }


    public static ServiceResult FailFromProblemDetails(ApiException exception)
    {
        if (string.IsNullOrEmpty(exception.Content))
            return new ServiceResult
            {
                ProblemDetails = new ProblemDetails
                {
                    Title = exception.Message
                }
            };


        return new ServiceResult
        {
            ProblemDetails = JsonSerializer.Deserialize<ProblemDetails>(exception.Content, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            })
        };
    }

    public static ServiceResult Fail(ProblemDetails problemDetails)
    {
        return new ServiceResult
        {
            ProblemDetails = problemDetails
        };
    }

    public static ServiceResult Fail(string error, string? errorDetail = null)
    {
        return new ServiceResult
        {
            ProblemDetails = new ProblemDetails
            {
                Title = error,
                Detail = errorDetail
            }
        };
    }
}

public class ServiceResult<T> : ServiceResult
{
    public T? Data { get; init; }

    // Static factory method for success with data
    public static ServiceResult<T> Success(T data)
    {
        return new ServiceResult<T> { Data = data };
    }


    public new static ServiceResult<T> FailFromProblemDetails(ApiException exception)
    {
        if (string.IsNullOrEmpty(exception.Content))
            return new ServiceResult<T>
            {
                ProblemDetails = new ProblemDetails
                {
                    Title = exception.Message
                }
            };


        return new ServiceResult<T>
        {
            ProblemDetails = JsonSerializer.Deserialize<ProblemDetails>(exception.Content, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            })
        };
    }

    public new static ServiceResult<T> Fail(ProblemDetails problemDetails)
    {
        return new ServiceResult<T>
        {
            ProblemDetails = problemDetails
        };
    }

    public new static ServiceResult<T> Fail(string error, string? errorDetail = null)
    {
        return new ServiceResult<T>
        {
            ProblemDetails = new ProblemDetails
            {
                Title = error,
                Detail = errorDetail
            }
        };
    }


    // Static factory method for general failure
}