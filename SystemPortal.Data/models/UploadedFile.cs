using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemPortal.Data.models
{
    public class UploadedFile
    {
        public required IFormFile File { get; set; }
    }
}
