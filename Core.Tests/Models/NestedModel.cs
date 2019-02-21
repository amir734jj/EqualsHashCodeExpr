namespace Core.Tests.Models
{
    public class NestedModel
    {
        public int PropertyX { get; set; }
        
        public string PropertyY { get; set; }
        
        public ParentModel ParentRef { get; set; }
    }
}