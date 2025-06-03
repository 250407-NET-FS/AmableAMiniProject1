// Services/Users/Queries/GetUserByIdQuery.cs
using MediatR;
using Microsoft.AspNetCore.Identity;
using MiniProject.Models;
using MiniProject.Models.DTOs;

namespace MiniProject.Services.Admin.Queries
{
    public class GetUserByIdQuery : IRequest<UserDTO?>
    {
        public Guid Id { get; set; }
    }

    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDTO?>
    {
        private readonly UserManager<User> _userManager;

        public GetUserByIdQueryHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<UserDTO?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            if (user == null) return null;

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
