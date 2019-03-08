using EventsCalendar.DataAccess.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsCalendar.Services.Helpers
{
    public class ConfirmationNumberUtil
    {
        private readonly MsSqlTicketRepository _repository;
        public ConfirmationNumberUtil(MsSqlTicketRepository repository)
        {
            _repository = repository;
        }

        private static Random random = new Random();
        public string RandomString(int length)
        {
            const string firstchars = "ABCDEFGH";
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            StringBuilder confirmationNumber = new StringBuilder();
            confirmationNumber.Append(firstchars[random.Next(firstchars.Length)]);

            var remainder = new string(Enumerable.Repeat(chars, length - 1)
              .Select(s => s[random.Next(s.Length)]).ToArray());

            confirmationNumber.Append(remainder);
            var stringConfNumb = confirmationNumber.ToString();

            var boolResult = _repository.Collection()
                .Where(t => t.ConfirmationNumber == stringConfNumb)
                .Any();

            if (boolResult)
                RandomString(length);

            return stringConfNumb;
        }
    }
}
