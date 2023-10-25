using Booking.Areas.BackOffice.Data.Interface;
using Booking.Areas.BackOffice.Models.Input;
using Booking.Areas.BackOffice.Models.Output;
using Microsoft.AspNetCore.Mvc;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Booking.Areas.BackOffice.Controllers
{
    [Area("BackOffice")]
    public class RoomsController : Controller
    {
        public required IRoomsRepository _roomsRepository;
        private readonly IHostingEnvironment _hostingEnvironment;
        /// <summary>
        /// Constructor to initialize the object
        /// </summary>
        /// <param name="roomsRepository"></param>
        public RoomsController(IRoomsRepository roomsRepository, IHostingEnvironment hostingEnvironment)
        {
            _roomsRepository = roomsRepository;
            _hostingEnvironment = hostingEnvironment;
        }

        [Route("~/add-room-details")]
        public async Task<IActionResult> AddRoomDetails(string RoomId)
        {
            ViewBag.RoomId = RoomId;
			AddRoomDTO roomsDTOs = new AddRoomDTO();
			roomsDTOs = await _roomsRepository.GetAddRoomDetails();
			return View(roomsDTOs);
		}

        [Route("~/view-room-details")]
        public async Task<IActionResult> ViewRoomDetails()
        {

			List<RoomsDTO> roomsDTOs = new List<RoomsDTO>();

			roomsDTOs = await _roomsRepository.GetRooms();

			return View(roomsDTOs);
		}

        [Route("~/get-room-details-byId")]
        public async Task<IActionResult> GetRoomDetailsById(Int64 RoomId)
        {
            RoomsDetailsDTO roomsDTOs = new RoomsDetailsDTO();

            roomsDTOs = await _roomsRepository.GetRoomDetailsById(RoomId);

            return Ok(roomsDTOs);
        }

        [Route("~/save-room-details")]
        public async Task<IActionResult> SaveRoomDetails([FromForm] RoomsDetailsDTO roomsDetailsDTO)
        {

            if (roomsDetailsDTO.FileImages != null && roomsDetailsDTO.FileImages.Count > 0)
            {
                string UniqueFileName = string.Empty;
                List<IFormFile> file = roomsDetailsDTO.FileImages;

                string imagesPath = string.Concat(_hostingEnvironment.WebRootPath, "//Attachments//RoomImages");
                var FileNames=string.Empty;

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
                    FileNames+=uniqueFileName+ "$";
                }

                if (!string.IsNullOrEmpty(FileNames) && FileNames.Length > 0)
                {
                    FileNames = FileNames.Substring(0, FileNames.Length - 1);
                    roomsDetailsDTO.Images = !string.IsNullOrEmpty(roomsDetailsDTO.Images)? ("$"+ FileNames): FileNames;

                }
            }
            //var result = await _roomsRepository.SaveRoomDetails(roomsDetailsDTO);

            return Ok(await _roomsRepository.SaveRoomDetails(roomsDetailsDTO));
        }

        [Route("~/delete-room-details")]
        public async Task<IActionResult> DeleteRoomDetails([FromQuery] int RoomId)
        {
            //var result = await _roomsRepository.SaveRoomDetails(roomsDetailsDTO);

            return Ok(await _roomsRepository.DeleteRoomDetails(RoomId));
        }

    }
}
