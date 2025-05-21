using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Diplom_Cheremnykh.Data;
using Diplom_Cheremnykh.Models;

namespace Diplom_Cheremnykh.Helpers
{
    public static class Logger
    {
        public static void LogAction(int? userId, string userEmail, string action)
        {
            using (var context = new AppDbContext())
            {
                var log = new Log
                {
                    UserId = userId,
                    UserEmail = userEmail,
                    Action = action,
                    CreatedAt = DateTime.Now
                };

                context.Logs.Add(log);
                context.SaveChanges();
            }
        }
    }
}
