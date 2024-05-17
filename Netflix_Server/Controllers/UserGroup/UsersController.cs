using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Netflix_Server.Models.Context;
using Netflix_Server.Models.UserGroup;
using Netflix_Server.IRepository;
using Netflix_Server.Services.UserGroup;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.ComponentModel.DataAnnotations;


namespace Netflix_Server.Controllers.UserGroup
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IRepository<User> _repository;
        private readonly IUserAuthentication _authService;

        public UsersController(IRepository<User> repository, IUserAuthentication authService)
        {
            _repository = repository;
            _authService = authService;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            return await _repository.GetList();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _repository.GetById(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _repository.Update(user);
            await _repository.Save();

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(string email, string password, int pricingPlanId)
        {
            try
            {
                var user = await _authService.RegisterUserAsync(email, password, pricingPlanId);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _repository.Delete(id);
            await _repository.Save();

            return NoContent();
        }



        [Route("auth/register")]
        [HttpPost]
        public async Task<ActionResult<User>> Register([FromBody] RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _authService.RegisterUserAsync(model.Email, model.Password, Convert.ToInt32(model.PricingPlanId));
                    return Ok(user);
                }
                catch (Exception ex)
                {
                    return BadRequest(new { message = ex.Message });
                }
            }

            return BadRequest(ModelState);
        }


        [Route("auth/login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _authService.AuthenticateUserAsync(model.Email, model.Password);
                    return Ok(user);
                }
                catch (Exception ex)
                {
                    return BadRequest(new { message = ex.Message });
                }
            }

            return BadRequest(ModelState);
        }


        [Route("auth/check-email")]
        [HttpPost]
        public async Task<IActionResult> CheckEmail([FromBody] EmailRequest request)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var response = await _authService.IsEmailNotExist(request.Email);

                    if (response)
                    {
                        return Ok(response);
                    }
                    else
                    {
                        return BadRequest(new { message = "User with this email is already registered." });
                    }

                }
                catch (Exception ex)
                {
                    return BadRequest(new { message = ex.Message });
                }
            }

            return BadRequest(ModelState);
        }
    }
}
