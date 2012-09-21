using System.ComponentModel.DataAnnotations;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Records;

namespace Maps.Models
{
    public class MapRecord : ContentPartRecord
    {
        public virtual double Latitude { get; set; }
        public virtual double Longitude { get; set; }
    }
    public class MapPart : ContentPart<MapRecord> {
        [Required]
        public double Latitude {
            get { return Record.Latitude; }
            set { Record.Latitude = value; }
        }

        [Required]
        public double Longitude {
            get { return Record.Longitude; }
            set { Record.Longitude = value; }
        }

    }
}