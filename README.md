### EqualsHashCodeExpr

Build `Equals` and `GetHashCode` Func using Linq Expressions dynamically with support of complex data types

```csharp
Func<ParentModel, ParentModel, bool> equalsFunc = new EqualsBuilder().BuildFunc<ParentModel>();

Func<ParentModel, int> hashCodeFunc = new HashCodeBuilder().BuildFunc<ParentModel>();
```

Or create a instance of `EqualityComparer`:
```csharp
IEqualityComparer<ParentModel> comparer = EqualityComparerBuilder.New<ParentModel>();
```

Or add `EqualityComparer` methods to your model class:
```csharp
public class ParentModel : EqualityComparerBase<ParentModel>
{
    public string Property1 { get; set; }

    public double Property2 { get; set; }
    
    public int Property3 { get; set; }

    public NestedModel NestedRef { get; set; }
}
```
