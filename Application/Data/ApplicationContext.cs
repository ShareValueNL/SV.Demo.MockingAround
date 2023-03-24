using Microsoft.EntityFrameworkCore;
using SV.Demo.Application.Interfaces;
using SV.Demo.Application.Models;

namespace SV.Demo.Application.Data;

public class ApplicationContext : DbContext, IApplicationContext
{
    public virtual DbSet<User> Users { get; set; }

    public async Task<int> SaveChangesAsync() => await base.SaveChangesAsync();
}
