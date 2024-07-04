using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using project.Core.Models;

namespace project.Service.Contracts
{
    public interface ITokenService
    {
        string GenerateJwtToken(User user);

    }
}