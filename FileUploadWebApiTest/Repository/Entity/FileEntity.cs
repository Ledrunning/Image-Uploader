using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FileUploadWebApiTest.Repository.Entity
{
    public class FileEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset DateTime { get; set; }
        public string PhotoPath { get; set; }
    }
}