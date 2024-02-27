using Booking.Areas.BackOffice.Data.Interface;
using Booking.Areas.BackOffice.Models.Input;
using Booking.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Booking.Areas.BackOffice.Controllers
{
    [Area("BackOffice")]
   // [Authorize(Policy = Policies.RequireAdminClaim)]
    public class EventController : Controller
    {
        public required IEventRepository _eventRepository;
        private readonly IHostingEnvironment _hostingEnvironment;
        /// <summary>
        /// Constructor to initialize the object
        /// </summary>
        /// <param name="roomsRepository"></param>
        public EventController(IEventRepository eventRepository, IHostingEnvironment hostingEnvironment)
        {
            _eventRepository = eventRepository;
            _hostingEnvironment = hostingEnvironment;
        }

        [Route("~/add-event-details")]
        public  IActionResult AddEventDetails(string EventId)
        {
            ViewBag.EventId = EventId;
            return View();
        }

        [Route("~/view-event-details")]
        public async Task<IActionResult> ViewEventDetails()
        {
            List<Event> events = new List<Event>();

            events = await _eventRepository.GetEvent();

            return View(events);
        }

        [Route("~/get-event-details-byId")]
        public async Task<IActionResult> GetEventDetailsById(Int64 EventId)
        {
            Event events = new Event();

            events = await _eventRepository.GetEventDetailsById(EventId);

            return Ok(events);
        }

        [Route("~/save-event-details")]
        public async Task<IActionResult> SaveEventDetails([FromForm] Event events)
        {

            if (events.FileImages != null && events.FileImages.Count > 0)
            {
                string UniqueFileName = string.Empty;
                List<IFormFile> file = events.FileImages;

                string imagesPath = string.Concat(_hostingEnvironment.WebRootPath, "//Attachments//EventImages");
                var FileNames = string.Empty;

                if (!Directory.Exists(imagesPath))
                {
                    Directory.CreateDirectory(imagesPath);
                }

                foreach (var uploadedFile in file)
                {
                    // Get the filename and extension of the uploaded file
                    string fileName = Path.GetFileNameWithoutExtension(uploadedFile.FileName);
                    string fileExtension = Path.GetExtension(uploadedFile.FileName);

                    // Generate a unique filename
                    string uniqueFileName = $"{fileName}_{DateTime.Now.ToString("MM_dd_yyyy_hh_mm_ss")}{fileExtension}";

                    // Create a file stream to save the uploaded file to disk
                    using (var fileStream = new FileStream(Path.Combine(imagesPath, uniqueFileName), FileMode.Create))
                    {
                        // Copy the uploaded file to the file stream
                        uploadedFile.CopyTo(fileStream);
                    }
                    FileNames += uniqueFileName + "$";
                }

                if (!string.IsNullOrEmpty(FileNames) && FileNames.Length > 0)
                {
                    FileNames = FileNames.Substring(0, FileNames.Length - 1);
                    events.Images = !string.IsNullOrEmpty(events.Images) ? ("$" + FileNames) : FileNames;

                }
            }
            //var result = await _roomsRepository.SaveRoomDetails(roomsDetailsDTO);

            return Ok(await _eventRepository.SaveEventDetails(events));
        }

        [Route("~/delete-event-details")]
        public async Task<IActionResult> DeleteEventDetails([FromQuery] Int64 EventId)
        {
            return Ok(await _eventRepository.DeleteEventDetails(EventId));
        }
    }
}
