using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Orchard.Autoroute.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Core.Common.Models;
using Orchard.MediaLibrary;
using Orchard.MediaLibrary.Models;
using Orchard.MediaLibrary.Services;

namespace ceenq.com.TinyMce.Handlers
{
    public class BodyPartHandler : ContentHandler
    {
        private readonly IMediaLibraryService _mediaLibraryService;

        public BodyPartHandler(IMediaLibraryService mediaLibraryService)
        {
            _mediaLibraryService = mediaLibraryService;


            //OnCreating<BodyPart>(
            //    (context, part) =>
            //    {
            //        var newBody = Regex.Replace(part.Text, "\"data:image/png;base64,(.*?)\"", delegate(Match match)
            //        {
            //            var matchString = match.ToString().Replace("data:image/png;base64,", "").Replace("\"", "");

            //            var buffer = Convert.FromBase64String(matchString);

            //            var imageFileName = Guid.NewGuid().ToString() + ".jpg";
            //            var imagePublicUrl = "";
            //            using (var stream = new MemoryStream(buffer))
            //            {
            //                var mediaPart = _mediaLibraryService.ImportMedia(stream, "Sermons", imageFileName);
            //                imagePublicUrl = _mediaLibraryService.GetMediaPublicUrl(mediaPart.FolderPath, mediaPart.FileName);
            //            }
            //            return "\"" + imagePublicUrl + "\"";
            //        });

            //        part.Text = newBody;

            //    });

            OnLoaded<BodyPart>(
                (context, part) =>
                {
                    if (string.IsNullOrWhiteSpace(part.Text) || !part.Text.Contains("data:image")) return;
                    var newBody = Regex.Replace(part.Text, "\"data:image/png;base64,(.*?)\"", delegate(Match match)
                    {
                        var matchString = match.ToString().Replace("data:image/png;base64,", "").Replace("\"", "");

                        var buffer = Convert.FromBase64String(matchString);

                        var imageFileName = Guid.NewGuid().ToString() + ".jpg";
                        string imagePublicUrl;
                        using (var stream = new MemoryStream(buffer))
                        {
                            var mediaDirectory = context.ContentItem.ContentType;
                            var autoroute = context.ContentItem.As<AutoroutePart>();
                            if (autoroute != null && !String.IsNullOrEmpty(autoroute.DisplayAlias))
                            {
                                mediaDirectory += "\\" + autoroute.DisplayAlias;
                            }

                            var mediaPart = _mediaLibraryService.ImportMedia(stream, mediaDirectory, imageFileName);
                            imagePublicUrl = _mediaLibraryService.GetMediaPublicUrl(mediaPart.FolderPath, mediaPart.FileName);
                        }
                        return "\"" + imagePublicUrl + "\"";
                    });

                    part.Text = newBody;

                });
        }
    }
}
