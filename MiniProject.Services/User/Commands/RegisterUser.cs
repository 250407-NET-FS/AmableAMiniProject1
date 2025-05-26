using MiniProject.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace MiniProject.Services.Users.Commands;
public class RegisterUser
{
    public class RegisterCommand : IRequest<string>
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

    public class Handler(UserManager<User> userManager) : IRequestHandler<RegisterCommand, string>
    {
        public async Task<string> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var user = new User { UserName = request.Email, Email = request.Email };
            var result = await userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
                throw new Exception(string.Join("; ", result.Errors.Select(e => e.Description)));

            return user.Id.ToString();
        }
    }
}
