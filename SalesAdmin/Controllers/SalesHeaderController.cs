namespace SalesAdmin.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SalesAdmin.Data;
    using SalesAdmin.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class SalesHeaderController : CustomControllerBase
    {
        private readonly ISalesHeaderRepository _repo;
        private readonly IMapper _mapper;

        public SalesHeaderController(
            ISalesHeaderRepository salesHeaderRepository,
            IMapper mapper)
        {
            _repo = salesHeaderRepository;
            _mapper = mapper;
        }

        [HttpPost("create")]
        public IActionResult Create(
            string no, 
            string description = "")
        {
            var userId = GetUserId();
            var existingSalesHeader = _repo.GetSalesHeader(no);

            if (existingSalesHeader != null)
            {
                return BadRequest();
            }

            var salesHeader = new SalesHeader
            {
                No = no,
                Description = description,
                CreatedDate = DateTime.UtcNow.Date,
                LastModifiedDate = DateTime.UtcNow.Date,
                CreatedByUserId = userId
            };

            _repo.CreateSalesHeader(salesHeader);

            var result = _mapper.Map<SalesHeaderResponse>(salesHeader);
            return Ok(result);
        }

        [HttpGet("")]
        public IActionResult List(int? page = null, int? pageSize = 10)
        {
            var salesHeaders = _repo.GetSalesHeaders(page, pageSize);

            var result = new SalesHeaderListResponse
            {
                SalesHeaders = _mapper.Map<SalesHeaderResponse[]>(salesHeaders)
            };

            return Ok(result);
        }
    }
}
