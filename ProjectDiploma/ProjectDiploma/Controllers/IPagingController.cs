using System.Collections.Generic;

namespace ProjectDiploma.Controllers
{
    public interface IPagingController<T> where T: class
    {
        IEnumerable<T> GetPage(int pageIndex, int pageSize);
    }
}
