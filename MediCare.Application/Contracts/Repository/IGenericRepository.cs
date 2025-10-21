using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.Contracts.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(string spName);
        Task<T?>GetByIdAsync(string spName, string idParameterName, object id);
        Task<int>AddAsync(string spName,T entity);
        Task<int>UpdateAsync(string spName,T entity);
        Task<int>DeleteAsync(string spName, string idParameterName, object id);

    }
}
