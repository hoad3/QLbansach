﻿using Microsoft.AspNetCore.Mvc;
using QLbansach_tutorial.Models;
using QLbansach_tutorial.Services;

namespace QLbansach_tutorial.Controllers;

public class PaymentController:Controller
{
    private readonly IVnPayService _vnPayService;
    public PaymentController(IVnPayService vnPayService)
    {
		
        _vnPayService = vnPayService;
    }

    public IActionResult CreatePaymentUrlVnpay(PaymentInformationModel model)
    {
        var url = _vnPayService.CreatePaymentUrl(model, HttpContext);

        return Redirect(url);
    }


}