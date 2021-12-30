using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace MyGarage
{
    class GarageManager 
    {
        private Garage<Vehicle> _garage;

        public Garage<Vehicle> Garage
        {
            get { return _garage; } 
        }

        public GarageManager(int size)
        {
            _garage = new Garage<Vehicle>(size);
        }

        public bool ParkVehicle(Vehicle vehicle)
        {
            return _garage.Add(vehicle);
        }

        public bool DriveOut(string regNum)
        {
            var found = FindVehicleByRegNum(regNum);
            return found is null ? false : _garage.Remove(found);
            //return _garage.Remove(regNum);
        }

        public string[] FindVehicleByString(string keyword)
        {
            return _garage.Where(vehicle => vehicle.Matches(keyword))
                            .Select(vehicle => $"{vehicle}")
                            .ToArray();
            //return _garage.FindByString(keyword);
        }

        public Vehicle FindVehicleByRegNum(string regNum)
        {
            return _garage.FirstOrDefault(v => v.RegNum == regNum);
            //return _garage.FindByRegNum(regNum);
        }

        public IEnumerable<Vehicle> FindVehicleByType(string type)
        {
            return _garage.Where(v => v.GetType().Name == type);

            //List<Vehicle> result = new List<Vehicle>();
            //foreach (Vehicle v in _garage)
            //    if (v.GetType().Name == type)
            //        result.Add(v);

            //return result.ToArray();
        }

        //public bool TestM(Vehicle v)
        //{
        //    return v.Color == "Red";
        //}

        public string[] GetAllVehicles()
        {
            //var res = _garage.Where(v => v.Color == "Red");
                    
           // var res2 = _garage.Where(TestM);

           // return _garage.Select(v => string.Format($"P-plats  {v.ToString()}")).ToArray();

            Vehicle[] vehicle = _garage.ToArray();
            string[] result = new string[_garage.Count()];

            for (int i = 0; i < vehicle.Length; i++)
                if (vehicle[i] != null)
                    result[i] = string.Format("P-plats {0}: {1}", i + 1, vehicle[i]);

            return result;
        }

        public string[] GetAllSpaces()
        {
            Vehicle[] vehicle = _garage.GetAll();
            string[] result = new string[vehicle.Length];
            for (int i = 0; i < vehicle.Length; i++)
                if (vehicle[i] == null)
                    result[i] = string.Format("P-plats {0} är tom.", i + 1);
                else
                    result[i] = string.Format("P-plats {0}: {1}", i + 1, vehicle[i]);

            return result;

        }

        public string GetStatistics()
        {

            //var res = _garage.GroupBy(v => v.GetType().Name)
            //                 .Select(v => new Stats()
            //                 {
            //                     Name = v.Key,
            //                     NrOfVheicles = v.Count()
            //                 })
            //                 .Select(s => $"Name: {s.Name} Count: {s.NrOfVheicles}");
                             

            Dictionary<string, int> types = new Dictionary<string, int>();
            Vehicle[] vehicle = _garage.GetAll();
            int n;

            for (int i = 0; i < vehicle.Length; i++)
            {
                if (vehicle[i] != null)
                {
                    string typeName = vehicle[i].GetType().Name;
                    if (types.ContainsKey(typeName))
                        types[typeName] += 1;
                    else
                        types[typeName] = 1;
                }
            }

            return string.Format(
                "Antal platser:    {1}st{0}" +
                "S:a antal fordon: {2}st{0}" +
                "Bilar:            {3}st{0}" +
                "Flygplan:         {4}st{0}" +
                "Motorcyklar:      {5}st{0}" +
                "Bussar:           {6}st{0}" +
                "Båtar:            {7}st",
                Environment.NewLine,
                _garage.Size,
                _garage.Count(),
                types.TryGetValue("Car", out n) ? n : 0,
                types.TryGetValue("Airplane", out n) ? n : 0,
                types.TryGetValue("Motorcycle", out n) ? n : 0,
                types.TryGetValue("Bus", out n) ? n : 0,
                types.TryGetValue("Boat", out n) ? n : 0
                );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool HasFreeSpace()
        {
            return _garage.Count() < _garage.Size;
        }

        /// <summary>
        /// Saves a garage to file.
        /// </summary>
        public void SaveGarage()
        {
            _garage.Serialize();
        }

        /// <summary>
        /// Loads a garage form file.
        /// </summary>
        public void LoadGarage()
        {
            Garage<Vehicle> g = _garage.DeSerialize();
            _garage = g;
        }
    }
}
