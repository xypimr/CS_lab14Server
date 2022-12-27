using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Newtonsoft.Json;
using WebApplication1;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;


var builder = WebApplication.CreateBuilder(args);
// builder.Services.AddControllers().AddNewtonsoftJson(x =>
//     x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddDbContext<AssembliesDbContext>();

var app = builder.Build();

//AssembliesDbContext db = new AssembliesDbContext();

app.MapGet("/api/Detail/{id}", (AssembliesDbContext db, string id) =>
{
    using (db)
    {
        Detail? ans = db.Details.Find(int.Parse(id));
        if (ans != null)
        {
            return Results.Json(ans);
        }
        else
        {
            return Results.NotFound();
        }
    }
});

app.MapGet("/api/Details", (AssembliesDbContext db) =>
{
    using (db)
    {
        List<Detail> temp = new List<Detail>();
        temp = db.Details.ToList();
        if (temp.Count != 0)
        {
            return Results.Json(temp);
        }
        else
        {
            return Results.NoContent();
        }
    }
});

app.MapPost("/api/Detail", (AssembliesDbContext db, Detail data) =>
{
    using (db)
    {
        Detail? dublicat = db.Details.FirstOrDefault(u => u.Name == data.Name);
        if (dublicat == null)
        {
            db.Details.Add(data);
            db.SaveChanges();
            return Results.Ok();
        }
        else
        {
            return Results.NoContent();
        }
    }
});

app.MapPut("/api/Detail", (AssembliesDbContext db, Detail data) =>
{
    using (db)
    {
        Detail? temp = db.Details.Find(data.Id);
        if (temp == null)
        {
            return Results.NotFound();
        }
        else
        {
            temp.Name = data.Name;
            temp.Quantity = data.Quantity;
            db.Details.Update(temp);
            db.SaveChanges();
            return Results.Ok();
        }
    }
});

app.MapDelete("/api/Detail/{id}", (AssembliesDbContext db, string id) =>
{
    using (db)
    {
        var detail = db.Details.Attach(new Detail { Id = int.Parse(id) });
        if (detail != null)
        {
            detail.State = EntityState.Deleted;
            db.SaveChanges();
            return Results.Ok();
        }
        else
        {
            return Results.NotFound();
        }
    }
});

app.MapGet("/api/Assembly/{id}", (AssembliesDbContext db, string id) =>
{
    using (db)
    {
        Assembly? assembly = db.Assemblies.Find(int.Parse(id));
        if (assembly != null)
        {
            db.Parts.Where(u => u.AssemblyId == assembly.Id).Load();
            List<Part> temp = assembly.Parts.ToList();
            List<PartView> partViews = new List<PartView>();
            foreach (var VARIABLE in temp)
            {
                db.Details.Where(u => u.Id == VARIABLE.DetailId).Load();
            }

            for (int i = 0; i < temp.Count; i++)
            {
                PartView partView = new PartView()
                {
                    AssemblyId = temp[i].AssemblyId,
                    Detail = temp[i].Detail,
                    Id = temp[i].Id,
                    Quantity = temp[i].Quantity,
                    DetailId = temp[i].DetailId,
                    DetailName = temp[i].DetailName
                };
                partViews.Add(partView);
            }

            AssemblyView ans = new AssemblyView()
            {
                id = assembly.Id,
                name = assembly.Name,
                PartViews = partViews
            };
            return Results.Json(ans);
        }
        else
        {
            return Results.NotFound();
        }
    }
});

app.MapGet("/api/Assemblies", (AssembliesDbContext db) =>
{
    using (db)
    {
        List<Assembly> data = new List<Assembly>();
        data = db.Assemblies.ToList();
        foreach (var VARIABLE in data)
        {
            db.Parts.Load();
        }

        if (data.Count != 0)
        {
            List<AssemblyView> ansdata = new List<AssemblyView>();
            foreach (var assembly in data)
            {
                List<Part> temp = assembly.Parts.ToList();
                List<PartView> partViews = new List<PartView>();
                for (int i = 0; i < temp.Count; i++)
                {
                    PartView partView = new PartView()
                    {
                        AssemblyId = temp[i].AssemblyId,
                        Detail = temp[i].Detail,
                        Id = temp[i].Id,
                        Quantity = temp[i].Quantity,
                        DetailId = temp[i].DetailId,
                        DetailName = temp[i].DetailName
                    };
                    partViews.Add(partView);
                }

                AssemblyView ans = new AssemblyView()
                {
                    id = assembly.Id,
                    name = assembly.Name,
                    PartViews = partViews
                };
                ansdata.Add(ans);
            }

            return Results.Json(ansdata);
        }
        else
        {
            return Results.NoContent();
        }
    }
});

