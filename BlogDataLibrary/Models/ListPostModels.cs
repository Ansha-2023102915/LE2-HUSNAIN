using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogDataLibrary.Models
{
    public class ListPostModels
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string AuthorName { get; set; } = "";
        public DateTime CreatedAt { get; set; }
    }
}
