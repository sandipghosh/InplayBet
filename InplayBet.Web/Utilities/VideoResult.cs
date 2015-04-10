

namespace InplayBet.Web.Utilities
{
    using System.IO;
    using System.Web.Hosting;
    using System.Web.Mvc;

    public class VideoResult : ActionResult
    {
        private readonly string _videoMimeType;

        public VideoResult(string videoMimeType)
        {
            this._videoMimeType = videoMimeType;
        }

        private string GetFileName(string videoMimeType)
        {
            switch (videoMimeType)
            { 
                case "video/webm":
                    return "Inplay20-HowTo_VP8.mp4";
                case "video/ogg":
                    return "Inplay20-HowTo_libtheora.mp4";
                default:
                    return "Inplay20-HowTo_x264.mp4";
            }
        }

        public override void ExecuteResult(ControllerContext context)
        {
            string fileName = this.GetFileName(this._videoMimeType);
            var videoFilePath = HostingEnvironment.MapPath(string.Format("~/Media/{0}", fileName));
            context.HttpContext.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", fileName));
            var file = new FileInfo(videoFilePath);
            if (file.Exists)
            {
                var stream = file.OpenRead();
                var bytesinfile = new byte[stream.Length];
                stream.Read(bytesinfile, 0, (int)file.Length);
                context.HttpContext.Response.BinaryWrite(bytesinfile);
            }
        }
    }
}