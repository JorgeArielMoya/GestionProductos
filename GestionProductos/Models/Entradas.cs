using System.ComponentModel.DataAnnotations;

namespace GestionProductos.Models;

public class Entradas
{
    [Key]
    public int EntradaId { get; set; }

    [Required(ErrorMessage = "La fecha es obligatoria")]
    public DateTime Fecha { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "El concepto es obligatorio")]
    public string Concepto { get; set; } = null!;
    public double Total { get; set; }
    public ICollection<EntradasDetalles> EntradaDetalle { get; set; } = new List<EntradasDetalles>();       
}
