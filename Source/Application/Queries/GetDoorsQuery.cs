using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using Domain.Dto;
using MediatR;
using System.Linq.Expressions;

namespace Application.Queries
{
    public record GetDoorsQuery : IRequest<List<DoorDto>>
    {
        public long UserId { get; set; }
    }

    public class GetDoorsQueryHandler : IRequestHandler<GetDoorsQuery, List<DoorDto>>
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<RoleDoorMapping> _roleDoorRepository;
        private readonly IMapper _mapper;
        public GetDoorsQueryHandler(IRepository<User> repository, IRepository<RoleDoorMapping> roleDoorRepository, IMapper mapper)
        {
            _userRepository = repository;
            _roleDoorRepository = roleDoorRepository;
            _mapper = mapper;
        }

        public async Task<List<DoorDto>> Handle(GetDoorsQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null) 
            {
                return new List<DoorDto> { };
            }

            Expression<Func<RoleDoorMapping, bool>> roleCompare = x => x.RoleId == user.RoleId;
            return  (await _roleDoorRepository
                        .GetAsync(roleCompare, nameof(RoleDoorMapping.Door)))
                        .Select(x => x.Door)
                        .ProjectTo<DoorDto>(_mapper.ConfigurationProvider)
                        .ToList();
        }
    }
}
