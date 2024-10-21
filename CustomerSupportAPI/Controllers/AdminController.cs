using CustomerSupportAPI.Data;
using CustomerSupportAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomerSupportAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public AdminController( ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("get-all-users")]
        public async Task<IActionResult> GetAllUsers()
        {

            var allUsers = await _userManager.Users.ToListAsync();

            var users = new List<object>();

            foreach (var user in allUsers)
            {
                if (!await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    users.Add(new
                    {
                        user.Id,
                        user.UserName,
                        user.Email,
                        user.IsApproved
                    });
                }
            }

            return Ok(users);
        }


        [HttpPost("approve-user/{userId}")]
       
        public async Task<IActionResult> ApproveUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            
                return NotFound("User not found");
            

            user.IsApproved = true;
            await _userManager.UpdateAsync(user);


            return Ok(new { message = "User approved Successfully" }  );
        }


        [HttpPost("block-user/{userId}")]

        public async Task<IActionResult>BlockUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            user.IsApproved= false;

            await _userManager.UpdateAsync(user);

            return Ok("User blocked");
        }
    }

}
