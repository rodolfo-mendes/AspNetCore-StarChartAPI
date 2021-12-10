using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StarChart.Models;

namespace StarChart.Data
{
    interface ICelestialObjectRepository
    {
        CelestialObject GetById(int id);

        List<CelestialObject> GetByOrbitedObjectId(int id);

        List<CelestialObject> GetByName(string name);

        List<CelestialObject> GetAll();

        void Create(CelestialObject celestialObject);

        void Update(CelestialObject celestialObject);

        void Delete(CelestialObject objectToBeDeleted);

        void DeleteAll(List<CelestialObject> objectsToBeDeleted);
    }
}
