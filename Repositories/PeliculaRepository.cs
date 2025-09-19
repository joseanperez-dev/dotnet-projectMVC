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

public class PeliculaRepository: IPeliculaRepository<Pelicula>
{

    public readonly Context context;

    public PeliculaRepository(Context context)
    {
        this.context = context;
    }

    public Pelicula GetById(int id)
    {
        return context.Set<Pelicula>().Find(id);
    }

    public IEnumerable<Pelicula> GetAll()
    {
        return context.Set<Pelicula>().Include(x => x.Tematica).OrderByDescending(c => c.Id).ToList();
    }

    public void Add(Pelicula pelicula)
    {
        context.Set<Pelicula>().Add(pelicula);
        context.SaveChanges();
    }

    public void Update(Pelicula pelicula)
    {
        context.Entry(pelicula).State = EntityState.Modified;
        context.SaveChanges();
    }
    public void Delete(int id)
    {
        var pelicula = context.Set<Pelicula>().Find(id);
        if (pelicula != null)
        {
            context.Set<Pelicula>().Remove(pelicula);
            context.SaveChanges();
        }
    }

    public IEnumerable<Pelicula> GetAllByTematica(int tematicaId)
    {
        return context.Set<Pelicula>().Where(p => p.TematicaId == tematicaId).ToList();
    }

    public IEnumerable<Pelicula> GetPagedPeliculas(int page, int pageSize)
    {
        return context.Set<Pelicula>().OrderByDescending(c => c.Id)
                                      .Skip((page - 1) * pageSize)
                                      .Take(pageSize).Include(x => x.Tematica)
                                      .ToList();
    }

    public IEnumerable<Pelicula> GetPagedPeliculasPorTematica(int tematicaId, int page, int pageSize)
    {
        return context.Set<Pelicula>().Where(p => p.TematicaId == tematicaId)
                                      .OrderByDescending(c => c.Id)
                                      .Skip((page - 1) * pageSize)
                                      .Take(pageSize).Include(x => x.Tematica)
                                      .ToList();
    }

    public IEnumerable<Pelicula> GetAllBuscador(string b)
    {
        return context.Set<Pelicula>().Where(p => p.Nombre.Contains(b)).ToList();
    }

    public IEnumerable<Pelicula> GetPagedPeliculasBuscador(string b, int page, int pageSize)
    {
        return context.Set<Pelicula>().Where(p => p.Nombre.Contains(b))
                                      .OrderByDescending(c => c.Id)
                                      .Skip((page - 1) * pageSize)
                                      .Take(pageSize).Include(x => x.Tematica)
                                      .ToList();
    }
}