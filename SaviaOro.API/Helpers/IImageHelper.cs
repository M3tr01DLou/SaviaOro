namespace SaviaOro.API.Helpers
{
    public interface IImageHelper
    {
        string UploadImageAsync(byte[] imageFile, string folder);

        void DeleteImage(string file, string folder);
    }
}
