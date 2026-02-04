using System.Linq.Expressions;

namespace Services.Specifications
{
    internal class OrderWithPaymentIntentIdSpecifications : BaseSpecification<Order, Guid>
    {
        public OrderWithPaymentIntentIdSpecifications(string paymentIntentId) : base(o=>o.PaymentIntentId == paymentIntentId)
        {
        }
    }
}
