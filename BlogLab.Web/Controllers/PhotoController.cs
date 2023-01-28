using BlogLab.Models.Photo;
using BlogLab.Repository;
using BlogLab.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;

namespace BlogLab.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        private readonly IPhotoRepository _photoRepository;
        private readonly IPhotoService _photoService;
        private readonly IBlogRepository _blogRepository;

        public PhotoController(IPhotoRepository photoRepository, IPhotoService photoService, IBlogRepository blogRepository)
        {
            _photoRepository = photoRepository;
            _photoService = photoService;
            _blogRepository = blogRepository;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Photo>> UploadPhoto(IFormFile file)
        {
            int applicationUserId = int.Parse(User.Claims.First(i => i.Type == JwtRegisteredClaimNames.NameId).Value);

            var uploadResult = await _photoService.AddPhotoAsync(file);
            if (uploadResult.Error != null) return BadRequest(uploadResult.Error.Message);

            var photoCreate = new PhotoCreate
            {
                PublicId = uploadResult.PublicId,
                ImageUrl = uploadResult.SecureUrl.AbsoluteUri,
                Description = file.FileName
            };

            var photo = await _photoRepository.InsertAsync(photoCreate, applicationUserId);
            return Ok(photo);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<Photo>>> GetByApplicationUserId()
        {
            int applicationUserId = int.Parse(User.Claims.First(i => i.Type == JwtRegisteredClaimNames.NameId).Value);
            var photo = await _photoRepository.GetAllByUserIdAsync(applicationUserId);
            return Ok(photo);
        }

        [HttpGet("{photoId}")]
        public async Task<ActionResult<Photo>> Get(int photoId)
        {
            var photo= await _photoRepository.GetAsync(photoId);
            return Ok(photo);
        }

        [HttpDelete("{photoId}")]
        public async Task<ActionResult<int>> Delete(int photoId)
        {
            int applicationUserId = int.Parse(User.Claims.First(i => i.Type == JwtRegisteredClaimNames.NameId).Value);
            var foundPhoto = await _photoRepository.GetAsync(photoId);
            var affectedRows=0;
            if (foundPhoto!= null)
            {
                if(foundPhoto.ApplicationUserId== applicationUserId)
                {
                    var blogs = await _blogRepository.GetAllByUserIdAsync(applicationUserId);
                    var usedinBlog = blogs.Any(b => b.PhotoId == photoId);

                    if (usedinBlog) return BadRequest("Can't remove photo as it's used in published blog");

                    var deleteResult = await _photoService.DeletePhotoAsync(foundPhoto.PublicId);

                    if(deleteResult.Error != null) return BadRequest(deleteResult.Error.Message);

                    affectedRows = await _photoRepository.DeletePhotoAsync(foundPhoto.PhotoId);

                   
                }
                return Ok(affectedRows);
            }
            else
            {
                return BadRequest("Photo was not uploaded by the user");
            }
        }

    }
}
