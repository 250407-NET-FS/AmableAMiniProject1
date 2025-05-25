using System.Security.Claims;
using MiniProject.Models;

namespace MiniProject.Services;

public interface IUserService{
    Task<string> GenerateToken(User user);
}