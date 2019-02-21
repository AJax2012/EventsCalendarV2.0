using System.Collections.Generic;
using System.Web;
using EventsCalendar.Core.Models;
using EventsCalendar.Core.ViewModels;

namespace EventsCalendar.Core.Contracts
{
    public interface IPerformerService
    {
        IEnumerable<PerformerViewModel> ListPerformers();
        PerformerViewModel NewPerformerViewModel(/*HttpPostedFileBase image*/);
        void CreatePerformer(PerformerViewModel performerViewModel);
        PerformerViewModel ReturnPerformerViewModel(int id);
        void EditPerformer(PerformerViewModel performerViewModel, int id);
        void DeletePerformer(int id);
    }
}