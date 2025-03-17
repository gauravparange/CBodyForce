using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyForce
{
    public class MemberShipService : IMemberShipService
    {
        private readonly IUnitOfWork _unitOfWork;
        public MemberShipService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<MembersDto>> GetAllMembers()
        {

            var result = from u in (await _unitOfWork.Repository<User>().GetAllAsync())
                         join m in (await _unitOfWork.Repository<MemberShip>().GetAllAsync()) on u.Id equals m.UserId
                         select new MembersDto
                         {
                             MemberId = m.MemberShipId,
                             Name = u.FirstName + " " + u.LastName,
                             MemberCode = u.UserName,
                             PhoneNo = u.PhoneNumber,
                             DOJ = u.DOJ,
                             MembershipStatus = m.Status == true ? "Active" : "Inactive",
                             MembershipStartDate = m.StartDate,
                             MembershipEndDate = m.EndDate,
                             UserId = m.UserId,
                         };
            return result;
        }
        public async Task<SignUpDto> GetMember(int UserId)
        {

            var result = (from u in (await _unitOfWork.Repository<User>().GetAllAsync())                         
                         where u.Id == UserId 
                         select new SignUpDto
                         {
                             FirstName = u.FirstName,
                             LastName = u.LastName,
                             Email = u.Email,
                             PhoneNo = u.PhoneNumber,
                             ParentPhoneNo = u.ParentPhoneNo,
                             DOB = u.DOB,
                             Address = u.Address,
                             Height = u.Height,
                             Weight = u.Weight,
                             Username = u.UserName
                         }).FirstOrDefault();
            return result;
        }
        
    }
}
