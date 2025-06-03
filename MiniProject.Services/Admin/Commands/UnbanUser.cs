// Services/Users/Commands/UnbanUserCommand.cs
using MediatR;
using Microsoft.AspNetCore.Identity;
using MiniProject.Models;
using MiniProject.Models.DTOs;

namespace MiniProject.Services.Admin.Commands
{
    public class UnbanUserCommand : IRequest<UserDTO?>
    {
        public Guid Id { get; set; }
    }

    public class UnbanUserCommandHandler : IRequestHandler<UnbanUserCommand, UserDTO?>
    {
        private readonly UserManager<User> _userManager;

        public UnbanUserCommandHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<UserDTO?> Handle(UnbanUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            if (user == null) return null;

            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow);
            await _userManager.SetLockoutEnabledAsync(user, false);

            var isLockedOut = await _userManager.IsLockedOutAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            return new UserDTO
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                IsLockedOut = isLockedOut,
                Role = roles.FirstOrDefault()
            };
        }
    }
}
