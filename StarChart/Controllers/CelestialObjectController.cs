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
        private readonly ICelestialObjectRepository _repository;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _repository = new CelestialObjectEntityRepository(context);
        }

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var celestialObject = _repository.GetById(id);
            if (celestialObject == null)
            {
                return NotFound();
            }

            celestialObject.Satellites = _repository.GetByOrbitedObjectId(id);

            return Ok(celestialObject);
        }

        [HttpGet("{name}", Name = "GetByName")]
        public IActionResult GetByName(string name)
        {
            var celestialObjects = _repository.GetByName(name);
            if (celestialObjects == null || celestialObjects.Count == 0)
            {
                return NotFound();
            }

            foreach (var obj in celestialObjects)
            {
                obj.Satellites = _repository.GetByOrbitedObjectId(obj.Id);
            }

            return Ok(celestialObjects);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var celestialObjects = _repository.GetAll();
            foreach (var obj in celestialObjects)
            {
                obj.Satellites = _repository.GetByOrbitedObjectId(obj.Id);
            }

            return Ok(celestialObjects);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CelestialObject celestialObject)
        {
            _repository.Create(celestialObject);

            return CreatedAtRoute("GetById", new {id = celestialObject.Id}, celestialObject);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] CelestialObject celestialObject)
        {
            var oldCelestialObject = _repository.GetById(id);
            if (oldCelestialObject == null)
            {
                return NotFound();
            }

            oldCelestialObject.Name = celestialObject.Name;
            oldCelestialObject.OrbitalPeriod = celestialObject.OrbitalPeriod;
            oldCelestialObject.OrbitedObjectId = celestialObject.OrbitedObjectId;

            _repository.Update(oldCelestialObject);

            return NoContent();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var celestialObject = _repository.GetById(id);
            if (celestialObject == null)
            {
                return NotFound();
            }

            celestialObject.Name = name;

            _repository.Update(celestialObject);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var objectToBeDeleted = _repository.GetById(id);
            if (objectToBeDeleted == null)
            {
                return NotFound();
            }

            var objectsInOrbit = _repository.GetByOrbitedObjectId(id);

            _repository.DeleteAll(objectsInOrbit);
            _repository.Delete(objectToBeDeleted);

            return NoContent();
        }
    }
}
