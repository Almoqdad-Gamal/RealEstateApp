using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace RealEstateApp.Application.Features.Notifications.Commands.MarkAllAsRead
{
    public class MarkAllAsReadCommand : IRequest
    {
        public int UserId { get; set; }

        public MarkAllAsReadCommand(int userId)
        {
            UserId = userId;
        }
    }
}