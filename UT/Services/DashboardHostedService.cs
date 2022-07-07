using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DigitalEnvision.Data;
using DigitalEnvision.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using UT.Dto;
using UT.Hubs;

namespace UT.Services
{
    
    public class DashboardHostedService : IHostedService
    {
        private Timer _timer;
        private readonly IHubContext<NotificationHub> _hubContext;

        public DashboardHostedService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
            
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(60));

            return Task.CompletedTask;
        }

        public static DateTime NowUserLocalTime(string Location)
        {
            try
            {
                var result = string.Empty;
                var getOffset = (HttpWebRequest) WebRequest.Create("https://worldtimeapi.org/api/timezone/" + Location);
                getOffset.ContentType = "application/json";
                getOffset.Method = "GET";

                var Offset = (HttpWebResponse) getOffset.GetResponse();
                using (var streamReader = new StreamReader(Offset.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();

                }

                var response = JsonConvert.DeserializeObject<Offset>(result);

                return response.datetime;

                

          
            }
            catch (Exception ex)
            {
                return DateTime.Now;
            }

        }

        private async void DoWork(object state)
        {
            
            var BirthdayUser = await GetUser();

            foreach (var User in BirthdayUser)
            {
                var location = User.Location;
                DateTime Userlocaltime = NowUserLocalTime(location);

                var scheduledTime = new DateTime(Userlocaltime.Year, Userlocaltime.Month, Userlocaltime.Day, 09, 00, 00, 000);
                var endDate = new DateTime(Userlocaltime.Year, Userlocaltime.Month, Userlocaltime.Day, 23, 59, 59, 999);

                if (Userlocaltime >= scheduledTime && Userlocaltime <= endDate && User.Year != Userlocaltime.Year)
                {
                    try
                    {
                        var httpWebRequest =
                            (HttpWebRequest) WebRequest.Create("https://hookb.in/lJzKXkJYYmFaLRX7nEww");
                        var result = string.Empty;
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "POST";

                        string template = "Hey, {full_name} it’s your birthday";
                        template = template.Replace("{full_name}", User.Firstname + ' ' + User.Lastname);

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            string json = template;

                            streamWriter.Write(json);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }

                        var httpResponse = (HttpWebResponse) httpWebRequest.GetResponse();

                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            result = streamReader.ReadToEnd();
                        }

                        if (result.Contains("success"))
                        {
                            User.Year = Userlocaltime.Year;

                            var context = new UTContext();

                            context.User.Update(User);

                            await context.SaveChangesAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }

                }

            }

        }


        public async Task<List<User>> GetUser()
        {
            #region find user by birthday and birth month
            var context = new UTContext();
                DateTime nowLocal = DateTime.Now;
                DateTime nowUTC = nowLocal.ToUniversalTime().AddHours(-12);
                return await context.User.Where(x => x.DateOfBirth.Month == nowUTC.Month && x.DateOfBirth.Day >= nowUTC.Day && x.DateOfBirth.Day <= nowUTC.AddDays(1).Day)
                    .ToListAsync();
            #endregion region find user by birth month
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }
    }
}
