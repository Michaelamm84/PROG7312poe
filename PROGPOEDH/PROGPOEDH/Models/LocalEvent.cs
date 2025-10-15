namespace PROGPOEDH.Models
{
    public class LocalEvent
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }  // for priority queue

        public override string ToString()
        {
            return $"[Event ID: {Id}] {Title} ({Category})\n" +
                   $"Date: {Date:yyyy-MM-dd HH:mm}\n" +
                   $"Priority: {Priority}\n" +
                   $"Description: {Description}";
        }
    }
}
