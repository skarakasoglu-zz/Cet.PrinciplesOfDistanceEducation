using System;
using System.IO;
using Cet.PrinciplesOfDistanceEducation.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Cet.PrinciplesOfDistanceEducation.Models;
using Microsoft.AspNetCore.Http;
using System.Threading;
using Xabe.FFmpeg;
using System.Threading.Tasks;
using System.Linq;
using Xabe.FFmpeg.Streams;
using Xabe.FFmpeg.Enums;
using Xabe.FFmpeg.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

namespace Cet.PrinciplesOfDistanceEducation.Controllers
{
    [Authorize(Roles = "Superuser,Administrator")]
    public class UploadController : Controller
    {
        private IConfiguration _configuration;
        private IVideo _videoService;

        private string _hostRootVideoPath;
        private string _htmlRootVideoPath;
        private string _tempVideoPath;

        public UploadController(IConfiguration configuration, IVideo videoService)
        {
            
            _configuration = configuration;
            _videoService = videoService;

            var folderPaths = _configuration.GetSection("FolderPaths");

            _hostRootVideoPath = Path.Combine(folderPaths.GetValue<string>("WebRootPath"), "videos/");
            _htmlRootVideoPath = folderPaths.GetValue<string>("HtmlVideoPath");
            _tempVideoPath = Path.Combine(_hostRootVideoPath, "temp/");

            if (!(Directory.Exists(_tempVideoPath))) Directory.CreateDirectory(_tempVideoPath);
        }

        public IActionResult Index()
        {
            UploadViewModel uploadModel = new UploadViewModel();
            uploadModel.Id = Guid.NewGuid().ToString();
            uploadModel.Year = DateTime.Now.Year;
            Bundle<UploadViewModel> model = new Bundle<UploadViewModel>();
            model.PageModel = uploadModel;
            ViewData["Title"] = "Upload a Video"; 
            return View(model);
        }        
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(314572800)]
        public IActionResult Upload(IFormFile file, string id)
        {
            string duration = string.Empty;
            Bundle<UploadViewModel> model = new Bundle<UploadViewModel>(new UploadViewModel { Id = id });
            if (file != null)
            {
                var tempVideoPath = CreateTempVideoPath(model.PageModel.Id);
                UploadFile(file, tempVideoPath);

                IMediaInfo mediaInfo = MediaInfo.Get(tempVideoPath).Result;

                var outputPath = CreateVideoPath(model.PageModel.Id);

                model.PageModel.ThumbnailOptions = new VideoThumbnail[3];

                CreateThumbnails(tempVideoPath, outputPath, Convert.ToInt32(mediaInfo.Duration.TotalSeconds), model.PageModel.ThumbnailOptions);
                ConvertAndSaveVideo(mediaInfo, outputPath, id);

                model.PageModel.VideoTitle = Path.GetFileNameWithoutExtension(file.FileName);
                model.PageModel.Duration = mediaInfo.Duration.ToString();

                string videoUrl = $"{_htmlRootVideoPath}/{id}/video{FileExtensions.Mp4}";
                string thumbnailUrl = $"{_htmlRootVideoPath}/{id}/thumbnail{FileExtensions.Png}";

                _videoService.CreateVideo(id, videoUrl, thumbnailUrl, mediaInfo.Duration,
                    Path.GetFileNameWithoutExtension(file.FileName), model.PageModel.Year, string.Empty, string.Empty, User.Identity.Name);
            }

            return new JsonResult(model);
        }

