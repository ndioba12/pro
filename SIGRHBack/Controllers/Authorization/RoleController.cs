using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SIGRHBack.Database.Shared;
using SIGRHBack.Dtos.Authorization.Role;
using SIGRHBack.Dtos.Shared;
using SIGRHBack.Services.Authorization.Role;

namespace SIGRHBack.Controllers.Authorization
{
    /// <summary>
    /// a controller for manage role to users / CRUD
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = UserRoleNames.Admin)]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        [HttpGet]
        [ProducesResponseType(typeof(ResponseDto<IEnumerable<RoleDto>>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetAll()
        {
            var result = await _roleService.GetAll();
            return Ok(result);
        }
        [HttpPost]
        [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Create(string name)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleService.CreateRole(name);
                return Ok(result);
            }
            return BadRequest(ModelState);
        }
        [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status201Created)]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(string id)
        {
            var result = await _roleService.Delete(id);
            return Ok(result);
        }
        [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Update(string id, string name)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleService.UpdateRole(id, name);
                return Ok(result);
            }
            return BadRequest();
        }
        [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult> GetOne(string id)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleService.GetOne(id);
                return Ok(result);
            }
            return BadRequest(ModelState);
        }
    }
}
