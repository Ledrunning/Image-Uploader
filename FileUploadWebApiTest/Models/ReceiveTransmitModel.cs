using System;

namespace FileUploadWebApiTest.Models
{
    public class ReceiveTransmitModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset Datetime { get; set; }
    }
}