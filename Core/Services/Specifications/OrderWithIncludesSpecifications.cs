using System.Linq.Expressions;

namespace Services.Specifications
{
    internal class OrderWithIncludesSpecifications : BaseSpecification<Order, Guid>
    {
        //Get order by id including related entities (order items, delivery method)
        public OrderWithIncludesSpecifications(Guid id) : base( o=>o.id == id)
        {
            AddInclude(o=>o.OrderItems);
            AddInclude(o=>o.DeliveryMethod);
            AddOrderBy(o=>o.OrderDate);
        }
        public OrderWithIncludesSpecifications(string userEmail) : base(o=>o.UserEmail == userEmail)
        {
            AddInclude(o => o.OrderItems);
            AddInclude(o => o.DeliveryMethod);
            AddOrderBy(o => o.OrderDate);
        }
    }
}
