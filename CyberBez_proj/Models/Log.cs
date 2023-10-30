using System.ComponentModel.DataAnnotations;

namespace CyberBez_proj.Models
{
    public class Log
    {
        [Key] 
        public int Id { get; set; }

        public string UserID { get; set; }

        public required string ActionExecutedBy { get; set; }

        public DateTime Timestamp { get; set; }

        public required string Action {  get; set; }

        public bool IsSuccessful { get; set; }
    }
}
