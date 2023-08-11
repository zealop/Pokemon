namespace Game.Battles
{
    public class ActiveRequest
    {
        public bool Wait { get; set; }
        public bool TeamPreview { get; set; }
        public bool ForceSwitch { get; set; }

        public bool[] Active { get; set; }
    }
}