using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyForce
{
    public class MemberShipService
    {
        private readonly IUnitOfWork _unitOfWork;
        public MemberShipService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<MembersDto>> GetAllMembers()
        {

            var allUsers = (await _unitOfWork.Repository<User>().GetAllAsync()).AsQueryable();
            var allMembers = (await _unitOfWork.Repository<MemberShip>().GetAllAsync()).AsQueryable();
            
            var result = from u in allUsers
                         join m in allMembers on u.Id equals m.UserId
                         select new MembersDto
                         {
                             MemberId = m.MemberShipId,
                             Name = u.FirstName + " " + u.LastName,
                             PhoneNo = u.PhoneNumber,
                             DOJ = u.DOJ,
                             MembershipStatus = m.Status == true ? "Active" : "Inactive",
                             MembershipStartDate = m.StartDate,
                             MembershipEndDate = m.EndDate                             
                         };
        }
    }
}
