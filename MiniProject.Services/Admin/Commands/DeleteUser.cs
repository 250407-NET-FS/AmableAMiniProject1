// Services/Users/Commands/DeleteUserCommand.cs
using MediatR;
using Microsoft.AspNetCore.Identity;
using MiniProject.Models;

namespace MiniProject.Services.Admin.Commands
{
    public class DeleteUserCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }

    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly UserManager<User> _userManager;

        public DeleteUserCommandHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            if (user == null) return false;

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }
    }
}
