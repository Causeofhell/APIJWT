﻿namespace Application.DTOs.User
{
    public record LoginResponse(bool Flag, string Message = null!, string Token = null!);
}
