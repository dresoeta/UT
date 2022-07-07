using System;
using System.Threading.Tasks;
using DigitalEnvision.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using UT.Dto;
using UT.Helpers;
using UT.Interface;

namespace UT.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepository;

        public AccountController(IConfiguration config, IUserRepository _userRepository)
        {
            this._config = config;
            this._userRepository = _userRepository;
        }

        [HttpGet("list-user")]
        public async Task<IActionResult> ListUser()
        {
            var user = await _userRepository.GetAllUserAsync();

            return Ok(user);
        }

        [HttpPost("post-user")]
        public async Task<IActionResult> PostUser(PostUserDto Dto)
        {
            User UserTable = new User();

            #region check location

            var check = Extentions.CheckLocation(Dto.Location);

            if(check.success != true) return BadRequest( new
            {
                message = "Location not found",
                success = 0
            });

            #endregion check location

            UserTable.Firstname = Dto.Firstname;
            UserTable.Lastname = Dto.Lastname;
            UserTable.DateOfBirth = Convert.ToDateTime(Dto.DateOfBirth);
            UserTable.Location = Dto.Location;

            _userRepository.Insert(UserTable);

            if(await _userRepository.SaveAllAsync())return Ok(new
            {
                message = "User Added",
                success = 1
            });

            return StatusCode(500, new
            {
                message = "Something went wrong",
                success = 0
            });

        }

        [HttpPut("update-user/{Id}")]
        public async Task<IActionResult> UpdateUser(int Id, PostUserDto Dto)
        {
            var User = await _userRepository.GetUserByIdAsync(Id);

            #region check location

            if (Dto.Location != null)
            {
                var check = Extentions.CheckLocation(Dto.Location);

                if (check.success != true) return BadRequest(new
                {
                    message = "Location not found",
                    success = 0
                });

                User.Location = Dto.Location;
            }

            #endregion check location

            if (Dto.Firstname != null) User.Firstname = Dto.Firstname;
            if (Dto.Lastname != null) User.Lastname = Dto.Lastname;
            if (Dto.DateOfBirth != null) User.DateOfBirth = Convert.ToDateTime(Dto.DateOfBirth);

            _userRepository.Update(User);

            if (await _userRepository.SaveAllAsync()) return Ok(new
            {
                message = "User Updated",
                success = 1
            });

            return StatusCode(500, new
            {
                message = "Something went wrong",
                success = 0
            });
        }

        [HttpDelete("delete-user/{Id}")]
        public async Task<IActionResult> UpdateUser(int Id)
        {
            var User = await _userRepository.GetUserByIdAsync(Id);

            if(User != null) _userRepository.Delete(User);


            if (await _userRepository.SaveAllAsync()) return Ok(new
            {
                message = "User Deleted",
                success = 1
            });

            return StatusCode(500, new
            {
                message = "Something went wrong",
                success = 0
            });
        }

    }
}
