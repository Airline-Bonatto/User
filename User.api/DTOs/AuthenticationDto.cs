using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.api.DTOs;

public class AuthenticationDto
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}