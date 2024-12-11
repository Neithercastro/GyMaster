using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using GYMaster_API.Services;
using Microsoft.Extensions.Logging;
using PayPal.Api;
using System.Configuration;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using GYMaster_API.DTO;
using GYMaster_API.Entities;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.CodeAnalysis;

namespace GYMaster_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaypalController : ControllerBase
    {
        private readonly GymasterContext _context;
        private readonly ILogger<PaypalController> logger;
        private IHttpContextAccessor httpContextAccessor;
        IConfiguration configuration;

        private Payment payment;

        public PaypalController(ILogger<PaypalController> Logger, IHttpContextAccessor HttpContextAccessor, IConfiguration Configuration, GymasterContext context)
        {
            logger = Logger;
            httpContextAccessor = HttpContextAccessor;
            configuration = Configuration;
            _context = context;
        }

        [HttpGet("PaymentWithPaypal")]
        public ActionResult PaymentWithPaypal(string? Cancel = null, string? guid = "", string? paymentId = "", string? token = "", string? PayerID = "")
        {
            var clientId = configuration.GetValue<string>("Paypal:ClientID");
            var clientSecret = configuration.GetValue<string>("Paypal:Secret");
            var mode = configuration.GetValue<string>("Paypal:Mode");

            APIContext apiContext = PaypalConfiguration.GetAPIContext(clientId, clientSecret);

            try
            {
                string payerId = PayerID;
                if (string.IsNullOrEmpty(payerId))
                {
                    string baseURI = this.Request.Scheme + "://" + this.Request.Host + "/api/Paypal/PaymentWithPaypal?";
                    var guidd = Convert.ToString((new Random()).Next(100000));
                    guid = guidd;

                    var CreatedPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid, "");
                    var links = CreatedPayment.links.GetEnumerator();
                    string PaypalRedirectURL = null;

                    while (links.MoveNext())
                    {
                        Links link = links.Current;
                        if (link.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            PaypalRedirectURL = link.href;
                        }
                    }

                    return Redirect(PaypalRedirectURL);
                }
                else
                {
                    string PaymentId = paymentId;
                    var executedPayment = ExecutePayment(apiContext, payerId, PaymentId);

                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return NotFound();
                    }
                    else
                    {
                        var Data = new Paypal()
                        {
                            Guid = guid!,
                            Transaccion = PaymentId,
                            Comprador = payerId,
                        };

                        _context.Paypals.Add(Data);
                        _context.SaveChangesAsync();

                        return Ok();
                    } 
                }
            }
            catch (Exception ex)
            {
                return NotFound("Hubo un error");
            }
        }

        private Payment CreatePayment(APIContext apiContext, string redirectUrl, string blogId)
        {
            /*var List = await _context.Carritosmaestros.Where(
                Request => Request.Id.Equals(idCarrito)).Select(
                    Data => new CarritosdetalleDTO
                    {
                        IdCarrito = Data.IdCarrito,
                        IdProducto = Data.IdProducto,
                        Cantidad = Data.Cantidad,
                        Subtotal = Data.Subtotal
                    }
                )
            );*/

            var itemList = new ItemList() { items = new List<Item>() };
            itemList.items.Add(new Item()
            {
                name = "Proteina",
                currency = "USD",
                price = "2.00",
                quantity = "1",
                sku = "asd"
            });

            var payer = new Payer() { payment_method = "paypal" };
            var redirectURLs = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };

            var amount = new Amount()
            {
                currency = "USD",
                total = "2"
            };

            var transactionList = new List<Transaction>();
            transactionList.Add(new Transaction()
            {
                description = "Transaccion default",
                invoice_number = Guid.NewGuid().ToString(),
                amount = amount,
                item_list = itemList
            });

            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirectURLs
            };

            return this.payment.Create(apiContext);
        }

        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };

            this.payment = new Payment()
            {
                id = paymentId
            };

            return this.payment.Execute(apiContext, paymentExecution);
        }
    }
}
