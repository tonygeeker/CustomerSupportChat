using CustomerSupportChatApi.BusinessLayer.Dtos;
using CustomerSupportChatApi.BusinessLayer.Interfaces;
using CustomerSupportChatApi.DataLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace CustomerSupportChatApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatSessionsController : ControllerBase
    {

        private IChatSessionsService _chatSessionService;

        public ChatSessionsController(IChatSessionsService chatSessionService)
        {
            _chatSessionService = chatSessionService;
        }

        [HttpPost(Name = "CreateChatSession")]
        public async Task<IActionResult> Post([FromBody]CreateChatSessionRequestDto requestDto)
        {
            try
            {
                if (ModelState.IsValid == false)
                    return BadRequest(ModelState);

                var result = await _chatSessionService.CreateChatSession(requestDto);

                if (result == false)
                    return BadRequest("Could not initiate chat at this time, please try again later!");

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}