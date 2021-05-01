using AutoMapper;
using Core.Api.ViewModels;
using Core.Business.Intefaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Core.Api.Controllers
{
    [Authorize(Roles = "admin")]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/manageuserclaims")]
    public class ManageUserClaimsController : MainController
    {
        private readonly IMapper _mapper;
        private readonly IAppControllerRepository _appControllerRepository;
        private readonly IAppActionRepository _appActionRepository;
        private readonly IDapperDbRepository _dapperDbRepository;
        private readonly UserManager<IdentityUser> _userManager;
        

        public ManageUserClaimsController(UserManager<IdentityUser> userManager,
                                          IAppControllerRepository appControllerRepository,
                                          IAppActionRepository appActionRepository,
                                          IDapperDbRepository dapperDbRepository,
                                          IMapper mapper,
                                          INotifier notifier, IUser user) : base(notifier, user)
        {
            _userManager = userManager;
            _appControllerRepository = appControllerRepository;
            _appActionRepository = appActionRepository;
            _dapperDbRepository = dapperDbRepository;
            _mapper = mapper;
        }

        [HttpGet("get-all-users")]
        public async Task<IEnumerable<AllUsersViewModel>> GetAllUsers()
        {
            return _mapper.Map<IEnumerable<AllUsersViewModel>>(await _dapperDbRepository.GetAllUsers());
        }

        [HttpGet("get-claims-byuser/{id:guid}")]
        public async Task<IEnumerable<UserClaimsViewModel>> GetClaimsByUser(Guid id)
        {
            return _mapper.Map<IEnumerable<UserClaimsViewModel>>(await _dapperDbRepository.GetAllUserClaimsByUserId(id.ToString()));
        }


        [HttpGet("get-all-controllers")]
        public async Task<IEnumerable<AppControllerViewModel>> GetAllControllers()
        {
            return _mapper.Map<IEnumerable<AppControllerViewModel>>(await _appControllerRepository.GetAll());
        }

        [HttpGet("get-all-actions")]
        public async Task<IEnumerable<AppActionViewModel>> GetAllActions()
        {
            return _mapper.Map<IEnumerable<AppActionViewModel>>(await _appActionRepository.GetAll());
        }

        [HttpGet("get-actions-bycontroller/{id:guid}")]
        public async Task<IEnumerable<AppActionViewModel>> GetActionsByController(Guid id)
        {
            return _mapper.Map<IEnumerable<AppActionViewModel>>(await _appActionRepository.GetActionsByController(id));
        }


        [HttpPost]
        public async Task<ActionResult<ControllerActionsViewModel>> CreateClaims(ControllerActionsViewModel controllerActionsViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            var user = await _userManager.FindByIdAsync(controllerActionsViewModel.UserId);
            var ClaimsOfTheUser = await _userManager.GetClaimsAsync(user);

            if (ClaimsOfTheUser.Any(c => c.Type == controllerActionsViewModel.ControllerName && c.Value==controllerActionsViewModel.ActionName))
            {
                NotifyError("Já existe esta claim para este utilizador.");
                return CustomResponse(controllerActionsViewModel);
            }
            else
            {
                await _userManager.AddClaimAsync(user, new Claim(controllerActionsViewModel.ControllerName, controllerActionsViewModel.ActionName));
            }
            
            return CustomResponse(controllerActionsViewModel);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<UserClaimsViewModel>> Delete(int id)
        {
            var userClaimViewModel = _mapper.Map<UserClaimsViewModel>(await _dapperDbRepository.GetUserClaimsById(id));
            if (userClaimViewModel == null) return NotFound();

            var result = await _userManager.RemoveClaimAsync(_userManager.FindByIdAsync(userClaimViewModel.UserId).Result, new Claim(userClaimViewModel.ClaimType, userClaimViewModel.ClaimValue));

            if(!result.Succeeded) return NotFound();

            return CustomResponse(userClaimViewModel);
        }
    }

}

