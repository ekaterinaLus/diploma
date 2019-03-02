using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectDiploma.ViewModel;
using System;
using System.IO;

namespace ProjectDiploma.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private const string FILE_FOLDER_PATH = @"\Content\";

        private readonly IHostingEnvironment _hostingEnvironment;

        public FileController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost("[action]")]
        public Response UploadFile([FromForm]IFormFile file)
        {
            var filePath = $"{_hostingEnvironment.ContentRootPath}{FILE_FOLDER_PATH}{Guid.NewGuid().ToString()}{Path.GetExtension(file.FileName)}";

            var response = new Response<string>(string.Empty);

            try
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                response.ItemResult = Path.GetFileName(filePath);
            }
            catch (Exception e)
            {
                response.AddMessage(MessageType.ERROR, "Не удалось загрузить файл!");
            }

            return response;
        }

        [HttpGet("[action]/{fileName}")]
        public IActionResult LoadFile([FromRoute] string fileName)
        {
            var fileExtension = Path.GetExtension(fileName).ToLower();
            var produces = string.Empty;

            switch (fileExtension)
            {
                case ".png":
                    produces = "png";
                    break;
                case ".jpg":
                    produces = "jpeg";
                    break;
            }

            return PhysicalFile($"{_hostingEnvironment.ContentRootPath}{FILE_FOLDER_PATH}{fileName}", $"image/{produces}");
        }
    }
}
