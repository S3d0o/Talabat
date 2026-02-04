using Domain.Contracts.StoreDb;
using Domain.Entities.BasketModule;
using Domain.Entities.OrderModule;
using Domain.Exceptions;
using Microsoft.Extensions.Configuration;
using Shared.Dtos.BasketModule;
using Stripe;
using Product = Domain.Entities.ProductModule.Product;

namespace Services.Implementations
{
    public class PaymentService(
        IBasketRepository _basketRepo, IUnitOfWork _unitOfWork, IMapper _mapper, IConfiguration _configuration) : IPaymentService
    {
        public async Task<BasketDto> CreateOrUpdatePaymentIntentAsync(string basketId)
        {
            var basket = await GetBasketAsync(basketId);

            await ValidateBasketAsync(basket); // validate prices and shipping

            var totalAmount = CalculateTotalAmount(basket);

            await CreationOrUpdatePaymentIntentAsync(basket, totalAmount);

            await _basketRepo.CreateOrUpdateBasketAsync(basket);

            return _mapper.Map<BasketDto>(basket);
        }

        public async Task UpdatePaymentStatusAsync(string json, string signatureHeader)
        {
            var endpointSecret = _configuration["StripeSettings:EndPointSecret"];

            Event stripeEvent;

            try
            {
                stripeEvent = EventUtility.ConstructEvent(
                    json,
                    signatureHeader,
                    endpointSecret
                );
            }
            catch (StripeException)
            {
                // Invalid signature
                return;
            }

            if (stripeEvent.Data.Object is not PaymentIntent paymentIntent)
                return;

            switch (stripeEvent.Type)
            {
                case EventTypes.PaymentIntentSucceeded:
                    await UpdatePaymentStatusRecievedAsync(paymentIntent.Id);
                    break;

                case EventTypes.PaymentIntentPaymentFailed:
                    await UpdatePaymentStatusFaildAsync(paymentIntent.Id);
                    break;

                default:
                    break;
            }
        }


        private async Task UpdatePaymentStatusFaildAsync(string paymentIntentId)
        {
            var order = await _unitOfWork
                .GenericRepository<Order, Guid>()
                .GetByIdAsync(
                    new OrderWithPaymentIntentIdSpecifications(paymentIntentId)
                );

            if (order is null) return;
            if (order.PaymentStatus == OrderPaymentStatus.PaymentFailed) return;

            order.PaymentStatus = OrderPaymentStatus.PaymentFailed;
            _unitOfWork.GenericRepository<Order, Guid>().Update(order);
            await _unitOfWork.SaveChangesAsync();
        }

        private async Task UpdatePaymentStatusRecievedAsync(string paymentIntentId)
        {
            var order = await _unitOfWork
                .GenericRepository<Order, Guid>()
                .GetByIdAsync(
                    new OrderWithPaymentIntentIdSpecifications(paymentIntentId)
                );

            if (order is null) return;
            if (order.PaymentStatus == OrderPaymentStatus.PaymentRecieved) return;

            order.PaymentStatus = OrderPaymentStatus.PaymentRecieved;
            _unitOfWork.GenericRepository<Order, Guid>().Update(order);
            await _unitOfWork.SaveChangesAsync();
        }


        #region Helpers 
        private async Task CreationOrUpdatePaymentIntentAsync(CustomerBasket basket, long totalAmount)
        {
            var stripeService = new Stripe.PaymentIntentService();
            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var options = new Stripe.PaymentIntentCreateOptions
                {
                    Amount = totalAmount,
                    Currency = "usd",
                    AutomaticPaymentMethods = new Stripe.PaymentIntentAutomaticPaymentMethodsOptions
                    {
                        Enabled = true,
                    },
                    Metadata = new Dictionary<string, string>
                    {
                        { "BasketId", basket.Id },
                        { "DeliveryMethodId", basket.DeliveryMethodId!.Value.ToString() }
                    }
                };
                // request options with idempotency key to avoid duplicate payment intents for the same basket
                // idempotency key : AVOIDS => “The SAME request was sent twice (network retry, timeout, double-click, race condition)
                var requestOptions = new RequestOptions
                {
                    IdempotencyKey = $"basket_{basket.Id}_create"
                };
                var paymentIntent = await stripeService.CreateAsync(options, requestOptions);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else
            {
                // products prices or shipping price has been changed ==> update payment intent
                // user changed basket(+- quantity) ==> update payment intent amount
                var options = new Stripe.PaymentIntentUpdateOptions
                {
                    Amount = totalAmount // new amount
                };
                var requestOptions = new RequestOptions
                {
                    IdempotencyKey = $"basket_{basket.Id}_update"
                };

                await stripeService.UpdateAsync(
                    basket.PaymentIntentId,
                    options,
                    requestOptions // to avoid duplicate requests
                );
            }
        }

        private long CalculateTotalAmount(CustomerBasket basket)
        {
            return (long)(basket.Items.Sum(item => item.Price * item.Quantity) + basket.ShippingPrice)! * 100;
        }

        private async Task ValidateBasketAsync(CustomerBasket basket)
        {
            var productIds = basket.Items.Select(i => i.Id).ToList();

            var products = await _unitOfWork
                .GenericRepository<Product, int>()
                .FindAsync(p => productIds.Contains(p.id), asNoTracking: true);
            if (products.Count() != basket.Items.Count())
            {
                throw new NotFoundException("One or more products no longer exist");
            }

            foreach (var item in basket.Items)
            {
                var product = products.First(p => p.id == item.Id);
                item.Price = product.Price;
            }

            if (!basket.DeliveryMethodId.HasValue) throw new Exception("Delivery method is not selected");
            var deliveryMethod = await _unitOfWork.GenericRepository<DeliveryMethod, int>()
                .GetByIdAsync(basket.DeliveryMethodId.Value)
                ?? throw new DeliveryMethodNotFoundException(basket.DeliveryMethodId.Value);
            basket.ShippingPrice = deliveryMethod.Price;
        }

        private async Task<CustomerBasket> GetBasketAsync(string basketId)
        {
            return await _basketRepo.GetBasketAsync(basketId)
               ?? throw new BasketNotFoundException(basketId);
        }

        #endregion
    }
}
