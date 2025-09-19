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

public class PeliculaFotoRepository : IPeliculaFotoRepository<PeliculaFoto>
{
    public readonly Context context;

    public PeliculaFotoRepository(Context context)
    {
        this.context = context;
    }

    public void Add(PeliculaFoto peliculaFoto)
    {
        context.Set<PeliculaFoto>().Add(peliculaFoto);
        context.SaveChanges();
    }

    public void Delete(int id)
    {
        var peliculaFoto = context.Set<PeliculaFoto>().Find(id);
        if (peliculaFoto != null)
        {
            context.Set<PeliculaFoto>().Remove(peliculaFoto);
            context.SaveChanges();
        }
    }

    public IEnumerable<PeliculaFoto> GetAll()
    {
        return context.Set<PeliculaFoto>().Include(x => x.Pelicula).OrderByDescending(c => c.Id).ToList();
    }

    public PeliculaFoto GetById(int id)
    {
        return context.Set<PeliculaFoto>().Find(id);
    }

    public IEnumerable<PeliculaFoto> GetFotosByPelicula(int peliculaId)
    {
        return context.Set<PeliculaFoto>().Where(p => p.PeliculaId == peliculaId).ToList();
    }

    public void Update(PeliculaFoto peliculaFoto)
    {
        context.Entry(peliculaFoto).State = EntityState.Modified;
        context.SaveChanges();
    }
}