using gamestore.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gamestore.dataacess.Repository.IRepository
{
    public interface ICoverTypeReposirory : IRepository<CoverType>
    {
        void Update(CoverType obj);

    }
}
