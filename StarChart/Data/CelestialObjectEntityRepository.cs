using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StarChart.Models;

namespace StarChart.Data
{
    public class CelestialObjectEntityRepository : ICelestialObjectRepository
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectEntityRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        void ICelestialObjectRepository.Create(CelestialObject celestialObject)
        {
            _context.CelestialObjects.Add(celestialObject);
            _context.SaveChanges();
        }

        void ICelestialObjectRepository.Delete(CelestialObject objectToBeDeleted)
        {
            _context.CelestialObjects.Remove(objectToBeDeleted);
            _context.SaveChanges();
        }

        void ICelestialObjectRepository.DeleteAll(List<CelestialObject> objectsToBeDeleted)
        {
            _context.CelestialObjects.RemoveRange(objectsToBeDeleted);
            _context.SaveChanges();
        }

        List<CelestialObject> ICelestialObjectRepository.GetAll()
        {
            return _context.CelestialObjects.ToList();
        }

        CelestialObject ICelestialObjectRepository.GetById(int id)
        {
            return _context.CelestialObjects.Find(id);
        }

        List<CelestialObject> ICelestialObjectRepository.GetByName(string name)
        {
            return _context.CelestialObjects
                .Where(o => o.Name == name)
                .ToList();
        }

        List<CelestialObject> ICelestialObjectRepository.GetByOrbitedObjectId(int id)
        {
            return _context.CelestialObjects
                .Where(c => c.OrbitedObjectId == id)
                .ToList();
        }

        void ICelestialObjectRepository.Update(CelestialObject celestialObject)
        {
            _context.CelestialObjects.Update(celestialObject);
            _context.SaveChanges();
        }
    }
}
