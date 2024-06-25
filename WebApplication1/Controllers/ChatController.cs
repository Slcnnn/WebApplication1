using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApplication1.Dtos;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly UserManager<AppUser> _userManager;

        public ChatController(AppDbContext appDbContext, UserManager<AppUser> userManager)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
        }

        [HttpGet("GetMessagesWithUser")]
        public IActionResult GetMessagesWithUser()
        {
            var values = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userId = _userManager.FindByNameAsync(values).Result.Id;

            var userMessages = _appDbContext.chats
                .Where(c => c.SenderId == userId || c.RecipientId == userId)
                .Select(c => new MessageDto
                {
                    SenderId = c.SenderId,
                    SenderUsername = c.Sender.UserName,
                    RecipientId = c.RecipientId,
                    RecipientUsername = c.Recipient.UserName,
                    Content = new string[] { c.Content }, // Content'i string dizisi olarak ata
                    CreatedAt = c.CreatedAt
                })
                .ToList();

            return Ok(userMessages);
        }

        [HttpGet("GetMessagesWithGroup")]
        public IActionResult GetMessagesWithGroup(int groupId)
        {
            var groupMessages = _appDbContext.GroupChats
                .Where(gc => gc.GroupId == groupId)
                .GroupBy(gc => gc.Group.Name) // Grup adına göre grupla
                .Select(group => new MessageGroup
                {
                    SenderId = group.First().SenderId,
                    SenderUsername = group.First().Sender.UserName,
                    RecipientId = group.First().GroupId,
                    GroupName = group.Key, // Grup adı (recipientUsername) olarak al
                    Contents = group.Select(gc => gc.Content).ToList(), // İçerikleri bir dizi olarak al
                    CreatedAt = group.First().CreatedAt // İlk mesajın oluşturma zamanını al
                })
                .ToList();

            return Ok(groupMessages);
        }

        [HttpPost("SendMessage")]
        public IActionResult SendMessage([FromBody] ChatDto chatDto)
        {
            var values = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userId = _userManager.FindByNameAsync(values).Result.Id;
            var chat = new Chat
            {
                SenderId = userId,
                RecipientId = chatDto.RecipientId,
                Content = chatDto.Content,
                CreatedAt = DateTime.Now
            };

            _appDbContext.chats.Add(chat);
            _appDbContext.SaveChanges();

            return Ok();
        }

        [HttpPost("SendGroupMessage")]
        public IActionResult SendGroupMessage([FromBody] GroupChatModel groupChatDto)
        {
            var values = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userId = _userManager.FindByNameAsync(values).Result.Id;

            var groupChat = new GroupChat
            {
                SenderId = userId,
                GroupId = groupChatDto.GroupId,
                Content = groupChatDto.Content,
                CreatedAt = DateTime.Now
            };

            _appDbContext.GroupChats.Add(groupChat);
            _appDbContext.SaveChanges();

            return Ok();
        }

        [HttpGet("GetUserGroups")]
        public IActionResult GetUserGroups()
        {
            var values = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userId = _userManager.FindByNameAsync(values).Result.Id;

            var userGroups = _appDbContext.GroupUsers.Where(gu => gu.UserId == userId)
                                                      .Select(gu => gu.Group)
                                                      .ToList();

            return Ok(userGroups);
        }

        [HttpPost("CreateGroup")]
        public IActionResult CreateGroup([FromBody] GroupModel groupDto)
        {
            var values = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userId = _userManager.FindByNameAsync(values).Result.Id;

            var group = new Group
            {
                Name = groupDto.Name
            };

            _appDbContext.Groups.Add(group);
            _appDbContext.SaveChanges();

            var groupUser = new GroupUsers
            {
                GroupId = group.Id,
                UserId = userId
            };

            _appDbContext.GroupUsers.Add(groupUser);
            _appDbContext.SaveChanges();

            return Ok();
        }

        [HttpPost("AddUserToGroup")]
        public IActionResult AddUserToGroup([FromBody] AddUserToGroupModel addUserToGroupDto)
        {
            var group = _appDbContext.Groups.FirstOrDefault(g => g.Id == addUserToGroupDto.GroupId);
            if (group == null)
            {
                return NotFound("Group not found");
            }

            var existingMembership = _appDbContext.GroupUsers.FirstOrDefault(gu => gu.GroupId == addUserToGroupDto.GroupId && gu.UserId == addUserToGroupDto.UserId);
            if (existingMembership != null)
            {
                return BadRequest("User is already a member of the group");
            }

            var membership = new GroupUsers
            {
                GroupId = addUserToGroupDto.GroupId,
                UserId = addUserToGroupDto.UserId
            };

            _appDbContext.GroupUsers.Add(membership);
            _appDbContext.SaveChanges();

            return Ok();
        }

    }
}
