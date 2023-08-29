using CustomerSupportChatApi.DataLayer.Models;
using System.ComponentModel.DataAnnotations;

namespace CustomerSupportChatApi.BusinessLayer.Dtos
{
    public class CreateChatSessionRequestDto
    {
        [Required]
        public string CustomerName { get; set; }
        [Required]
        public string CustomerEmail { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Message { get; set; }
        public DateTime TimeSent { get; set; }
    }
}
