namespace PaymentGateway.Api.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public static class ControllerExtensions
    {
        public static Guid GetAuthenticatedMerchantId(this ControllerBase controller)
        {
            return new Guid(controller.User.Identity.Name);
        }
    }
}
