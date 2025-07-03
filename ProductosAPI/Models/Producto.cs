using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace ProductosAPI.Models;

[Index("Sku", Name = "UQ__Producto__DDDF4BE78D5F94F6", IsUnique = true)]
public partial class Producto
{
    [Key]
    [Column("id_producto")]
    public int IdProducto { get; set; }

    [Column("nombre")]
    [StringLength(255)]
    public string Nombre { get; set; } = null!;

    [Column("descripcion")]
    public string? Descripcion { get; set; }

    [Column("precio", TypeName = "decimal(10, 2)")]
    public decimal Precio { get; set; }

    [Column("sku")]
    [StringLength(50)]
    public string? Sku { get; set; }

    [Column("stock")]
    public int Stock { get; set; }

    [Column("url_imagen_principal")]
    [StringLength(255)]
    public string? UrlImagenPrincipal { get; set; }

    [JsonIgnore]
    [Column("id_categoria")]
    public int IdCategoria { get; set; }

    [Column("fecha_creacion", TypeName = "datetime")]
    public DateTime? FechaCreacion { get; set; }

    [Column("ultima_actualizacion", TypeName = "datetime")]
    public DateTime? UltimaActualizacion { get; set; }

    [Column("activo")]
    public bool? Activo { get; set; }

    [JsonIgnore]
    [ForeignKey("IdCategoria")]
    [InverseProperty("Productos")]
    public virtual Categoria IdCategoriaNavigation { get; set; } = null!;

    [NotMapped]
    [JsonPropertyName("categoriaNombre")]
    public string CategoriaNombre
        => IdCategoriaNavigation?.Nombre ?? string.Empty;
}
