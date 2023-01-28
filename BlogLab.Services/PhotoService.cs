using BlogLab.Models.Settings;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogLab.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;

        public PhotoService(IOptions<Cloudinaryoptions> config)
        {
            var account = new Account(config.Value.CloudName, config.Value.ApiKey, config.Value.ApiSecret);
            _cloudinary= new Cloudinary(account);
        }
        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile formFile)
        {
           var uploadResult= new ImageUploadResult();   
            if(formFile.Length > 0)
            {
                using(var stream = formFile.OpenReadStream()) 
                {
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(formFile.Name, stream),
                        Transformation = new Transformation().Height(300).Width(500).Crop("fill")
                    };
                    uploadResult = await _cloudinary.UploadAsync(uploadParams);
                }
               
            }
            return uploadResult;
        }

        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            var deletionParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deletionParams);
            return result;
        }
    }
}
