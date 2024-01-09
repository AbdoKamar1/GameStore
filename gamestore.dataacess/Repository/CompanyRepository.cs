using gamestore.dataacess.Data;
using gamestore.dataacess.Repository.IRepository;
using gamestore.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gamestore.dataacess.Repository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private readonly ApplicatioonDbContext _db;
        public CompanyRepository(ApplicatioonDbContext db) : base(db)
        {
            _db = db;
        }


        public void Update(Company obj)
        {
            _db.Companies.Update(obj);
        }
    }
}
