using library_sesterm.DTOs;
using library_sesterm.Models;
using library_sesterm.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace library_sesterm.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetAllUsers()
        {
            var users = await _userRepository.GetAllUsersAsync();
            var userResponseDtos = new List<UserResponseDto>();
            foreach (var user in users)
            {
                userResponseDtos.Add(new UserResponseDto
                {
                    Id = user.Id,
                    Nic = user.Nic,
                    Name = user.Name,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Address = user.Address,
                    Role = user.Role
                });
            }
            return Ok(userResponseDtos);
        }

        // POST: api/users
        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] UserRequestDto userRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new User
            {
                Nic = userRequest.Nic,
                Name = userRequest.Name,
                Email = userRequest.Email,
                PhoneNumber = userRequest.PhoneNumber,
                Address = userRequest.Address,
                Role = userRequest.Role
            };

            await _userRepository.AddUserAsync(user);
            return CreatedAtAction(nameof(GetAllUsers), new { id = user.Id }, user);
        }

        // PUT: api/users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserRequestDto userRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingUser = await _userRepository.GetUserByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            existingUser.Nic = userRequest.Nic;
            existingUser.Name = userRequest.Name;
            existingUser.Email = userRequest.Email;
            existingUser.PhoneNumber = userRequest.PhoneNumber;
            existingUser.Address = userRequest.Address;
            existingUser.Role = userRequest.Role;

            await _userRepository.UpdateUserAsync(existingUser);
            return NoContent();
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            await _userRepository.DeleteUserAsync(id);
            return NoContent();
        }
    }
}
