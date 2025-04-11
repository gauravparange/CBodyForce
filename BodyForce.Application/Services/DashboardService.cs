
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BodyForce
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemberShipService _memberShipService;
        public DashboardService(IUnitOfWork unitOfWork, IMemberShipService memberShipService)
        {
            _unitOfWork = unitOfWork;
            _memberShipService = memberShipService;
        }
        public async Task<DashboardViewModel> GetDashboardCount()
        {

            var membershipRepo = _unitOfWork.Repository<MemberShip>();
            var paymentRepo = _unitOfWork.Repository<Payment>();

            // Count of active and not deleted memberships
            var membershipCount = await membershipRepo.CountAsync(m => !m.IsDeleted && m.Status);

            // Count of memberships due for renewal
            var subscriptionRenewalCount = await membershipRepo.CountAsync(s =>
                !s.IsDeleted &&
                s.RenewalDate.HasValue &&
                s.RenewalDate.Value.Date < DateTime.Now.Date);

            // Count of pending payments for active memberships
            var memberships = await membershipRepo.GetByConditionAsync(m => !m.IsDeleted && m.Status);
            var memberIds = memberships.Select(m => m.MemberShipId).ToList();

            var pendingPaymentsCount = await paymentRepo.CountAsync(p =>
                !p.IsDeleted &&
                p.PaymentMethod.ToUpper() == "PENDING" &&
                memberIds.Contains(p.MemberShipId));

            return new DashboardViewModel
            {
                ActiveMembershipsCount = membershipCount,
                SubscriptionRenewalCount = subscriptionRenewalCount,
                PendingPaymentsCount = pendingPaymentsCount
            };

        }
        public async Task<IEnumerable<MembersDto>> GetList(string category)
        {
            IEnumerable<MembersDto> data =   await _memberShipService.GetAllMembers();

            switch (category)
            {
                case "ActiveMemberships":
                    data = data.Where(m => m.MembershipStatus == true);
                    break;

                case "SubscriptionRenewals":
                    data = data.Where(s => s.RenewalDate.HasValue && s.RenewalDate.Value.Date < DateTime.Now.Date);
                    break;

                case "PendingPayments":
                    var memberships = data.Where(m => m.MembershipStatus == true);
                    var memberIds = memberships.Select(m => m.MemberId).ToList();

                    var payments = await _unitOfWork.Repository<Payment>().GetByConditionAsync(p =>
                        !p.IsDeleted && p.PaymentMethod.ToUpper() == "PENDING" && memberIds.Contains(p.MemberShipId));

                    data = memberships.Where(m => payments.Any(p => p.MemberShipId == m.MemberId));
                    break;
            }

            return data;
        }

    }
}
