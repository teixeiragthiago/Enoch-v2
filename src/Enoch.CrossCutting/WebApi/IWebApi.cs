using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Enoch.CrossCutting.WebApi
{
    public interface IWebApi
    {
        HttpResponseMessage Get(string server, string route, string parameters = null,
         string token = null);

        HttpResponseMessage Post(string server, string route, string token, string url = null,
            object obj = null);

        HttpResponseMessage Post(string server, string route, string token, object obj = null);

        HttpResponseMessage Post(string server, string route, object obj = null);

        HttpResponseMessage Put(string server, string route, string token, object obj);
        HttpResponseMessage Patch(string server, string route, string token, object obj);
    }
}
