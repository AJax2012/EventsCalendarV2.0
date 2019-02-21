using System.Web;

namespace EventsCalendar.Core.Contracts
{
    public interface IImageIoService
    {
        bool SaveImageService(HttpPostedFileBase image);
    }
}
