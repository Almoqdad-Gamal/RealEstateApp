using RealEstateApp.Domain.Common;

namespace RealEstateApp.Domain.Entities
{
    public class Review : BaseEntity
    {
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
        

        //Foreign Keys
        public int PropertyId { get; set; }
        public Property Property { get; set; } = null!;

        public int UserId { get; set; }
        public User User { get; set; } = null!;
        
    }
}