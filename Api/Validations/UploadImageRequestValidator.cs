namespace EcommerceApi.Validations
{
    public class UploadImageRequestValidator : AbstractValidator<UploadImageRequest>
    {
        public UploadImageRequestValidator() 
        {
             RuleFor(x => x)
            .Must(request => request.Image.Length <= FileSettings.MaxFileSizeInBytes)
            .WithMessage($"Max file size is {FileSettings.MaxFileSizeInMB} MB.")
            .When(x => x.Image is not null);

            RuleFor(x => x)
            .Must((request, context) =>
            {
                BinaryReader binary = new(request.Image.OpenReadStream());
                var bytes = binary.ReadBytes(2);

                var fileSequenceHex = BitConverter.ToString(bytes);

                foreach (var signature in FileSettings.BlockedSignatures)
                    if (signature.Equals(fileSequenceHex, StringComparison.OrdinalIgnoreCase))
                        return false;

                return true;
            })
            .WithMessage("Not allowed file content")
            .When(x => x.Image is not null);

            RuleFor(x => x.Image)
            .Must((request, context) =>
            {
                var extension = Path.GetExtension(request.Image.FileName.ToLower());
                return FileSettings.AllowedImagesExtensions.Contains(extension);
            })
            .WithMessage("File extension is not allowed")
            .When(x => x.Image is not null);
        }



    }


}