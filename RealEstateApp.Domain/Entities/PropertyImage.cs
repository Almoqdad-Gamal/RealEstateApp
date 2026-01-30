using RealEstateApp.Domain.Common;

namespace RealEstateApp.Domain.Entities
{
    public class PropertyImage : BaseEntity
    {
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsPrimary { get; set; } = false;
        public int DisplayOrder { get; set; }

        //Foreign Key
        public int PropertyId { get; set; }
        public Property Property { get; set; } = null!;
    }
}