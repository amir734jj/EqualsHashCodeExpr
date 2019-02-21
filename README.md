### EqualsHashCodeExpr

Build `Equals` and `GetHashCode` Func using Linq Expressions dynamically with support of complex data types

```csharp
Func<ParentModel, ParentModel, bool> equalsFunc = new EqualsBuilder().BuildFunc<ParentModel>();

Func<ParentModel, int> hashCodeFunc = new HashCodeBuilder().BuildFunc<ParentModel>();
```