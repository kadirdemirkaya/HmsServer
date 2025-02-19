﻿using System.Text.Json;

namespace Hsm.Domain.Models.Response
{
    public class ApiResponseModel<T>
    {
        public bool Success { get; set; }
        public string[]? Message { get; set; }
        public T? Data { get; set; }
        public DateTime CreatedAt { get; set; }
        public int StatusCode { get; set; }

        public ApiResponseModel()
        {

        }
        public ApiResponseModel(T data, bool success, int statusCode, params string[] message)
        {
            Success = success;
            Message = message;
            Data = data;
            StatusCode = statusCode;
            CreatedAt = DateTime.UtcNow;
        }
        public static ApiResponseModel<T> CreateSuccess(T data)
        {
            return new ApiResponseModel<T>(data, true, 200, null);
        }

        public static ApiResponseModel<T> CreateFailure(params string[] message)
        {
            return new ApiResponseModel<T>(default(T), false, 400, message);
        }

        public static ApiResponseModel<T> CreateNotFound(params string[] message)
        {
            return new ApiResponseModel<T>(default(T), false, 404, message);
        }

        public static ApiResponseModel<T> CreateServerError(params string[] message)
        {
            return new ApiResponseModel<T>(default(T), false, 500, message);
        }
        public static ApiResponseModel<T> CreateEmailAlreadyExists(params string[] message)
        {
            return new ApiResponseModel<T>(default(T), false, 409, message);
        }
        public override string ToString() => JsonSerializer.Serialize(this);
    }
}
