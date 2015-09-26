namespace mojoPortal.Business.WebHelpers.PaymentGateway
{
    public enum PaymentGatewayResponse
    {
        Approved = 1,
        Declined = 2,
        Pending = 3,
        Error = 5,
        NoRequestInitiated = 11
    }

}
