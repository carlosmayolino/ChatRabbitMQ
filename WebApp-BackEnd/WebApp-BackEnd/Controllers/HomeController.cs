using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebApp_BackEnd.Controllers
{
    [ApiController]
    [Route("api/{controller}")]
    public class HomeController : Controller
    {
        public string Index()
        {
            return "Olá Mundo!!";
        }


        
    }
}