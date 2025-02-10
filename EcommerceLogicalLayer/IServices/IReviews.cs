
using EcommerceDataLayer.DTOS;

namespace EcommerceDataLayer.IRopesitry
{
    public interface IReviews
    {
        public List<Review> GetAll();


        public void Add(ReviewDTOForCreate review);

        public void Delete(int reviewID);

        public void Update(ReviewDTOForUpdate review);

    }
}
