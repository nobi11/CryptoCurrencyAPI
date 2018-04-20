using CryptoCurrencyApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using CryptoCurrencyApi.Classes;
using System;
using System.Net.Http;

namespace CryptoCurrencyApi.Controllers
{
    public class CryptoCurrencyApiController : Controller
    {
        // GET: CryptoCurrencyApi
        public ActionResult Index()
        {
            CryptoCurrency cryptoCurrency = FillCurrency();
            return View(cryptoCurrency);
        }

        [HttpPost]
        public ActionResult Index(CryptoCurrency cryptoCurrency, string currencies)
        {
            cryptoCurrency = FillCurrency();

            if (currencies != null)
            {
                HttpWebRequest apiRequest = WebRequest.Create("https://api.coinmarketcap.com/v1/ticker/" + currencies + "/") as HttpWebRequest;

                string apiResponse = "";
                using (HttpWebResponse response = apiRequest.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    apiResponse = reader.ReadToEnd();
                }

                //Use http://json2csharp.com to create the model
                //RootObject rootObject = JsonConvert.DeserializeObject<RootObject>(apiResponse);
                //Remove the square brackets at the beginning and end of apiResponse so it can de deserailized otherwise, it will fall over, thinking it is an array
                RootObject rootObject = JsonConvert.DeserializeObject<RootObject>(apiResponse.Substring(1, apiResponse.Length-2));

                StringBuilder sb = new StringBuilder();

                if (Request.Form["submit"] == "")
                {
                    sb.Append("<table><tr><th>Currency Description</th></tr>");
                    sb.Append("<tr><td>Name:</td><td>" + rootObject.name + "</td></tr>");
                    sb.Append("<tr><td>Symbol:</td><td>" + rootObject.symbol + "</td></tr>");
                    sb.Append("<tr><td>Rank:</td><td>" + rootObject.rank + "</td></tr>");
                    sb.Append("<tr><td>Price (USD):</td><td>" + rootObject.price_usd + "</td></tr>");
                    sb.Append("<tr><td>Price (BTC):</td><td>" + rootObject.price_btc + "</td></tr>");
                    sb.Append("<tr><td>24-HR Volume USD:</td><td>" + rootObject.__invalid_name__24h_volume_usd + "</td></tr>");
                    sb.Append("<tr><td>Market Cap (USD):</td><td>" + rootObject.market_cap_usd + " </td></tr>");
                    sb.Append("<tr><td>Available Supply:</td><td>" + rootObject.available_supply + "</td></tr>");
                    sb.Append("<tr><td>Total Supply:</td><td>" + rootObject.total_supply + "</td></tr>");
                    sb.Append("<tr><td>Maximum Supply:</td><td>" + rootObject.max_supply + "</td></tr>");
                    sb.Append("<tr><td>Percent Change (1HR):</td><td>" + rootObject.percent_change_1h + "</td></tr>");
                    sb.Append("<tr><td>Percent Change (24HR):</td><td>" + rootObject.percent_change_24h + "</td></tr>");
                    sb.Append("<tr><td>Percent Change (7D):</td><td>" + rootObject.percent_change_7d + "</td></tr>");
                    sb.Append("<tr><td>Last Updated:</td><td>" + rootObject.last_updated + "</td></tr>");
                    sb.Append("</table>");
                    cryptoCurrency.apiResponse = sb.ToString();
                    return View(cryptoCurrency);
                }
                else
                //Export
                {
                    sb.Append("Currency Description, Value\r\n");
                    sb.AppendFormat("{0},{1}", "Name:", rootObject.name + "\r\n");
                    sb.AppendFormat("{0},{1}", "Symbol:", rootObject.symbol + "\r\n");
                    sb.AppendFormat("{0},{1}", "Rank:", rootObject.rank + "\r\n");
                    sb.AppendFormat("{0},{1}", "Price (USD):", rootObject.price_usd + "\r\n");
                    sb.AppendFormat("{0},{1}", "Price (BTC):", rootObject.price_btc + "\r\n");
                    sb.AppendFormat("{0},{1}", "24-HR Volume USD:", rootObject.__invalid_name__24h_volume_usd + "\r\n");
                    sb.AppendFormat("{0},{1}", "Market Cap (USD):", rootObject.market_cap_usd + "\r\n");
                    sb.AppendFormat("{0},{1}", "Available Supply:", rootObject.available_supply + "\r\n");
                    sb.AppendFormat("{0},{1}", "Total Supply:", rootObject.total_supply + "\r\n");
                    sb.AppendFormat("{0},{1}", "Maximum Supply:", rootObject.max_supply + "\r\n");
                    sb.AppendFormat("{0},{1}", "Percent Change (1HR):", rootObject.percent_change_1h + "\r\n");
                    sb.AppendFormat("{0},{1}", "Percent Change (24HR):", rootObject.percent_change_24h + "\r\n");
                    sb.AppendFormat("{0},{1}", "Percent Change (7D):", rootObject.percent_change_7d + "\r\n");
                    sb.AppendFormat("{0},{1}", "Last Updated:", rootObject.last_updated + "\r\n");
                    return File(System.Text.Encoding.ASCII.GetBytes(sb.ToString()), "text/csv", "CryptoCurrency.csv");
                }
            }
            else
            {
                //Reset
                cryptoCurrency.apiResponse = "► Select Currency";
                return View(cryptoCurrency);
            }
        }

        public CryptoCurrency FillCurrency()
        {
            CryptoCurrency cryptoCurrency = new CryptoCurrency();
            cryptoCurrency.currencies = new Dictionary<string, string>();
            cryptoCurrency.currencies.Add("BTC", "bitcoin");
            cryptoCurrency.currencies.Add("ETH", "ethereum");
            cryptoCurrency.currencies.Add("XRP", "ripple");
            cryptoCurrency.currencies.Add("BCH", "bitcoin-cash");
            cryptoCurrency.currencies.Add("ADA", "cardano");
            cryptoCurrency.currencies.Add("XLM", "stellar");
            cryptoCurrency.currencies.Add("LTC", "litecoin");
            cryptoCurrency.currencies.Add("EOS", "eos");
            cryptoCurrency.currencies.Add("NEM", "nem");
            cryptoCurrency.currencies.Add("IOTA", "iota");
            return cryptoCurrency;
        }

     }
}