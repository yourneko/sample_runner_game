namespace Runner.Game
{
    // all these parameters are expected to be more editable and accessible, but...
    static class Configuration
    {
        public const float BLOCK_LENGTH = 500f;
        public const float SINGLE_TRACK_WIDTH = 50f;
        public const int MIN_BLOCKS_IN_LOCATION = 5;
        public const int MAX_BLOCKS_IN_LOCATION = 12;
        public const int OBJECT_ROWS_IN_BLOCK = 20;
        public const float DISTANCE_BETWEEN_OBJECT_ROWS = BLOCK_LENGTH / OBJECT_ROWS_IN_BLOCK;
        
        public const int LEFT_TRACK_INDEX = 0;
        public const int RIGHT_TRACK_INDEX = 2;
        public const int DEFAULT_TRACK_INDEX = 1;
        public const int TRACK_COUNT = RIGHT_TRACK_INDEX - LEFT_TRACK_INDEX + 1;

        public const float PRELOAD_DISTANCE = 1000f;
        public const float UNLOAD_DISTANCE = -200f;
        public const int PRELOAD_ON_START_COUNT = (int)(PRELOAD_DISTANCE / BLOCK_LENGTH);

        public const float INITIAL_SPEED = 10f;
        public const float MAX_SPEED = 50f;
        public const float ASYMPTOTIC_SPEED_GAIN_PER_FRAME = 0.0001f;

        public const string CACHED_BLOCKS_DATA_NAME = "CachedBlocksData";
        public const string LOCATION_BLOCK_PREFABS_PATH = "Assets/Prefabs/Blocks/";
        public const string BLOCK_ELEMENTS_PREFABS_PATH = "Assets/Prefabs/Elements/";

        public static readonly int[][] ObjectSpawnSequences = {
                                                                  new[] {0} //todo: create some data
                                                              };
    }
}
