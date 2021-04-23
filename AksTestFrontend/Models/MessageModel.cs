using System;
using System.Collections.Generic;

namespace AksTestFrontend.Models
{
    public class MessageModel
    {
        public Guid Id { get; set; }

        public DateTime SendTime { get; set; }

        public string Content { get; set; }

        public List<string> IpRoutes { get; set; }
    }

    public class ResultModel
    {
        public bool Result { get; set; }
        public int MessageCount { get; set; }
    }
}