namespace GymGenius.Helpers
{
    public static class UploadPhoto
    {
        public static string SaveFileAsync(IFormFile FileUrl, string FoloderPath)
        {

            // Get Directory
            string FilePath = Directory.GetCurrentDirectory() + "/wwwroot/" + FoloderPath;

            // Get File Name
            string FileName = Guid.NewGuid() + Path.GetFileName(FileUrl.FileName); // to avoid the token and replace

            // Merge The Directory With File Name 
            string FinalPath = Path.Combine(FilePath, FileName);

            // Save Your File As Stream "Data Overtime" 
            using (var Stream = new FileStream(FinalPath, FileMode.Create))
            {
                FileUrl.CopyTo(Stream);
            }

            return FileName;
        }
    }
}
