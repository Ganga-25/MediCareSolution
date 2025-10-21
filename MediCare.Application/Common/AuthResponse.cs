using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MediCare.Application.DTOs.AuthDTO
{
    public class AuthResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = null!;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? AccessToken { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? RefreshToken { get; set; }

        public AuthResponse(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }

        public AuthResponse(int statusCode, string message, string accessToken)
        {
            StatusCode = statusCode;
            Message = message;
            AccessToken = accessToken;
        }

        public AuthResponse(int statusCode, string message, string accessToken, string refreshToken)
        {
            StatusCode = statusCode;
            Message = message;
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
}
