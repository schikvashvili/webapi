using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace WebAPI.Controllers
{
    [EnableCors("http://localhost:4200", "*", "*", PreflightMaxAge = 100000, SupportsCredentials = true)]
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            var session = System.Web.HttpContext.Current.Session;

            if (session != null)
            {
                if (session["Values"] == null)
                {
                    session["Values"] = new string[] { "value1", "value2", "value3" };
                    return new string[] { "value1" };
                }
            }

            return new string[] { JsonConvert.SerializeObject((Object)session["Values"]) };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

       
        // POST api/values
        [HttpPost]
        public async Task<string> Post([FromBody]SearchUrl data)
        {
            //var content = new StringContent(JsonConvert.SerializeObject(data).ToString(),
            //Encoding.UTF8, "application/json");

            var session = System.Web.HttpContext.Current.Session;

            if (session != null)
            {
                if (session["Values"] == null)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Add("User-Agent", "your_user_agent");
                        using (HttpResponseMessage response = await client.GetAsync(data.URL))
                        using (HttpContent content = response.Content)
                        {
                            // ... Read the string.
                            session["Values"] = await content.ReadAsStringAsync();
                            return "Send request one more time to fetch data from session!";
                        }
                    }
                    
                }
            }

            return JsonConvert.SerializeObject((Object)session["Values"]);
        }


        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}


public class SearchUrl
{
    public string URL { get; set; }
}
