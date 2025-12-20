using GestionProductos.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GestionProductos.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Entradas> Entradas { get; set; } 
    public DbSet<Productos> Productos { get; set; } 
    public DbSet<EntradasDetalles> EntradasDetalles { get; set; }
}
