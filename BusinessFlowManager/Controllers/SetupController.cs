using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BusinessFlowManager.Controllers
{
    public class SetupController : Controller
    {
        public ActionResult Index()
        {
            return View ();
        }

		public ActionResult Fields()
		{
			return View();
		}
    }
}
