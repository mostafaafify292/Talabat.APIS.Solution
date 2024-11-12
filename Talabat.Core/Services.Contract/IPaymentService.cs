using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Services.Contract
{
    public interface IPaymentService
    {
        //function to Create or Update Payment Intent
        public Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string basketId);
    }
}
