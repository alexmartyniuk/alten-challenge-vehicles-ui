using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VehiclesUI.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string VehicleId { get; set; }
        public string RegistrationNumber { get; set; }
        public bool Connected { get; set; }
        public Customer Customer { get; set; }        
    }
}
