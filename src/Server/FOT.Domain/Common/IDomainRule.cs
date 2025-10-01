namespace FOT.Domain.Common;

public interface IDomainRule<in TDomain>
{
    bool IsSatisfied { get; }
    
    string ErrorMessage { get; }
}