using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uploader.Model
{
    public class RecieveTransmitModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset Datetime { get; set; }
    }
}
