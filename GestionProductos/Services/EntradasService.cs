using GestionProductos.Data;
using GestionProductos.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GestionProductos.Services;

public class EntradasService(IDbContextFactory<ApplicationDbContext> DbFactory)
{
    public async Task<bool> Guardar (Entradas entrada)
    {
        if (!await Existe (entrada.EntradaId))
        {
            return await Insertar(entrada);
        }
        else
        {
            return await Modificar(entrada);
        }
    }

    private async Task<bool> Existe (int entradaId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Entradas.AnyAsync(e => e.EntradaId  == entradaId);
    }

    private async Task<bool> Insertar (Entradas entrada)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.Entradas.Add(entrada);
        await AfectarExistencia(entrada.EntradaDetalle, TipoOperacion.Suma);
        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(Entradas entrada)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();

        var EntradaActual = await contexto.Entradas
            .Include(d => d.EntradaDetalle)
            .FirstOrDefaultAsync(p => p.EntradaId == entrada.EntradaId);

        if (EntradaActual == null) return false;

        await AfectarExistencia(EntradaActual.EntradaDetalle, TipoOperacion.Resta);

        contexto.EntradasDetalles.RemoveRange(EntradaActual.EntradaDetalle);

        EntradaActual.Fecha = entrada.Fecha;
        EntradaActual.Concepto = entrada.Concepto;

        foreach(var detalle in entrada.EntradaDetalle)
        {
            var nuevoDetalle = new EntradasDetalles
            {
                ProductoId = detalle.ProductoId,
                Cantidad = detalle.Cantidad,
                Costo = detalle.Costo
            };
            EntradaActual.EntradaDetalle.Add(nuevoDetalle); 
        }

        await AfectarExistencia(entrada.EntradaDetalle, TipoOperacion.Suma);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> Eliminar(int entradaId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();

        var EntradaActual = await contexto.Entradas
            .Include(d => d.EntradaDetalle)
            .FirstOrDefaultAsync(p => p.EntradaId == entradaId);

        if (EntradaActual == null) return false;

        await AfectarExistencia(EntradaActual.EntradaDetalle, TipoOperacion.Resta);

        contexto.EntradasDetalles.RemoveRange(EntradaActual.EntradaDetalle);
        contexto.Entradas.Remove(EntradaActual);

        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<Entradas?> Buscar (int entradaId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Entradas
            .Include(e=> e.EntradaDetalle)
            .FirstOrDefaultAsync(e => e.EntradaId == entradaId);
    }

    public async Task<List<Entradas>> Listar(Expression<Func<Entradas, bool>> criterio)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Entradas
            .Include(e => e.EntradaDetalle)
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }

    private async Task AfectarExistencia(ICollection<EntradasDetalles> detalle, TipoOperacion operacion)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();

        foreach (var item in detalle)
        {
            var Producto = await contexto.Productos.SingleAsync(c => c.ProductoId == item.ProductoId);

            if (operacion == TipoOperacion.Suma)
            {
                Producto.Existencia += item.Cantidad;
            }

            else
            {
                Producto.Existencia -= item.Cantidad;
            }

            await contexto.SaveChangesAsync();
        }
    }

    private enum TipoOperacion
    {
        Suma = 1,
        Resta = 2
    }
}
