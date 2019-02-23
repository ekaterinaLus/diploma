using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectDiploma.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectDiploma.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private const string FILE_FOLDER_PATH = "./Content/";

        [HttpPost("[action]")]
        public Response UploadFile([FromForm]IFormFile file)
        {
            var filePath = $"{FILE_FOLDER_PATH}{Guid.NewGuid().ToString()}{Path.GetExtension(file.FileName)}";

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

        [HttpPost("[action]")]
        public IActionResult LoadFile([FromBody] string fileName)
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

            return PhysicalFile($"{FILE_FOLDER_PATH}{fileName}", $"image/{produces}");
        }
    }
}
