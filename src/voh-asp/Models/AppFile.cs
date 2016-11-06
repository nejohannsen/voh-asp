using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace voh_asp.Models
{
    public class AppFile
    {
        public Guid AppFileId { get; set; }
        public String Name { get; set; }
        public String FileName { get; set; }
        public String AWSBucket { get; set; }
        public String Path { get; set; }
        public String Type { get; set; }
    }
}
