//using CefSharp;
//using System;
//using System.IO;

//namespace Marky
//{
//    public class LocalSchemeHandler : ISchemeHandler
//    {
//        public bool ProcessRequestAsync(IRequest request, ISchemeHandlerResponse response, OnRequestCompletedHandler requestCompletedCallback)
//        {
//            Uri u = new Uri(request.Url);
//            String file = u.Authority + u.AbsolutePath;

//            if (File.Exists(file))
//            {
//                Byte[] bytes = File.ReadAllBytes(file);
//                response.ResponseStream = new MemoryStream(bytes);
//                switch (Path.GetExtension(file))
//                {
//                    case ".html":
//                        response.MimeType = "text/html";
//                        break;
//                    case ".js":
//                        response.MimeType = "text/javascript";
//                        break;
//                    case ".png":
//                        response.MimeType = "image/png";
//                        break;
//                    case ".appcache":
//                    case ".manifest":
//                        response.MimeType = "text/cache-manifest";
//                        break;
//                    default:
//                        response.MimeType = "application/octet-stream";
//                        break;
//                }
//                requestCompletedCallback();
//                return true;
//            }
//            return false;
//        }
//    }
//}