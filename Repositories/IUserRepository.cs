using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace projectMVC.Repositories;

public interface IUserRepository<T>
{
    Task<T> GetUser(string email, string password);
    Task<T> GetUserByEmail(string email);
    Task<T> GetUserActiveByEmail(string email);
    Task<T> GetUserByToken(string token);
    Task<T> GetUserActiveByToken(string token);
    void Add(T entity);
    void Update(T entity);
    void Delete(int id);
}