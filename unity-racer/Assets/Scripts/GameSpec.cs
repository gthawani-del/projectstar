using System;

namespace ProjectStar.Racer
{
    [Serializable]
    public class RacerGameSpec
    {
        public string template;
        public string location;
        public string track;
        public string weather;
        public string timeOfDay;
        public string vehicle;
        public int laps;
        public int opponents;
        public bool traffic;
        public string difficulty;
        public string[] brandingSlots;
        public string qualityTarget;
        public int mobileTargetFps;
        public int desktopTargetFps;
        public string[] visualPriorities;
    }
}
