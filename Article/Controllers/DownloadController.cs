using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Article.Controllers
{
    public class DownloadController : Controller
    {
		public FileResult Download()
		{
			var files = TempData["Files"] as IList<string>;

			var archive = Server.MapPath("~/archive.zip");
			var temp = Server.MapPath("~/temp");

			// clear any existing archive
			if (System.IO.File.Exists(archive))
			{
				System.IO.File.Delete(archive);
			}
			// empty the temp folder
			Directory.EnumerateFiles(temp).ToList().ForEach(f => System.IO.File.Delete(f));

			// copy the selected files to the temp folder
			foreach (var file in files)
			{
				System.IO.File.Copy(Server.MapPath(file), Path.Combine(temp, path2: Path.GetFileName(file) ?? throw new InvalidOperationException()));

			}
			// create a new archive
			ZipFile.CreateFromDirectory(temp, archive);

			return File(archive, "application/zip", "archive.zip");
		}
	}
}