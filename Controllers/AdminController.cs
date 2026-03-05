using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Project_dotnetcore.Controllers
{
    public class AdminController : Controller
    {
        // GET: AdminController
        public ActionResult Dashboard()
        {
            return View();
        }

        // GET: AdminController/Details/5
        public ActionResult CategoryMaster()
        {
            return View();
        }

        public ActionResult UserMaster()
        {
            return View();
        }

       
        public ActionResult Products()
        {
            return View();
        }


    }
}
