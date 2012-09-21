using Maps.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;

namespace Maps.Drivers
{
    public class MapDriver : ContentPartDriver<MapPart> {
        protected override DriverResult Display(MapPart part, string displayType, dynamic shapeHelper) {
            return ContentShape("Parts_Map", () => shapeHelper.Parts_Map(
                Longitude: part.Longitude,
                Latitude: part.Latitude));
        }

        //Get
        protected override DriverResult Editor(MapPart part, dynamic shapeHelper) {
            return ContentShape("Parts_Map_Edit", () => shapeHelper.EditorTemplate(
                TemplateName: "Parts/Map",
                Model: part,
                Prefix: Prefix));
        }

        //post
        protected override DriverResult Editor(MapPart part, IUpdateModel updater, dynamic shapeHelper) {
            updater.TryUpdateModel(part, Prefix, null, null);
            return Editor(part, shapeHelper);
        }
    }
}