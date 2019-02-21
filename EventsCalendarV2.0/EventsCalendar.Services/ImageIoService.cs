using EventsCalendar.Core.ViewModels;

namespace EventsCalendar.Services
{
    public class ImageIoService
    {
        public bool SaveImageService(PerformerViewModel viewModel)
        {
            try
            {
                if (viewModel.Performer.ImageUrl.Length > 0)
                {
                    viewModel.Performer.ImageUrl = "EventsCalendar.WebUI/Content/images/performers";
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