app.MapPost("api/Assembly", (AssembliesDbContext db, AssemblyView data) =>
{
    using (db)
    {
        Assembly? dublicat = db.Assemblies.FirstOrDefault(u => u.Name == data.name);
        if (dublicat == null)
        {
            Assembly temp = new Assembly() { Id = data.id, Name = data.name };
            db.Assemblies.Add(temp);
            db.SaveChangesAsync();
            List<Part> parttemp = new List<Part>();
            foreach (var VARIABLE in data.PartViews)
            {
                Part part = new Part()
                {
                    Assembly = temp,
                    AssemblyId = temp.Id,
                    Quantity = VARIABLE.Quantity,
                    DetailName = VARIABLE.DetailName,
                    DetailId = VARIABLE.DetailId,
                    Detail = VARIABLE.Detail
                };
                parttemp.Add(part);
            }

            temp.Parts = parttemp;
            db.Parts.AddRange(temp.Parts);
            db.Assemblies.Update(temp);
            //db.Assemblies.Add(temp);
            //db.PartViews.AddRange(temp.PartViews);
            db.SaveChanges();
            return Results.Ok();
        }
        else
        {
            return Results.NoContent();
        }
    }
});

app.MapPut("/api/Assembly", (AssembliesDbContext db, AssemblyView data) =>
{
    using (db)
    {
        Assembly? assembly = db.Assemblies.Find(data.id);
        assembly.Name = data.name;
        db.Parts.Where(u => u.AssemblyId == assembly.Id).Load();
        List<Part> oldparts = assembly.Parts.ToList();
        //List<PartView> newparts = data.PartViews.ToList();
        List<Part> PartsToAdd = new List<Part>();
        List<Part> PartsToDelete = new List<Part>();
        List<Part> PartsToUpdate = new List<Part>();
        for (int i = 0; i < data.PartViews.Count; i++)
        {
            if (oldparts.Exists(u => u.DetailName == data.PartViews[i].DetailName))
            {
                Part PartOld = oldparts[oldparts.FindIndex(u => u.DetailName == data.PartViews[i].DetailName)];
                PartOld.Quantity = data.PartViews[i].Quantity;
                PartsToUpdate.Add(PartOld);
            }
            else if (!oldparts.Exists(u => u.DetailName == data.PartViews[i].DetailName))
            {
                Part parttemp = new Part()
                {
                    Assembly = assembly,
                    AssemblyId = assembly.Id,
                    Detail = db.Details.FirstOrDefault(u => u.Name == data.PartViews[i].DetailName),
                    DetailId = db.Details.FirstOrDefault(u => u.Name == data.PartViews[i].DetailName).Id,
                    DetailName = data.PartViews[i].DetailName,
                    Quantity = data.PartViews[i].Quantity
                };
                PartsToAdd.Add(parttemp);
            }
        }

        foreach (var VARIABLE in oldparts)
        {
            if (!data.PartViews.Exists(u => u.DetailName == VARIABLE.DetailName))
            {
                PartsToDelete.Add(VARIABLE);
            }
        }

        if (PartsToUpdate.Count != 0)
        {
            oldparts = PartsToUpdate;
            db.Parts.UpdateRange(oldparts);
        }

        if (PartsToDelete.Count != 0)
        {
            db.Parts.RemoveRange(PartsToDelete);
        }

        if (PartsToAdd.Count != 0)
        {
            db.Parts.AddRange(PartsToAdd);
        }

        db.Assemblies.Update(assembly);
        db.SaveChanges();
    }
});


app.MapDelete("/api/Assembly/{id}", (AssembliesDbContext db, string id) =>
{
    using (db)
    {
        Assembly? assembly = db.Assemblies.FirstOrDefault(u => u.Id == int.Parse(id));
        db.Parts.Where(u => u.AssemblyId == assembly.Id).Load();
        if (assembly != null)
        {
            db.Assemblies.Remove(assembly);
            db.SaveChanges();
            return Results.Ok();
        }
        else
        {
            return Results.NotFound();
        }
    }
});

app.Run();

public class AssemblyView
{
    public string name { get; set; }
    public int id { get; set; }
    public List<PartView> PartViews { get; set; } = new List<PartView>();
}

public class PartView
{
    public int Id { get; set; }

    public int AssemblyId { get; set; }

    public int DetailId { get; set; }

    public string DetailName { get; set; } = null!;
    public Detail Detail { get; set; } = null!;
    public int Quantity { get; set; }
}