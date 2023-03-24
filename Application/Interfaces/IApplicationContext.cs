using Microsoft.EntityFrameworkCore;
using SV.Demo.Application.Models;

namespace SV.Demo.Application.Interfaces
{
    public interface IApplicationContext
    {
        DbSet<User> Users { get; set; }

        Task<int> SaveChangesAsync();
    }
}