

namespace EcommerceDataLayer.IRopesitry
{
    public interface IUpdatetableRopesitry< T> where T : class
    {
        Task<int> Updatet(T item);
    }
}
