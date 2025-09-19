using projectMVC.Data;
using projectMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace projectMVC.Repositories;

public class UserRepository : IUserRepository<User>
{
    public readonly Context context;

    public UserRepository(Context context)
    {
        this.context = context;
    }

    public void Add(User entity)
    {
        context.Set<User>().Add(entity);
        context.SaveChanges();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<User> GetUser(string email, string password)
    {
        return await context.Users.Where(u => u.Email == email)
                                  .Where(p => p.Password == password)
                                  .Where(e => e.Status == 1)
                                  .FirstOrDefaultAsync();
    }

    public async Task<User> GetUserActiveByEmail(string email)
    {
        return await context.Users.Where(u => u.Email == email)
                                  .Where(e => e.Status == 1)
                                  .FirstOrDefaultAsync();
    }

    public async Task<User> GetUserActiveByToken(string token)
    {
        return await context.Users.Where(u => u.Token == token)
                                  .Where(e => e.Status == 1)
                                  .FirstOrDefaultAsync();
    }

    public async Task<User> GetUserByEmail(string email)
    {
        return await context.Users.Where(u => u.Email == email)
                                  .FirstOrDefaultAsync();
    }

    public async Task<User> GetUserByToken(string token)
    {
        return await context.Users.Where(u => u.Token == token)
                                  .Where(e => e.Status == 0)
                                  .FirstOrDefaultAsync();
    }

    public void Update(User entity)
    {
        context.Entry(entity).State = EntityState.Modified;
        context.SaveChanges();
    }
}