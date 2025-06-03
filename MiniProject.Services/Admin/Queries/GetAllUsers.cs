// Services/Users/Queries/GetAllUsersQuery.cs
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MiniProject.Models;
using MiniProject.Models.DTOs;

namespace MiniProject.Services.Admin.Queries
{
    // 1) The Query object
    public class GetAllUsersQuery : IRequest<List<UserDTO>> { }

    // 2) The Handler
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<UserDTO>>
    {
        private readonly UserManager<User> _userManager;

        public GetAllUsersQueryHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<List<UserDTO>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            // Fetch all users
            var users = await _userManager.Users.ToListAsync(cancellationToken);

            // Map each User → UserDTO
            // (In production you might use AutoMapper; here we do it manually.)
            var results = new List<UserDTO>();
            foreach (var u in users)
            {
                // Get lockout state
                bool isLockedOut = await _userManager.IsLockedOutAsync(u);
                // Get roles (assuming single role per user; adjust if multiple)
                var roles = await _userManager.GetRolesAsync(u);
                var dto = new UserDTO
                {
                    Id = u.Id,
                    Email = u.Email,
                    UserName = u.UserName,
                    IsLockedOut = isLockedOut,
                    Role = roles.FirstOrDefault() // or join them if you allow multiple
                };
                results.Add(dto);
            }

            return results;
        }
    }
}
