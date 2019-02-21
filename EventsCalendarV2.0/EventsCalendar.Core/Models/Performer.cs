using System.Collections.Generic;

namespace EventsCalendar.Core.Models
{
    public sealed class Performer
    {
        private CustomImage _performerImage;

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string TourName { get; set; }
        public bool IsActive { get; set; }
        public int? GenreId { get; set; }
        public Genre Genre { get; set; }
        public string ImageUrl { get; set; }
        //public int CustomImageId { get; set; }
        //public CustomImage CustomImage { get; set; }
        public Topic Topic { get; set; }
        public int? TopicId { get; set; }
        public int PerformerTypeId { get; set; }
        public PerformerType PerformerType { get; set; }
        public ICollection<Performance> Performances { get; set; }

        public Performer()
        {
            Performances = new List<Performance>();
        }
    }
}