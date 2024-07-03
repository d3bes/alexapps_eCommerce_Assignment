using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Collections.Generic;


namespace project.Core.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }

    }
}