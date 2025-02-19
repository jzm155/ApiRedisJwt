using First_API_JWT_Redis.Application.Domain;
using First_API_JWT_Redis.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace First_API_JWT_Redis.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository) { 
            _userRepository = userRepository;
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <remarks>JSON Object</remarks>
        /// <param name="userDto">User data</param>
        /// <returns>New user</returns>
        /// <response code="204">Success</response>
        /// <response code="404">Not founded</response>
        [HttpPost]
        [Route("creating")]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> Post([FromBody] User userDto)
        {
            try
            {
                var user = new User(userDto.Name, userDto.Username, userDto.Password);
                var result = await _userRepository.Add(user);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new {message = ex.Message});
            }
            finally { }
        }

        /// <summary>
        /// login
        /// </summary>
        /// <remarks>JSON Object</remarks>
        /// <param name="model">User</param>
        /// <returns>Token acess</returns>
        /// <response code="204">Success</response>
        /// <response code="404">User not founded</response>
        /// <response code="404">User not founded</response>
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> Authenticate([FromBody] User model)
        {
            var user = await _userRepository.GetUserByUsername(model.Username);
            if (user == null) {
                return NotFound(new { message = "Usuario não encontrado" });
            }

            if (user.Password != model.Password)
            {
                return NotFound(new { message = "Password Incorreto" });
            }

            var token = TokenService.GenerateToken(user);

            return Ok(new {Token = token });

        }
    }
}
