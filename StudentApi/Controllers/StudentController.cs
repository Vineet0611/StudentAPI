using Cortex.Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentApi.Features.StudentFeatures.AddStudent;
using StudentApi.Features.StudentFeatures.GetAllStudents;
using StudentApi.Features.StudentFeatures.UpdateStudent;
using StudentApi.Features.StudentFeatures.DeleteStudent;
using StudentApi.Features.StudentFeatures;
using Microsoft.AspNetCore.Authorization;

namespace StudentApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController(IMediator _mediator) : ControllerBase
    {
        [Authorize]
        [HttpPost("")]
        [ProducesResponseType(typeof(AddStudentResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AddStudentResponseModel>> AddStudent([FromBody] AddStudentRequestModel request)
        {
            var result = await _mediator.SendCommandAsync<AddStudentRequestModel, AddStudentResponseModel>(request);
            return StatusCode(result.status, result);
        }

        [Authorize]
        [HttpGet("")]
        [ProducesResponseType(typeof(GetAllStudentsResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GetAllStudentsResponseModel>> GetAllStudents()
        {
            var request = new GetAllStudentsRequestModel();
            var result = await _mediator.SendCommandAsync<GetAllStudentsRequestModel, GetAllStudentsResponseModel>(request);
            return StatusCode(result.status, result);
        }

        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(UpdateStudentResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UpdateStudentResponseModel>> UpdateStudent(Guid id, [FromBody] StudentInputRepresentationModel student)
        {
            var request = new UpdateStudentRequestModel { id = id, student = student };
            var result = await _mediator.SendCommandAsync<UpdateStudentRequestModel, UpdateStudentResponseModel>(request);
            return StatusCode(result.status, result);
        }
        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(DeleteStudentResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DeleteStudentResponseModel>> DeleteStudent(Guid id)
        {
            var request = new DeleteStudentRequestModel { id = id };
            var result = await _mediator.SendCommandAsync<DeleteStudentRequestModel, DeleteStudentResponseModel>(request);
            return StatusCode(result.status, result);
        }
    }
}
