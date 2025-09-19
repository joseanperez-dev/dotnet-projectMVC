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

public class TematicaRepository: ITematicaRepository<Tematica>
{

    public readonly Context context;

    public TematicaRepository(Context context)
    {
        this.context = context;
    }

    public Tematica GetById(int id)
    {
        return context.Set<Tematica>().Find(id);
    }

    public IEnumerable<Tematica> GetAll()
    {
        return context.Set<Tematica>().OrderByDescending(c => c.Id).ToList();
    }

    public void Add(Tematica tematica)
    {
        context.Set<Tematica>().Add(tematica);
        context.SaveChanges();
    }

    public void Update(Tematica tematica)
    {
        context.Entry(tematica).State = EntityState.Modified;
        context.SaveChanges();
    }
    public void Delete(int id)
    {
        var tematica = context.Set<Tematica>().Find(id);
        if (tematica != null)
        {
            context.Set<Tematica>().Remove(tematica);
            context.SaveChanges();
        }
    }
}