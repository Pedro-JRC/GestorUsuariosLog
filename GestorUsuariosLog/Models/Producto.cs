using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;

namespace GestorUsuariosLog.Models
{
    public class Producto
    {
        [SwaggerSchema(ReadOnly = true)]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        [MaxLength(100)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "El stock es obligatorio.")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "El proveedor es obligatorio.")]
        public int IdProveedor { get; set; }

        [Required(ErrorMessage = "La categoría es obligatoria.")]
        public int IdCategoria { get; set; }

        
        [JsonIgnore]
        public Proveedor? Proveedor { get; set; }

        [JsonIgnore]
        public Categoria? Categoria { get; set; }
    }
}
