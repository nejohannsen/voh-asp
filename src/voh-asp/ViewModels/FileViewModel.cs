using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using voh_asp.Models;

namespace voh_asp.Models
{
    public class FileReplaceViewModel
    {
        public AppFile dbFile { get; set; }
        public IFormFile localFile { get; set; }
    }
}
