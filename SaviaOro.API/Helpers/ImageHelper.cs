namespace SaviaOro.API.Helpers
{
    public class ImageHelper : IImageHelper
    {
        public void DeleteImage(string file, string folder)
        {
            string path = Path.Combine(
                $"wwwroot\\images\\{folder}",
                file);

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public string UploadImageAsync(byte[] imageFile, string folder)
        {
            CheckImageContainer(folder);

            string guid = Guid.NewGuid().ToString();
            string file = $"{guid}.jpg";
            string path = Path.Combine(
                Directory.GetCurrentDirectory(),
                $"wwwroot\\images\\{folder}",
                file);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                using (BinaryWriter bw = new BinaryWriter(stream))
                {
                    bw.Write(imageFile);
                    bw.Close();
                }

            }

            return $"~/images/{folder}/{file}";
        }

        private void CheckImageContainer(string folder)
        {
            if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\images\\{folder}")))
            {
                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\images\\{folder}"));
            }
        }
    }
}
