using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WpfApp1.Model
{
    public class GenerateEWayBill
    {
        private readonly HttpClient _httpClient = new HttpClient();

        private readonly string _clientId = "YOUR_CLIENT_ID";
        private readonly string _clientSecret = "YOUR_CLIENT_SECRET";
        private readonly string _gstin = "29AAJFM0207E1ZM";
        private readonly string _userId = "ashabl505";
        private readonly string _password = "Ashabl@180368";

        private string _authToken;

        /// <summary>
        /// Step 1: Authenticate with NIC and store token
        /// </summary>
        public async Task AuthenticateAsync()
        {
            var payload = new
            {
                action = "ACCESSTOKEN",
                username = _userId,
                password = _password,
                gstin = _gstin,
                client_id = _clientId,
                client_secret = _clientSecret
            };

            var json = JsonConvert.SerializeObject(payload);
            var response = await _httpClient.PostAsync(
                "https://api.ewaybillgst.gov.in/ewaybillapi/authenticate",
                new StringContent(json, Encoding.UTF8, "application/json")
            );

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Auth failed: {await response.Content.ReadAsStringAsync()}");
            }

            var result = JsonConvert.DeserializeObject<dynamic>(
                await response.Content.ReadAsStringAsync()
            );

            _authToken = result.token.ToString(); // Extract token
        }

        /// <summary>
        /// Step 2: Generate an E-Way Bill
        /// </summary>
        public async Task<string> GenerateEWayBillAsync(string invoiceJson)
        {
            if (string.IsNullOrEmpty(_authToken))
                throw new Exception("You must call AuthenticateAsync() first");

            var request = new HttpRequestMessage(
                HttpMethod.Post,
                "https://api.ewaybillgst.gov.in/ewaybillapi/v1.03/genewaybill"
            );

            request.Headers.Add("Authorization", $"Bearer {_authToken}");
            request.Headers.Add("gstin", _gstin);

            request.Content = new StringContent(invoiceJson, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"EWB generation failed: {await response.Content.ReadAsStringAsync()}");
            }

            var result = await response.Content.ReadAsStringAsync();
            return result; // Contains eWayBillNo, date, validity, etc.
        }
    }
}



