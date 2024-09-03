using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalExercise.Models;

namespace TechnicalExercise.Services
{
    internal interface IAirportDatabase
    {
        Task<IList<AirportModel>> GetByCountry(string countryCode);
        IList<AirportModel> LondonAirports { get; }
    }
}
