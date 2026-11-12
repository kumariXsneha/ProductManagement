namespace ProductManagement.API.Patterns
{
    // SINGLETON PATTERN (basic version — NOT thread-safe yet, we'll fix that in 5.3/5.4)
    public sealed class ApiStatistics // sealed- no one can inherit from this class 
    {
        // ② The one-and-only instance (static = belongs to the class, shared by everyone)
        private static ApiStatistics? _instance; // for a box created singal class,nothing created yet null hain 

        // For the demo: count how many times the constructor runs (should be 1!)
        public static int InstancesCreated { get; private set; } = 0;

        public DateTime StartedAt { get; }
        public int TotalRequests { get; private set; }

        // ① PRIVATE constructor: nobody outside this class can call "new ApiStatistics()"
        private ApiStatistics() // only code inside this class can call new
        {
            InstancesCreated++;
            StartedAt = DateTime.Now;
        }

        // ③ The ONLY way to get the object
        public static ApiStatistics Instance // the global access point 
                                             //static field belongs to the class itself, so there's exactly one copy, shared by the whole app.
        {
            get
            {
                if (_instance == null)              // first time? create it
                {
                    _instance = new ApiStatistics();
                }
                return _instance;                   // every time: return the same one
            }
        }

        public void RecordRequest()
        {
            TotalRequests++;
        }
    }
}