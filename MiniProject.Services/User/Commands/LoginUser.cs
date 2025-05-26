using MediatR;
using Microsoft.AspNetCore.Identity;
using MiniProject.Models;

namespace MiniProject.Services.Users.Commands;


public class LoginUser
{
    public class Command : IRequest<string>
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class Handler(UserManager<User> userManager, IUserService userService) 
        : IRequestHandler<Command, string>
    {
        public async Task<string> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(request.Email)
                ?? throw new Exception("Invalid credentials");

            var result = await userManager.CheckPasswordAsync(user, request.Password);

            if (!result)
                throw new Exception("Invalid credentials");


            return await userService.GenerateToken(user);
        }
    }
}
