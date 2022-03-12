using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreWebApp.CustomValidation
{
    public class SingleFileValidate:ValidationAttribute
    {
        private readonly string[] _extensions;
        public SingleFileValidate(string[] Extensions)
        {
            _extensions = Extensions;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //Using Obj as IFormFile
            var file = value as IFormFile;
            var multiplefile = value as IFormFileCollection;
            if (file != null)
            {
                //Extracting Extension from the fileName of file
                var extension = Path.GetExtension(file.FileName);
                if (_extensions.Contains(extension.ToLower()))
                {
                    return ValidationResult.Success;
                }
            }
            return new ValidationResult(GetErrorMessage());
        }
        public string GetErrorMessage()
        {
            return $"This Type of File is Not Allowed";
        }        

    }
}
