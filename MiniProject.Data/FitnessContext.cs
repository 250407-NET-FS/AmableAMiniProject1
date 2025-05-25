using Microsoft.EntityFrameworkCore;
using MiniProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace MiniProject.Data;

public class FitnessContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public FitnessContext(DbContextOptions<FitnessContext> options) : base(options) { }

    
}
