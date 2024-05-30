using System.ComponentModel.DataAnnotations;

namespace TrainFoodDelivery.Models
{
    public class Video
    {
        [Key]
        public string Path { get; set; }
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
    }
}
