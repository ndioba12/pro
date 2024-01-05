using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGRHBack.Database.Shared;
using SIGRHBack.Dtos.Authorization.Role;
using SIGRHBack.Dtos.Authorization.User;
using SIGRHBack.Dtos.Shared;
using SIGRHBack.Services.Authorization.User;
using System.ComponentModel.DataAnnotations;

namespace SIGRHBack.Controllers.Authorization
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _serviceUser;

        public UserController(IUserService serviceUser)
        {
            _serviceUser = serviceUser;
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateUser([FromBody] InputUserDto item)
        {
            if (ModelState.IsValid)
            {
                var result = await _serviceUser.CreateUser(item);
                if (result == null)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            return BadRequest(ModelState);
        }

        [HttpPut("{userId:guid}")]
        [Authorize(Roles = UserRoleNames.Admin+","+UserRoleNames.DirecteurServicesJudiciaires)]
        [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Update(string userId, [FromBody] InputUpdatedUserDto user)
        {
            if (ModelState.IsValid)
            {
                var response = await _serviceUser.UpdateUser<InputUpdatedUserDto>(userId, user);
                return response.IsSuccess?Ok(response): new ObjectResult(response){StatusCode = StatusCodes.Status500InternalServerError};
            }
            return BadRequest(ModelState);
        }

        [HttpPut("account")]
        [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateMyAccount([FromBody] InputUpdatedAccountDto user)
        {
            if (ModelState.IsValid)
            {
                var connectedUser = User.FindFirst("UserId").Value;
                var response = await _serviceUser.UpdateUser<InputUpdatedAccountDto>(connectedUser, user);
                return response.IsSuccess?Ok(response): new ObjectResult(response){StatusCode = StatusCodes.Status500InternalServerError};
            }
            return BadRequest(ModelState);
        }

        [HttpGet("{id:guid}", Name = "GetUserById")]
        //[Authorize(Roles = UserRoleNames.Admin + "," + UserRoleNames.DirecteurServicesJudiciaires)]
        [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetUserById(string id)
        {
            var result = await _serviceUser.GetUserById(id);
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        [HttpGet("account")]
        [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetMyAccount()
        {
            var connectedUser = User.FindFirst("UserId").Value;
            var result = await _serviceUser.GetUserById(connectedUser);
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        [HttpGet("{email}", Name = "GetUserByEmail")]
        [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetUserByEmail([EmailAddress] string email)
        {
            var result = await _serviceUser.GetUserByEmail(email);
            if (result != null)
                return Ok(result);
            return NotFound();
        }
        //[Authorize(Roles = "Admin")]
        [HttpGet]
        [ProducesResponseType(typeof(ResponseDto<IEnumerable<OutputUserDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetAll([FromQuery] ServiceMetier.GetAllUserFilter filter)
        {
            var result = await _serviceUser.GetAllUser(filter);
            return Ok(result);
        }
        //[Authorize(Roles = "Admin")]
        [HttpPut("activeoudesactiver/{id:guid}")]
        [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> ActiverOuDesactiverUser(string id)
        {
            if (ModelState.IsValid)
            {
                var result = await _serviceUser.ActiverOuDesactiverUser(id);
                return Ok(result);
            }
            return BadRequest(ModelState);
        }
        [HttpPut("updatepassword")]
        [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdatePassword(InputUserUpdatePassword input)
        {
            if (ModelState.IsValid)
            {
                input.IdUser = User.FindFirst("UserId").Value;
                var result = await _serviceUser.UpdatePassword(input);
                if (result != null)
                {
                    return Ok(result);
                }
            }
            return BadRequest();
        }
        //[Authorize(Roles = UserRoleNames.Admin)]
        [HttpPut("removerole/{id:guid}")]
        [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> RemoveToRole(string id, [FromBody] RoleDto role)
        {
            if (ModelState.IsValid)
            {
                var result = await _serviceUser.RemoveToRole(id, role);
                return Ok(result);
            }
            return BadRequest(ModelState);
        }
        //[Authorize(Roles = UserRoleNames.Admin)]
        [HttpPut("assignrole/{id:guid}")]
        [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AssignToRole(string id, [FromBody] RoleDto role)
        {
            if (ModelState.IsValid)
            {
                var result = await _serviceUser.AssignToRole(id, role);
                return Ok(result);
            }
            return BadRequest(ModelState);
        }
    }
}
