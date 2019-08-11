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
            _repo = repo;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult Create(
            string documentNo,
            int lineNo)
        {
            var userId = GetUserId();
            var existingSalesLine = _repo.GetSalesLine(documentNo, lineNo);

            if (existingSalesLine != null)
            {
                return BadRequest();
            }

            var salesLine = new SalesLine
            {
                DocumentNo = documentNo,
                LineNo = lineNo,
                CreatedDateTime = DateTime.Today,
                LastModifiedDateTime = DateTime.Today,
                CreatedByUserId = userId
            };

            _repo.CreateSalesLine(salesLine);

            var result = _mapper.Map<SalesLineResponse>(salesLine);
            return Ok(result);
        }
        
        [HttpGet("")]
        public IActionResult List(string documentNo)
        {
            var salesLines = _repo.GetSalesLines(new SalesLine
            {
                DocumentNo = documentNo
            });

            var result = new SalesLineListResponse
            {
                SalesLines = _mapper.Map<SalesLineResponse[]>(salesLines)
            };

            return Ok(result);
        }
    }
}
