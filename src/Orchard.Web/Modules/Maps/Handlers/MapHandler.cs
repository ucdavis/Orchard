using Maps.Models;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;

namespace Maps.Handlers
{
    public class MapHandler : ContentHandler
    {
        public MapHandler(IRepository<MapRecord> repository) {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}