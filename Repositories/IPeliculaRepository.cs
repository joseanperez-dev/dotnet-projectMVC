using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace projectMVC.Repositories;

public interface IPeliculaRepository<T>
{
    T GetById(int id);
    IEnumerable<T> GetAll();
    IEnumerable<T> GetAllByTematica(int tematicaId);
    IEnumerable<T> GetPagedPeliculas(int page, int pageSize);
    IEnumerable<T> GetPagedPeliculasPorTematica(int tematicaId, int page, int pageSize);
    IEnumerable<T> GetAllBuscador(string b);
    IEnumerable<T> GetPagedPeliculasBuscador(string b, int page, int pageSize);
    void Add(T entity);
    void Update(T entity);
    void Delete(int id);
}
