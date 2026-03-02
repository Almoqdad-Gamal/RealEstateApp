using MediatR;

namespace RealEstateApp.Application.Features.Properties.Commands.DeleteProperty
{
    public class DeletePropertyCommand : IRequest
    {
        public int Id { get; set; }

        public DeletePropertyCommand(int id)
        {
            Id = id;
        }
        public int RequestingUserId { get; set; }
        public string RequestingRole { get; set; } = string.Empty;
    }
}