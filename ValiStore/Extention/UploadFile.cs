namespace ValiStore.Extention
{
    public class UploadFile
    {
        public static async Task<string> UploadFileImage(IFormFile file, string directory, string newName = null)
        {
            try
            {
                if (newName == null)
                {
                    newName = file.FileName;
                }
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ProductsImages", directory);
                string pathFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ProductsImages", directory, newName);
                var supportedTypes = new[] { "png", "jpg", "jpeg", "gif" };
                var fileExt = Path.GetExtension(file.FileName).Substring(1);
                if (!supportedTypes.Contains(fileExt.ToLower()))
                {
                    return null;
                }
                else
                {
                    using (var stream = new FileStream(pathFile, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    return newName;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}

