using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KanbanBoard.WebApi.Models;
using KanbanBoard.WebApi.Repositories;
using KanbanBoard.WebApi.Services;
using KanbanBoard.WebApi.V1.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KanbanBoard.WebApi.V1.Controllers
{

    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:ApiVersion}/boards/{boardId}/members")]
    public class BoardMembersController : V1ControllerBase
    {
        private readonly IBoardRepository _boardRepository;
        private readonly IDateTimeProvider _dateTimeProvider;

        public BoardMembersController(
            IBoardRepository boardRepository,
            IDateTimeProvider dateTimeProvider)
        {
            _boardRepository = boardRepository;
            _dateTimeProvider = dateTimeProvider;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BoardMemberViewModel>>> Index(int boardId)
        {
            bool boardExists = await _boardRepository.ExistsBoard(boardId);
            if (!boardExists)
            {
                return V1NotFound("Board not found");
            }

            IEnumerable<BoardMember> members = await _boardRepository.GetAllBoardMembers(boardId);

            int userId = int.Parse(HttpContext.User.Identity.Name);
            if (!members.Any(member => member.User.Id == userId))
            {
                return Forbid();
            }

            IEnumerable<BoardMemberViewModel> membersViewModel = members.Select(member => new BoardMemberViewModel
            {
                Id = member.User.Id,
                Name = member.User.Name,
                Email = member.User.Email,
                IsAdmin = member.IsAdmin
            });

            return Ok(membersViewModel);
        }

        [HttpPost]
        public async Task<ActionResult<BoardViewModel>> Create(PostBoardViewModel model)
        {
            return NoContent();
        }

        [HttpDelete("{memberId}")]
        public async Task<ActionResult> Delete(int boardId, int memberId)
        {
            return NoContent();
        }
    }
}
