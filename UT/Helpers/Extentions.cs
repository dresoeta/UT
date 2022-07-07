using System;
using System.IO;
using System.Net;
using AutoMapper;
using UT.Models;

namespace UT.Helpers
{
    public static class Extentions
    {
        public static Result CheckLocation(string location)
        {
            try
            {
                var result = string.Empty;
                var GetTimezoneList = (HttpWebRequest) WebRequest.Create("http://worldtimeapi.org/api/timezone");
                GetTimezoneList.ContentType = "application/json";
                GetTimezoneList.Method = "GET";

                var TimezoneList = (HttpWebResponse) GetTimezoneList.GetResponse();
                using (var streamReader = new StreamReader(TimezoneList.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();

                }


                var timezone = result;

                if (timezone.Contains(location))
                    return new Result {success = true, message = "Location Validated"};

                return new Result {success = false, message = "Location Not Found"};
            }
            catch (Exception ex)
            {
                return new Result {success = false, message = "Something Went Wrong"};
            }
           
        }





    }
}
