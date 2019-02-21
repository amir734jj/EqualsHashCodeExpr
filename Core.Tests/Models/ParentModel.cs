namespace Core.Tests.Models
{
    public class ParentModel
    {
        public string Property1 { get; set; }

        public double Property2 { get; set; }
        
        public int Property3 { get; set; }

        public NestedModel NestedRef { get; set; }
    }
}