         [HttpPost]
        public IActionResult UploadDetails(Bundle<UploadViewModel> model, string selectedThumbnailId)
        {
            if (string.IsNullOrEmpty(selectedThumbnailId)) return new JsonResult(new { ThumbnailMissing = true });

            UploadViewModel video = model.PageModel;
            var videoPath = Path.Combine(_hostRootVideoPath, video.Id);
            var thumbnailUrl = Path.Combine(videoPath, $"thumbnail{FileExtensions.Png}");

            if (selectedThumbnailId == "custom")
            {
                string customUrl = Path.Combine(videoPath, "customthumb" + FileExtensions.Png);
                FileInfo custom = new FileInfo(customUrl);
                custom.MoveTo(thumbnailUrl);
            }
            else
            {
                string customUrl = Path.Combine(videoPath, "thumbnail_" + selectedThumbnailId + FileExtensions.Png);
                FileInfo thumb = new FileInfo(customUrl);
                thumb.MoveTo(thumbnailUrl);
            }

            foreach (var file in Directory.GetFiles(videoPath))
            {
                if (Path.GetFileNameWithoutExtension(file) != "thumbnail" && Path.GetExtension(file) == FileExtensions.Png)
                {
                    RemoveFile(file);
                }
            }

            _videoService.EditVideo(model.PageModel.Id, model.PageModel.VideoTitle, model.PageModel.Year, 
                                        model.PageModel.GroupName, model.PageModel.Description,
                User.Identity.Name, "Active");

            return new JsonResult("");
        }

        [HttpPost]
        public IActionResult UploadCustomThumbnail(IFormFile file, string id)
        {
            string htmlThumbPath = $"{id}/customthumb" + FileExtensions.Png;
            if (file != null)
            {
                string outputPath = Path.Combine(_hostRootVideoPath, htmlThumbPath);
                UploadFile(file, outputPath);
            }
            return new JsonResult(htmlThumbPath);
        }

        private string CreateTempVideoPath(string id)
        {
            var tempPath = Path.Combine(_tempVideoPath, id + FileExtensions.Mp4);
            return tempPath;
        }

        private void UploadFile(IFormFile file, string path)
        {
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
        }

        private string CreateVideoPath(string id)
        {
            var uploadingPath = Path.Combine(_hostRootVideoPath, id);
            if (!Directory.Exists(uploadingPath))
                Directory.CreateDirectory(uploadingPath);
            return uploadingPath;
        }

        private void CreateThumbnails(string tempVideoPath, string outputPath, int totalVideoSeconds, VideoThumbnail[] videoThumbnails)
        {
            Random rnd = new Random();

            for (int i = 0; i < videoThumbnails.Length; i++)
            {
                TimeSpan randomSpan = new TimeSpan(0, 0, 0, rnd.Next(totalVideoSeconds));
                var outThumbnailPath = Path.Combine(outputPath, "thumbnail_" + i + FileExtensions.Png);
                IConversionResult result = Conversion.Snapshot(tempVideoPath, outThumbnailPath, randomSpan).Start().Result;
                string id = Path.GetFileName(outputPath);
                videoThumbnails[i] = new VideoThumbnail { ThumbnailUrl = Path.Combine(_htmlRootVideoPath, id + "/thumbnail_" + i + FileExtensions.Png), Id = i };
            }
        }

        private async Task ConvertAndSaveVideo(IMediaInfo mediaInfo, string outputPath, string id)
        {
            var fileName = "video" + FileExtensions.Mp4;
            var outVideoPath = Path.Combine(outputPath, fileName);

            IStream videoStream = mediaInfo.VideoStreams.FirstOrDefault()
                ?.SetCodec(VideoCodec.H264);

            IStream audioStream = mediaInfo.AudioStreams.FirstOrDefault()
                ?.SetCodec(AudioCodec.Aac);
            var result = await Conversion.New().AddStream(audioStream, videoStream)
                .SetOutput(outVideoPath)
                .Start();

            if (result.Success)
            {
                Thread.Sleep(60000);
                string tempUrl = Path.Combine(_hostRootVideoPath, $"temp/{id}{FileExtensions.Mp4}");
                RemoveFile(tempUrl);
            }
        }

        private void RemoveFile(string path)
        {
            FileInfo file = new FileInfo(path);
            file.Delete();
        }
        
    }
}