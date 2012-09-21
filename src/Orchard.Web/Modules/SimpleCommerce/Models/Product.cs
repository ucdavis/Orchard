using System.ComponentModel.DataAnnotations;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Records;

namespace SimpleCommerce.Models
{
    public class ProductPartRecord : ContentPartRecord
    {
        public virtual string Sku { get; set; }
        public virtual float Price { get; set; }
    }

    public class ProductPart : ContentPart<ProductPartRecord> {
        [Required]
        public string Sku {
            get { return Record.Sku; }
            set { Record.Sku = value; }
        }

        [Required]
        public float Price {
            get { return Record.Price; }
            set { Record.Price = value; }
        }
    }
}