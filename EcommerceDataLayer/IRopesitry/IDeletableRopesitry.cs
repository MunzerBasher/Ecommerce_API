using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceDataLayer.IRopesitry
{
    public interface IDeletableRopesitry<T> where T : class
    {
        int DeletableRopesitry(T item);


    }
}
