namespace SalesAdmin.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SalesAdmin.Data;
    using SalesAdmin.Models.SalesHeader;
    using System;
    using System.Collections.Generic;
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
        public async Task<IActionResult> Create(
            [FromBody] SalesHeaderRequest request)
        {
            var userId = GetUserId();
            var existingSalesHeader = await _repo.GetSalesHeaderAsync(request.No);

            if (existingSalesHeader != null)
            {
                return BadRequest();
            }

            var salesHeader = new SalesHeader
            {
                No = request.No,
                CreatedDateTime = DateTime.Today.ToUniversalTime(),
                LastModifiedDateTime = DateTime.Today.ToUniversalTime(),
                CreatedByUserId = userId
            };

            await _repo.CreateSalesHeaderAsync(salesHeader);

            var result = _mapper.Map<SalesHeaderResponse>(salesHeader);
            return Ok(result);
        }

        [HttpGet("{no}")]
        public async Task<ActionResult<SalesHeader>> GetByDocumentNo(string no)
        {
            return await _repo.GetSalesHeaderAsync(no);
        }

        [HttpGet]
        public async Task<IActionResult> List(int? page = null, int? pageSize = 10)
        {
            var salesHeaders = await _repo.GetSalesHeadersAsync(page, pageSize);

            var result = new SalesHeaderListResponse
            {
                SalesHeaders = _mapper.Map<IEnumerable<SalesHeaderResponse>>(salesHeaders)
            };

            return Ok(result);
        }

        [HttpDelete("{no}")]
        public async Task<IActionResult> Delete(string no)
        {
            var salesHeader = _repo.GetSalesHeaderAsync(no);

            if (salesHeader == null)
            {
                return NotFound();
            }

            await _repo.DeleteSalesHeaderAsync(no);

            return NoContent();
        }
    }
}
