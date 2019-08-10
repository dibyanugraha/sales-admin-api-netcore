namespace SalesAdmin.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SalesAdmin.Data;
    using SalesAdmin.Models;
    using System;

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class SalesLineController : CustomControllerBase
    {
        private readonly ISalesLineRepository _repo;
        private readonly IMapper _mapper;

        public SalesLineController(ISalesLineRepository repo, IMapper mapper)
        {

        }
    }
}
