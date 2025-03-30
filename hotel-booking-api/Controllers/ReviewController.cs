using hotel_booking_core.Interfaces;
using hotel_booking_dto;
using hotel_booking_dto.ReviewDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;


namespace hotel_booking_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ReviewController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IReviewService _reviewService;
        public ReviewController(IReviewService reviewService, ILogger logger)
        {
            _reviewService = reviewService;
            _logger = logger;

        }

        [HttpPatch("update")]
        [Authorize]
        public async Task<IActionResult> UpdateReview([FromQuery] string reviewId, [FromBody] ReviewUpdateDto reviewUpdateDto)
        {
            var customerId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
           
            var response = await _reviewService.UpdateReviewAsync(reviewId, customerId, reviewUpdateDto);
            return StatusCode(response.Code, response);
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> AddReviews([FromBody] AddReviewDto model)
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _reviewService.AddReviewAsync(model, customerId);
            return StatusCode(result.Code, result);
        }

        [HttpDelete("delete")]
        public ActionResult<Response<string>> DeleteReviews(string reviewId)
        {
            var customerId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var response = _reviewService.DeleteReviewAsync(customerId, reviewId);

            return Ok(response);

        }
    }
}
