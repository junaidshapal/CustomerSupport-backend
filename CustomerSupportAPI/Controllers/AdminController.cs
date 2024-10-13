using CustomerSupportAPI.Data;
using CustomerSupportAPI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CustomerSupportAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        //private readonly UserManager<User> _userManager;

        public AdminController( ApplicationDbContext context)
        {
            _context = context;
            //_userManager = userManager;
        }

       
    }
}
