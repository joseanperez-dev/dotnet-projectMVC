using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace projectMVC.Repositories;

public interface IPeliculaFotoRepository<T>
{
    T GetById(int id);
    IEnumerable<T> GetAll();
    IEnumerable<T> GetFotosByPelicula(int id);
    void Add(T entity);
    void Update(T entity);
    void Delete(int id);
}