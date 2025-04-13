namespace Careblock.Model.Web.Account;

public class GrantSignRequest
{
    public Guid? OriginId { get; set; }
    public Guid TargetId { get; set; }

}