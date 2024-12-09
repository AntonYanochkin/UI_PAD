namespace UI.Models
{
    public enum DifficultyLevel
    {
        Easy,
        Medium,
        Hard
    }
    public class WorkoutProgram
    {
        //public int Id { get; set; }
        //public string Name { get; set; }
        //public string Description { get; set; }
        //public string ImageUrl { get; set; }
        //public string ImageAltText { get; set; }




        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DifficultyLevel Level { get; set; }  // Обратите внимание на тип данных
        public int DurationInMinutes { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UserId { get; set; }  // Обратите внимание на тип данных

    }
}
