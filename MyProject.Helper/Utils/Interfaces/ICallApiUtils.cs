using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Helper.Utils.Interfaces
{
    public interface ICallApiUtils
    {
        HttpRequestMessage CallApi(string url, string apiKey, string requestBody);
    }
}
