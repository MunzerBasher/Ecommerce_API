using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceDataLayer.IRopesitry
{
    public interface IRopesitry<T> where T : class
    {
        Task<int> Add(T item);

        Task<IEnumerable<T>> GetAll();

        Task<T> GetById(T item);
    }


}
