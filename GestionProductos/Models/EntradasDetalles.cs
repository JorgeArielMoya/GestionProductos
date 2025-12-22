using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionProductos.Models;

public class EntradasDetalles
{
    [Key]
    public int DetalleId { get; set; }
    public int EntradaId { get; set; }
    public int ProductoId { get; set; }
    public double Cantidad { get; set; }
    public double Costo { get; set; }

    [ForeignKey(nameof(EntradaId))]
    public virtual Entradas Entrada { get; set; }

    [ForeignKey(nameof(ProductoId))]
    public virtual Productos Producto { get; set; }
}