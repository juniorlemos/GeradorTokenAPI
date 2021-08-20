using GeradorTokenAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeradorTokenAPI.Data
{
    public class UsuarioContext : IdentityDbContext
    {
        public UsuarioContext(DbContextOptions<UsuarioContext> options) : base(options)
        {
            
    }
        public DbSet<Usuario> Usuarios { get; set; }
    }
}
