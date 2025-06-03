// Services/Users/Queries/GetCurrentUserQuery.cs
using MediatR;
using Microsoft.AspNetCore.Identity;
using MiniProject.Models;
using MiniProject.Models.DTOs;
using System.Security.Claims;

namespace MiniProject.Services.Admin.Queries
{
    public class GetCurrentUserQuery : IRequest<UserDTO?>
    {
        // We’ll pass the HttpContext’s ClaimsPrincipal so the handler can find the user
        public ClaimsPrincipal Principal { get; set; } = default!;
    }

    public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, UserDTO?>
    {
        private readonly UserManager<User> _userManager;

        public GetCurrentUserQueryHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<UserDTO?> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            // “Principal” is the currently authenticated user’s ClaimsPrincipal
            var user = await _userManager.GetUserAsync(request.Principal);
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
