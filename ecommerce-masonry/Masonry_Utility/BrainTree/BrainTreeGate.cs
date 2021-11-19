using Braintree;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masonry_Utility.BrainTree
{
    public class BrainTreeGate : IBrainTreeGate
    {
        public BrainTreeSettings _options { get; set; }
        private IBraintreeGateway _braintreeGateway { get; set; }

        public BrainTreeGate(IOptions<BrainTreeSettings> options)
        {
            _options = options.Value;
        }

        public IBraintreeGateway CreateGateway()
        {
            return new BraintreeGateway(_options.Environment, _options.MerchantId, _options.PublicKey, _options.PrivateKey);
            
        }

        public IBraintreeGateway GetGateWay()
        {
            if (_braintreeGateway == null)
            {
                _braintreeGateway = CreateGateway();
                //_braintreeGateway = new BraintreeGateway(_options.Environment, _options.MerchantId, _options.PublicKey, _options.PrivateKey);
            }
            return _braintreeGateway;
        }
    }
}
