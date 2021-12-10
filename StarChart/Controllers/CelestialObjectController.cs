using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [ApiController]
    [Route("")]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var celestialObject = _context.CelestialObjects.Find(id);
            if (celestialObject == null)
            {
                return NotFound();
            }

            celestialObject.Satellites = _context.CelestialObjects
                .Where(c => c.OrbitedObjectId == id)
                .ToList();

            return Ok(celestialObject);
        }

        [HttpGet("{name}", Name = "GetByName")]
        public IActionResult GetByName(string name)
        {
            var celestialObjects = _context.CelestialObjects
                .Where(o => o.Name == name)
                .ToList();
            if (celestialObjects == null || celestialObjects.Count == 0)
            {
                return NotFound();
            }

            foreach (var obj in celestialObjects)
            {
                obj.Satellites = _context.CelestialObjects
                    .Where(o => o.OrbitedObjectId == obj.Id)
                    .ToList();
            }

            return Ok(celestialObjects);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var celestialObjects = _context.CelestialObjects.ToList();
            foreach (var obj in celestialObjects)
            {
                obj.Satellites = _context.CelestialObjects
                    .Where(o => o.OrbitedObjectId == obj.Id)
                    .ToList();
            }

            return Ok(celestialObjects);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CelestialObject celestialObject)
        {
            _context.CelestialObjects.Add(celestialObject);
            _context.SaveChanges();

            return CreatedAtRoute("GetById", new {id = celestialObject.Id}, celestialObject);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] CelestialObject celestialObject)
        {
            var oldCelestialObject = _context.CelestialObjects.Find(id);
            if (oldCelestialObject == null)
            {
                return NotFound();
            }

            oldCelestialObject.Name = celestialObject.Name;
            oldCelestialObject.OrbitalPeriod = celestialObject.OrbitalPeriod;
            oldCelestialObject.OrbitedObjectId = celestialObject.OrbitedObjectId;

            _context.CelestialObjects.Update(oldCelestialObject);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var celestialObject = _context.CelestialObjects.Find(id);
            if (celestialObject == null)
            {
                return NotFound();
            }

            celestialObject.Name = name;

            _context.CelestialObjects.Update(celestialObject);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var objectsToBeDeleted = _context.CelestialObjects
                .Where(o => (o.Id == id || o.OrbitedObjectId == id))
                .ToList();

            if (objectsToBeDeleted == null || objectsToBeDeleted.Count == 0)
            {
                return NotFound();
            }

            _context.CelestialObjects.RemoveRange(objectsToBeDeleted);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
