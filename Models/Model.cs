using System;
using System.ComponentModel.DataAnnotations;

namespace BackendServiceStarter.Models
{
    public class Model
    {
        public int Id { get; set; }
        
        [Timestamp]
        public DateTime? CreatedAt { get; set; }
        
        [Timestamp]
        public DateTime? UpdatedAt { get; set; }
        
        [Timestamp]
        public DateTime? DeletedAt { get; set; }
    }
}