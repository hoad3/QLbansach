using QLbansach_tutorial.Models;

namespace QLbansach_tutorial.Services;

public interface IVnPayService
{
    string CreatePaymentUrl(PaymentInformationModel model, HttpContext context);
    PaymentResponseModel PaymentExecute(IQueryCollection collections);

}