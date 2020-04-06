using Imdb.Data.Models.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Imdb.Services.Data.Contracts
{
    public interface IDirectorsService
    {
        Task AddAsync(string firstName, string lastName, Gender gender, DateTime? born, string imageUrl, string description);
    }
}
