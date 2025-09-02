namespace PROGPOEDH.Models
{
    public class FormModel
    {
       // public int id { get; set; }
        public string Location { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public byte[] Document {  get; set; }
        public byte[] Picture { get; set; }

        public string viewPicture {  get; set; }
    }
}
