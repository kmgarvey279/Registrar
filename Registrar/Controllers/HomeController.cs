using Microsoft.AspNetCore.Mvc;
using Registrar.Models;
using System.Collections.Generic;

namespace Registrar.Controllers
{
  public class HomeController : Controller
  {
    [HttpGet("/")]
    public ActionResult Index()
    {
      return View();
    }
  }
}
