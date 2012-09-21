using Orchard.ContentManagement.Handlers;
using SimpleCommerce.Models;
using Orchard.Data;

namespace SimpleCommerce.Handlers
{
    public class ProductHandler : ContentHandler
    {
        public ProductHandler(IRepository<ProductPartRecord> repository) {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}