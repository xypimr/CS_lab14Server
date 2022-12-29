using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Newtonsoft.Json;
using DataBaseServer;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;


var builder = WebApplication.CreateBuilder(args);
// builder.Services.AddControllers().AddNewtonsoftJson(x =>
//     x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddDbContext<ApplicationContext>();

var app = builder.Build();

//ApplicationContext db = new ApplicationContext();

app.MapGet("/api/Auditorium/{Id}", (ApplicationContext db, string id) =>
{
    using (db)
    {
        Auditorium? ans = db.Auditoriums.Find(int.Parse(id));
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

app.MapGet("/api/Auditoriums", (ApplicationContext db) =>
{
    using (db)
    {
        List<Auditorium> temp = new List<Auditorium>();
        temp = db.Auditoriums.ToList();
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

app.MapPost("/api/Auditorium", (ApplicationContext db, Auditorium data) =>
{
    using (db)
    {
        Auditorium? dublicat = db.Auditoriums.FirstOrDefault(u => u.Name == data.Name);
        if (dublicat == null)
        {
            db.Auditoriums.Add(data);
            db.SaveChanges();
            return Results.Ok();
        }
        else
        {
            return Results.NoContent();
        }
    }
});

app.MapPut("/api/Auditorium", (ApplicationContext db, Auditorium data) =>
{
    using (db)
    {
        Auditorium? temp = db.Auditoriums.Find(data.Id);
        if (temp == null)
        {
            return Results.NotFound();
        }
        else
        {
            temp.Name = data.Name;
            temp.NumberOfSeats = data.NumberOfSeats;
            temp.Description = data.Description;
            db.Auditoriums.Update(temp);
            db.SaveChanges();
            return Results.Ok();
        }
    }
});

app.MapDelete("/api/Auditorium/{Id}", (ApplicationContext db, string id) =>
{
    using (db)
    {
        var detail = db.Auditoriums.Attach(new Auditorium { Id = int.Parse(id) });
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

app.MapGet("/api/Building/{Id}", (ApplicationContext db, string id) =>
{
    using (db)
    {
        Building? assembly = db.Buildings.Find(int.Parse(id));
        if (assembly != null)
        {
            db.AuditoriumGroups.Where(u => u.BuildingId == assembly.Id).Load();
            List<AuditoriumGroup> temp = assembly.AuditoriumGroups.ToList();
            List<AuditoriumGroupView> auditoriumGroupViews = new List<AuditoriumGroupView>();
            foreach (var VARIABLE in temp)
            {
                db.Auditoriums.Where(u => u.Id == VARIABLE.AuditoriumId).Load();
            }

            for (int i = 0; i < temp.Count; i++)
            {
                AuditoriumGroupView auditoriumGroupView = new AuditoriumGroupView()
                {
                    BuildingId = temp[i].BuildingId,
                    Auditorium = temp[i].Auditorium,
                    Id = temp[i].Id,
                    Quantity = temp[i].Quantity,
                    AuditoriumId = temp[i].AuditoriumId,
                    // DetailName = temp[i].DetailName
                };
                auditoriumGroupViews.Add(auditoriumGroupView);
            }

            BuildingView ans = new BuildingView()
            {
                Id = assembly.Id,
                Name = assembly.Name,
                AuditoriumGroupViews = auditoriumGroupViews
            };
            return Results.Json(ans);
        }
        else
        {
            return Results.NotFound();
        }
    }
});

app.MapGet("/api/Buildings", (ApplicationContext db) =>
{
    using (db)
    {
        List<Building> data = new List<Building>();
        data = db.Buildings.Include(x => x.AuditoriumGroups).ThenInclude(u => u.Auditorium).ToList();
        if (data.Count != 0)
        {
            List<BuildingView> ansdata = new List<BuildingView>();
            foreach (var assembly in data)
            {
                List<AuditoriumGroup> temp = assembly.AuditoriumGroups.ToList();
                List<AuditoriumGroupView> auditoriumGroupViews = new List<AuditoriumGroupView>();
                for (int i = 0; i < temp.Count; i++)
                {
                    AuditoriumGroupView auditoriumGroupView = new AuditoriumGroupView()
                    {
                        BuildingId = temp[i].BuildingId,
                        Auditorium = temp[i].Auditorium,
                        Id = temp[i].Id,
                        Quantity = temp[i].Quantity,
                        AuditoriumId = temp[i].BuildingId,
                        // DetailName = temp[i].DetailName
                    };
                    auditoriumGroupViews.Add(auditoriumGroupView);
                }

                BuildingView ans = new BuildingView()
                {
                    Id = assembly.Id,
                    Name = assembly.Name,
                    AuditoriumGroupViews = auditoriumGroupViews
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

app.MapPost("api/Building", (ApplicationContext db, BuildingView data) =>
{
    using (db)
    {
        Building? dublicat = db.Buildings.FirstOrDefault(u => u.Name == data.Name);
        if (dublicat == null)
        {
            Building temp = new Building() { Id = data.Id, Name = data.Name };
            db.Buildings.Add(temp);
            db.SaveChangesAsync();
            List<AuditoriumGroup> tempAuditoriumGroups = new List<AuditoriumGroup>();
            foreach (var VARIABLE in data.AuditoriumGroupViews)
            {
                AuditoriumGroup auditoriumGroup = new AuditoriumGroup()
                {
                    Building = temp,
                    BuildingId = temp.Id,
                    Quantity = VARIABLE.Quantity,
                    // DetailName = VARIABLE.DetailName,
                    AuditoriumId = VARIABLE.AuditoriumId,
                    Auditorium = VARIABLE.Auditorium
                };
                tempAuditoriumGroups.Add(auditoriumGroup);
            }

            temp.AuditoriumGroups = tempAuditoriumGroups;
            db.AuditoriumGroups.AddRange(temp.AuditoriumGroups);
            db.Buildings.Update(temp);
            //db.Buildings.Add(temp);
            //db.AuditoriumGroupViews.AddRange(temp.AuditoriumGroupViews);
            db.SaveChanges();
            return Results.Ok();
        }
        else
        {
            return Results.NoContent();
        }
    }
});

app.MapPut("/api/Building", (ApplicationContext db, BuildingView data) =>
{
    using (db)
    {
        Building? building = db.Buildings.Include(x => x.AuditoriumGroups)
            .ThenInclude(u => u.Auditorium)
            .FirstOrDefault(x => x.Id == data.Id);
        building.Name = data.Name;
        List<AuditoriumGroup> oldparts = building.AuditoriumGroups.ToList();
        List<AuditoriumGroup> PartsToAdd = new List<AuditoriumGroup>();
        List<AuditoriumGroup> PartsToDelete = new List<AuditoriumGroup>();
        List<AuditoriumGroup> PartsToUpdate = new List<AuditoriumGroup>();
        for (int i = 0; i < data.AuditoriumGroupViews.Count; i++)
        {
            if (oldparts.Exists(u => u.Auditorium.Name == data.AuditoriumGroupViews[i].Auditorium.Name))
            {
                AuditoriumGroup auditoriumGroupOld = oldparts[oldparts.FindIndex(u => u.Auditorium.Name == data.AuditoriumGroupViews[i].Auditorium.Name)];
                auditoriumGroupOld.Quantity = data.AuditoriumGroupViews[i].Quantity;
                PartsToUpdate.Add(auditoriumGroupOld);
            }
            else
            {
                AuditoriumGroup auditoriumGroup = new AuditoriumGroup()
                {
                    Building = building,
                    BuildingId = building.Id,
                    Auditorium = db.Auditoriums.FirstOrDefault(u => u.Name == data.AuditoriumGroupViews[i].Auditorium.Name),
                    AuditoriumId = db.Auditoriums.FirstOrDefault(u => u.Name == data.AuditoriumGroupViews[i].Auditorium.Name).Id,
                    // DetailName = data.AuditoriumGroupViews[i].DetailName,
                    Quantity = data.AuditoriumGroupViews[i].Quantity
                };
                PartsToAdd.Add(auditoriumGroup);
            }
        }

        foreach (var VARIABLE in oldparts)
        {
            if (!data.AuditoriumGroupViews.Exists(u => u.Auditorium.Name == VARIABLE.Auditorium.Name))
            {
                PartsToDelete.Add(VARIABLE);
            }
        }

        if (PartsToUpdate.Count != 0)
        {
            oldparts = PartsToUpdate;
            db.AuditoriumGroups.UpdateRange(oldparts);
        }

        if (PartsToDelete.Count != 0)
        {
            db.AuditoriumGroups.RemoveRange(PartsToDelete);
        }

        if (PartsToAdd.Count != 0)
        {
            db.AuditoriumGroups.AddRange(PartsToAdd);
        }

        db.Buildings.Update(building);
        db.SaveChanges();
    }
});


app.MapDelete("/api/Building/{Id}", (ApplicationContext db, string id) =>
{
    using (db)
    {
        Building? assembly = db.Buildings.FirstOrDefault(u => u.Id == int.Parse(id));
        db.AuditoriumGroups.Where(u => u.BuildingId == assembly.Id).Load();
        if (assembly != null)
        {
            db.Buildings.Remove(assembly);
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

public class BuildingView
{
    public string Name { get; set; }
    public int Id { get; set; }
    public List<AuditoriumGroupView> AuditoriumGroupViews { get; set; } = new List<AuditoriumGroupView>();
}

public class AuditoriumGroupView
{
    public int Id { get; set; }

    public int BuildingId { get; set; }

    public int AuditoriumId { get; set; }

    // public string DetailName { get; set; } = null!;
    public Auditorium Auditorium { get; set; } = null!;
    public int Quantity { get; set; }
}