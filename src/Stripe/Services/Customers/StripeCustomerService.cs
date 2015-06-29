using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Stripe
{
    public class StripeCustomerService : StripeService
    {
        public StripeCustomerService(string apiKey = null) : base(apiKey) { }

        public bool ExpandDefaultCard { get; set; }

        public virtual StripeCustomer Create(StripeCustomerCreateOptions createOptions)
        {
            var url = this.ApplyAllParameters(createOptions, Urls.Customers, false);

            var response = Requestor.PostString(url, ApiKey);

            return Mapper<StripeCustomer>.MapFromJson(response);
        }

        public virtual StripeCustomer Get(string customerId)
        {
            var url = string.Format("{0}/{1}", Urls.Customers, customerId);
            url = this.ApplyAllParameters(null, url, false);

            var response = Requestor.GetString(url, ApiKey);

            return Mapper<StripeCustomer>.MapFromJson(response);
        }

        public virtual StripeCustomer Update(string customerId, StripeCustomerUpdateOptions updateOptions)
        {
            var url = string.Format("{0}/{1}", Urls.Customers, customerId);
            url = this.ApplyAllParameters(updateOptions, url, false);

            var response = Requestor.PostString(url, ApiKey);

            return Mapper<StripeCustomer>.MapFromJson(response);
        }

        public virtual void Delete(string customerId)
        {
            var url = string.Format("{0}/{1}", Urls.Customers, customerId);

            Requestor.Delete(url, ApiKey);
        }

        public virtual IEnumerable<StripeCustomer> List(StripeCustomerListOptions listOptions = null)
        {
            var url = Urls.Customers;
            url = this.ApplyAllParameters(listOptions, url, true);

            var response = Requestor.GetString(url, ApiKey);

            return Mapper<StripeCustomer>.MapCollectionFromJson(response);
        }

        public virtual IEnumerable<StripeCustomer> ListAll(StripeCustomerListOptions listOptions = null)
        {
            string starting_after = null;
            listOptions.EndingBefore = null;
            bool next = true;
            var totalList = new List<StripeCustomer>();
            while (next)
            {
                listOptions.StartingAfter = starting_after;
                var url = Urls.Customers;
                url = this.ApplyAllParameters(listOptions, url, true);
                var response = Requestor.GetString(url, ApiKey);
                var list = Mapper<StripeCustomer>.MapCollectionFromJson(response);
                totalList.AddRange(list);

                dynamic obj = JsonConvert.DeserializeObject(response);
                if (obj.has_more != null && Boolean.Parse(obj.has_more.ToString()))
                {
                    var last = list.LastOrDefault();
                    starting_after = last.Id;
                }
                else
                {
                    next = false;
                }
            }

            return totalList;
        }
    }
}
