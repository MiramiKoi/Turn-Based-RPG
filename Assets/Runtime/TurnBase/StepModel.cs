using Runtime.CustomAsync;

namespace Runtime.TurnBase
{
    public class StepModel
    {
        public StepType StepType { get; }
        
        public CustomAwaiter AllowedAwaiter { get; }
        
        public CustomAwaiter CompletedAwaiter { get; }
        
        public StepModel(StepType stepType)
        {
            StepType = stepType;
            AllowedAwaiter = new CustomAwaiter();
            CompletedAwaiter = new CustomAwaiter();
        }
    }
}