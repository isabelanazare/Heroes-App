using Fabrit.Heroes.Business.Services.Contracts;
using Fabrit.Heroes.Data;
using Fabrit.Heroes.Data.Entities;
using Fabrit.Heroes.Data.Entities.Exception;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fabrit.Heroes.Business.Services
{
    public class PowerService : IPowerService
    {
        private readonly HeroesDbContext _context;

        public PowerService(HeroesDbContext context)
        {
            _context = context;
        }

        public IAsyncEnumerable<Power> GetAll()
        {
            return _context.Powers
                .AsAsyncEnumerable();
        }

        public async Task<Power> GetPower(int id)
        {
            Power power = await _context.Powers
                               .SingleOrDefaultAsync(e => e.Id == id);
            if (power == null)
            {
                throw new EntityNotFoundException("Power not found");
            }

            return power;
        }

        public async Task<Power> GetPowerByName(string name)
        {
            Power power = await _context.Powers
                               .FirstOrDefaultAsync(e => e.Name == name);
            if (power == null)
            {
                throw new EntityNotFoundException("Power not found");
            }

            return power;
        }

        private void ValidatePower(Power power)
        {
            if (String.IsNullOrWhiteSpace(power.Name) || String.IsNullOrWhiteSpace(power.Element) || String.IsNullOrWhiteSpace(power.Details) || String.IsNullOrWhiteSpace(power.MainTrait))
            {
                throw new InvalidEntityException("Power is invalid");
            }
        }

        public async Task<Power> AddPower(Power power)
        {
            _context.Powers.Add(power);
            await _context.SaveChangesAsync();

            return power;
        }

        public async Task<Power> UpdatePower(Power power)
        {
            var dbPower = await _context.Powers.AsNoTracking().FirstOrDefaultAsync(power => power.Id == power.Id);

            if (dbPower == null)
            {
                throw new EntityNotFoundException("Power doesn't exist");
            }

            ValidatePower(power);

            _context.Powers.Update(power);
            await _context.SaveChangesAsync();

            return power;
        }

        public async Task DeletePower(int id)
        {
            var power = await _context.Powers.FirstOrDefaultAsync(p => p.Id == id);

            if (power == null)
            {
                throw new EntityNotFoundException("Power doesn't exist");
            }

            _context.Powers.Remove(power);
            await _context.SaveChangesAsync();
        }
    }
}

