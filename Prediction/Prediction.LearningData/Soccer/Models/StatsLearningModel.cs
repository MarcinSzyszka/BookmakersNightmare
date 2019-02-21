namespace Prediction.LearningData.Soccer.Models
{
    internal class StatsLearningModel
    {
        public float Result { get; set; }
        
        public float HostsBallPossession { get; set; }
           
        public float HostsAttacksOnGoal { get; set; }
            
        public float HostsShotsOnGoal { get; set; }
              
        public float HostsShotsOutGoal { get; set; }
             
        public float HostsCorners { get; set; }
            
        public float HostsAccuratePasses { get; set; }

        public float HostsBlocks { get; set; }

        public float GuestsBallPossession { get; set; }
            
        public float GuestsAttacksOnGoal { get; set; }
            
        public float GuestsShotsOnGoal { get; set; }
            
        public float GuestsShotsOutGoal { get; set; }
           
        public float GuestsCorners { get; set; }
       
        public float GuestsAccuratePasses { get; set; }
          
        public float GuestsBlocks { get; set; }

        //public int HostsResultPoints { get; set; }

        //public int GuestsResultPoints { get; set; }
    }
}
