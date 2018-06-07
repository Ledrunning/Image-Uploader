using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileUploadWebApiTest.Models
{
    public class RecieveTransmitModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset Datetime { get; set; }
    }
}
