using System.ComponentModel.DataAnnotations;

namespace GestionProductos.Models;

public class Productos
{
    [Key]
    public int ProductoId { get; set; }

    [Required(ErrorMessage = "La descripcion es obligatoria")]
    public string Descripcion { get; set; } = null!;

    [Range(0.01, double.MaxValue, ErrorMessage = "El costo debe ser mayor a 0")]
    public double Costo { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
    public double Precio { get; set; }
    public double Existencia { get; set; }
}
