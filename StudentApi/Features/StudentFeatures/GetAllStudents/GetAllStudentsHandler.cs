using Core.Entities;
using Cortex.Mediator.Commands;
using Infastructure.Repository.Base;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace StudentApi.Features.StudentFeatures.GetAllStudents
{
    public class GetAllStudentsHandler(
        IRepository<Student> studentRepository,
        IMapper _mapper
        ) : ICommandHandler<GetAllStudentsRequestModel, GetAllStudentsResponseModel>
    {
        public async Task<GetAllStudentsResponseModel> Handle(GetAllStudentsRequestModel request, CancellationToken cancellationToken)
        {
            var response = new GetAllStudentsResponseModel();
            
            var students = await studentRepository.GetAllQuery()
                                                  .Where(x=>x.status == Core.Enum.EntityStatus.Active)
                                                  .ToListAsync(cancellationToken);
            
            response.Students = _mapper.Map<List<StudentRepresentationModel>>(students);
            response.status = 200;
            response.message = "Students retrieved successfully.";
            
            return response;
        }
    }
}
