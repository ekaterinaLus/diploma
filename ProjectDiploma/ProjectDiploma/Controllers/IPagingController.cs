using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ProjectDiploma.Controllers
{
    public interface IPagingController<T> where T: class
    {
        IEnumerable<T> GetPage([FromQuery] int pageIndex, [FromQuery] int pageSize);
        int GetCount();
    }
}
