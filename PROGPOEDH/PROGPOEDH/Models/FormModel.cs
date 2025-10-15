namespace PROGPOEDH.Models
{
    public class FormModel
    {
       
        public string Location { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public byte[] Document {  get; set; }
        public byte[] Picture { get; set; }
        public string viewPicture {  get; set; }

        public IFormFile PictureFile { get; set; }
        public IFormFile DocumentFile { get; set; }
        public string viewDocument { get; set; }


    }
}
