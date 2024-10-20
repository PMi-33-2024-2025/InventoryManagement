using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.DAL.Entities
{
    public class Supplier
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
