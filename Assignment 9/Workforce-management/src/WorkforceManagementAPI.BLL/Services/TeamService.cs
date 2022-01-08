using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkforceManagementAPI.DAL;

namespace WorkforceManagementAPI.BLL.Service
{
    public class TeamService
    {
        private readonly DatabaseContext _context;

        public TeamService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateTeamAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> EditTeamAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteTeamAsync()
        {
            throw new NotImplementedException();
        }
    }
}
