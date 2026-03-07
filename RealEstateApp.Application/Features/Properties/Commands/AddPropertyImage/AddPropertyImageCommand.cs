using MediatR;

namespace RealEstateApp.Application.Features.Properties.Commands.AddPropertyImage
{
    public class AddPropertyImageCommand : IRequest<string>
    {
        public int PropertyId { get; set; }
        public Stream ImageStream { get; set; } = null!;
        public string FileName { get; set; } = string.Empty;
    }
}