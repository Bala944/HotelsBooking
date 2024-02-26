using Booking.Areas.BackOffice.Models.Input;
using Booking.Areas.BackOffice.Models.Output;

namespace Booking.Areas.BackOffice.Data.Interface
{
    public interface IEventRepository
    {
         Task<List<Event>> GetEvent();
         Task<Event> GetEventDetailsById(Int64 EventId);
         Task<int> SaveEventDetails(Event events);
         Task<int> DeleteEventDetails(Int64 EventId);
    }
}
