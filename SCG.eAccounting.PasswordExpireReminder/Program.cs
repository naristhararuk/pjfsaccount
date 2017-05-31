using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.eAccounting.PasswordExpireReminder.DAL;
using SS.SU.DTO;

namespace SCG.eAccounting.PasswordExpireReminder
{
    class Program
    {

       public  static void Main(string[] args)
        {
            Console.WriteLine("Send reminde expire account.");
            Factory.CreateObject();
             IList<SuUser> userList = Factory.SuUserQuery.FindReminderExpireAccount(DateTime.Now.Date);

            foreach (SuUser item in userList)
            {
                try
                {
                    //if expire then set active to false.
                    SuUser user = Factory.SuUserService.FindByIdentity(item.Userid);
                    if (user.PasswordExpiryDate == DateTime.Now.Date)
                    {
                        user.FailTime = user.SetFailTime;
                        Factory.SuUserService.Update(user);
                    }
                    Factory.SCGEmailService.SendEmailEM07(item.Userid);
                }
                catch (Exception)
                {
                    continue;
                }
            }
            Console.WriteLine("finish");

        }
    }
}
