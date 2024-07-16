
    
        // FeedbackI class representing individual feedback item
        public class FeedbackI
        {
            public string _id { get; set; }
            public string studentId { get; set; }
            public string subjectId { get; set; }
            public int rating { get; set; }
            public string feedback { get; set; }
            public string timestamp { get; set; }
           // public string? ProfileUrl { get; set; }
            //public string? studentName { get; set; }
        }

        // ReviewResponse class representing response containing reviews and average rating
        public class ReviewResponse
        {
            public List<FeedbackI> reviews { get; set; }
            public double averageRating { get; set; }
        }
    