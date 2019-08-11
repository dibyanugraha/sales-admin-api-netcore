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

        public SalesHeaderController(ISalesHeaderRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpPost]
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
                CreatedDateTime = DateTime.Today,
                LastModifiedDateTime = DateTime.Today,
                CreatedByUserId = userId
            };

            _repo.CreateSalesHeader(salesHeader);

            var result = _mapper.Map<SalesHeaderResponse>(salesHeader);
            return Ok(result);
        }

        [HttpGet("{no}")]
        public async Task<ActionResult<SalesHeader>> GetByDocumentNo(string no)
        {
            return await _repo.GetSalesHeader(no);
        }

        [HttpGet]
        public IActionResult List(int? page = null, int? pageSize = 10)
        {
            var salesHeaders = _repo.GetSalesHeaders(page, pageSize);

            var result = new SalesHeaderListResponse
            {
                SalesHeaders = _mapper.Map<SalesHeaderResponse[]>(salesHeaders)
            };

            return Ok(result);
        }

        [HttpDelete("{no}")]
        public IActionResult Delete(string no)
        {
            var salesHeader = _repo.GetSalesHeader(no);

            if (salesHeader == null)
            {
                return NotFound();
            }

            _repo.DeleteSalesHeader(no);

            return NoContent();
        }
    }
}
