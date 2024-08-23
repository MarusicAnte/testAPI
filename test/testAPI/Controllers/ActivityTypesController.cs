﻿using eStudent.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using testAPI.Constants;
using testAPI.Interfaces;
using testAPI.Models.DTO.ActivityTypeDtos;

namespace testAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityTypesController : BaseController
    {
        private readonly IActivityTypeService _activityTypeService;
        public ActivityTypesController(IActivityTypeService activityTypeService)
        {
            _activityTypeService = activityTypeService;
        }


        [Authorize(Policy = RolesConstant.AnyUserRole)]
        [HttpGet]
        public async Task<List<ActivityTypeDto>> GetAll()
        {
            return await _activityTypeService.GetAllActivityTypes();
        }


        [Authorize(Policy = RolesConstant.AnyUserRole)]
        [HttpGet("{id}")]
        public async Task<ActivityTypeDto> GetById([FromRoute] int id)
        {
            return await _activityTypeService.GetActivityTypeById(id);
        }


        [Authorize(Policy = RolesConstant.Administrator)]
        [HttpPost]
        public async Task<ActivityTypeDto> Create([FromBody] CreateActivityTypeDto createActivityTypeDto)
        {
            return await _activityTypeService.CreateActivityType(createActivityTypeDto);
        }


        [Authorize(Policy = RolesConstant.Administrator)]
        [HttpPatch("{id}")]
        public async Task<ActivityTypeDto> UpdateById([FromRoute] int id, [FromBody] UpdateActivityTypeDto updateActivityTypeDto)
        {
            return await _activityTypeService.UpdateActivityTypeById(id, updateActivityTypeDto);
        }


        [Authorize(Policy = RolesConstant.Administrator)]
        [HttpDelete("{id}")]
        public async Task<ActivityTypeDto> DeleteById([FromRoute] int id)
        {
            return await _activityTypeService.DeleteActivityTypeById(id);
        }
    }
}
